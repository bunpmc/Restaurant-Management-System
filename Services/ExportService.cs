using BusinessObjects;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Services
{
    public class ExportService : IExportService
    {
        public ExportService()
        {
            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public bool ExportStockToExcel(List<Stock> stocks, string filePath)
        {
            try
            {
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Stock Inventory");

                // Add headers
                var headers = new string[]
                {
                    "ID", "Item Name", "Category", "Quantity", "Unit", "Unit Price",
                    "Total Value", "Supplier", "Minimum Stock", "Expiry Date", "Status", "Notes"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                }

                // Add data
                for (int i = 0; i < stocks.Count; i++)
                {
                    var stock = stocks[i];
                    var row = i + 2;

                    worksheet.Cells[row, 1].Value = stock.Id;
                    worksheet.Cells[row, 2].Value = stock.ItemName;
                    worksheet.Cells[row, 3].Value = stock.Category;
                    worksheet.Cells[row, 4].Value = stock.Quantity;
                    worksheet.Cells[row, 5].Value = stock.Unit;
                    worksheet.Cells[row, 6].Value = stock.UnitPrice;
                    worksheet.Cells[row, 7].Value = stock.TotalValue;
                    worksheet.Cells[row, 8].Value = stock.Supplier;
                    worksheet.Cells[row, 9].Value = stock.MinimumStock;
                    worksheet.Cells[row, 10].Value = stock.ExpiryDate?.ToString("MM/dd/yyyy");
                    worksheet.Cells[row, 11].Value = stock.StockStatus;
                    worksheet.Cells[row, 12].Value = stock.Notes;

                    // Color code status
                    var statusCell = worksheet.Cells[row, 11];
                    switch (stock.StockStatus)
                    {
                        case "Out of Stock":
                            statusCell.Style.Font.Color.SetColor(Color.Red);
                            break;
                        case "Low Stock":
                            statusCell.Style.Font.Color.SetColor(Color.Orange);
                            break;
                        case "Expiring Soon":
                            statusCell.Style.Font.Color.SetColor(Color.DarkOrange);
                            break;
                        case "In Stock":
                            statusCell.Style.Font.Color.SetColor(Color.Green);
                            break;
                    }
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                // Add summary
                var summaryRow = stocks.Count + 3;
                worksheet.Cells[summaryRow, 1].Value = "Summary:";
                worksheet.Cells[summaryRow, 1].Style.Font.Bold = true;
                worksheet.Cells[summaryRow + 1, 1].Value = $"Total Items: {stocks.Count}";
                worksheet.Cells[summaryRow + 2, 1].Value = $"Low Stock Items: {stocks.Count(s => s.StockStatus == "Low Stock")}";
                worksheet.Cells[summaryRow + 3, 1].Value = $"Out of Stock Items: {stocks.Count(s => s.StockStatus == "Out of Stock")}";
                worksheet.Cells[summaryRow + 4, 1].Value = $"Expiring Soon: {stocks.Count(s => s.StockStatus == "Expiring Soon")}";

                // Save file
                var fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExportStockToPdf(List<Stock> stocks, string filePath)
        {
            try
            {
                using var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                using var writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();

                // Add title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var title = new Paragraph("Sakana House - Stock Inventory Report", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(title);

                // Add generation date
                var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var dateText = new Paragraph($"Generated on: {DateTime.Now:MM/dd/yyyy HH:mm}", dateFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 20
                };
                document.Add(dateText);

                // Create table
                var table = new PdfPTable(8) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 1f, 2.5f, 1.5f, 1f, 1f, 1.2f, 1.5f, 1.5f });

                // Add headers
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                var headers = new string[] { "ID", "Item Name", "Category", "Qty", "Unit", "Price", "Supplier", "Status" };

                foreach (var header in headers)
                {
                    var cell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        BackgroundColor = BaseColor.LightGray,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    table.AddCell(cell);
                }

                // Add data
                var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                foreach (var stock in stocks)
                {
                    table.AddCell(new PdfPCell(new Phrase(stock.Id.ToString(), dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(stock.ItemName, dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(stock.Category, dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(stock.Quantity.ToString(), dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(stock.Unit, dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(stock.UnitPrice?.ToString("C") ?? "", dataFont)) { Padding = 3 });
                    table.AddCell(new PdfPCell(new Phrase(stock.Supplier, dataFont)) { Padding = 3 });

                    // Status with color
                    var statusCell = new PdfPCell(new Phrase(stock.StockStatus, dataFont)) { Padding = 3 };
                    switch (stock.StockStatus)
                    {
                        case "Out of Stock":
                            statusCell.BackgroundColor = new BaseColor(255, 192, 203); // Pink
                            break;
                        case "Low Stock":
                            statusCell.BackgroundColor = BaseColor.Yellow;
                            break;
                        case "Expiring Soon":
                            statusCell.BackgroundColor = BaseColor.Orange;
                            break;
                    }
                    table.AddCell(statusCell);
                }

                document.Add(table);

                // Add summary
                document.Add(new Paragraph("\n"));
                var summaryFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                document.Add(new Paragraph("Summary:", summaryFont));

                var summaryText = $"Total Items: {stocks.Count}\n" +
                                 $"Low Stock Items: {stocks.Count(s => s.StockStatus == "Low Stock")}\n" +
                                 $"Out of Stock Items: {stocks.Count(s => s.StockStatus == "Out of Stock")}\n" +
                                 $"Expiring Soon: {stocks.Count(s => s.StockStatus == "Expiring Soon")}";

                document.Add(new Paragraph(summaryText, dataFont));

                document.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExportLowStockReportToExcel(List<Stock> lowStockItems, string filePath)
        {
            try
            {
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Low Stock Report");

                // Add title
                worksheet.Cells[1, 1].Value = "SAKANA HOUSE - LOW STOCK REPORT";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1, 1, 6].Merge = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells[2, 1].Value = $"Generated on: {DateTime.Now:MM/dd/yyyy HH:mm}";
                worksheet.Cells[2, 1, 2, 6].Merge = true;
                worksheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Add headers
                var headers = new string[] { "Item Name", "Current Stock", "Minimum Stock", "Shortage", "Unit", "Supplier" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[4, i + 1].Value = headers[i];
                    worksheet.Cells[4, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[4, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[4, i + 1].Style.Fill.BackgroundColor.SetColor(Color.Red);
                    worksheet.Cells[4, i + 1].Style.Font.Color.SetColor(Color.White);
                }

                // Add data
                for (int i = 0; i < lowStockItems.Count; i++)
                {
                    var stock = lowStockItems[i];
                    var row = i + 5;
                    var shortage = (stock.MinimumStock ?? 0) - stock.Quantity;

                    worksheet.Cells[row, 1].Value = stock.ItemName;
                    worksheet.Cells[row, 2].Value = stock.Quantity;
                    worksheet.Cells[row, 3].Value = stock.MinimumStock;
                    worksheet.Cells[row, 4].Value = shortage;
                    worksheet.Cells[row, 5].Value = stock.Unit;
                    worksheet.Cells[row, 6].Value = stock.Supplier;

                    // Highlight critical items (out of stock)
                    if (stock.Quantity == 0)
                    {
                        for (int col = 1; col <= 6; col++)
                        {
                            worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.LightPink);
                        }
                    }
                }

                worksheet.Cells.AutoFitColumns();

                var fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExportLowStockReportToPdf(List<Stock> lowStockItems, string filePath)
        {
            try
            {
                using var document = new Document(PageSize.A4, 25, 25, 30, 30);
                using var writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();

                // Add title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var title = new Paragraph("SAKANA HOUSE", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10
                };
                document.Add(title);

                var subtitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                var subtitle = new Paragraph("LOW STOCK ALERT REPORT", subtitleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(subtitle);

                // Add generation info
                var infoFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var info = new Paragraph($"Generated on: {DateTime.Now:MM/dd/yyyy HH:mm}\nTotal Low Stock Items: {lowStockItems.Count}", infoFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 20
                };
                document.Add(info);

                if (lowStockItems.Count == 0)
                {
                    var noItemsFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                    var noItems = new Paragraph("No low stock items found!", noItemsFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(noItems);
                }
                else
                {
                    // Create table
                    var table = new PdfPTable(6) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 3f, 1.5f, 1.5f, 1.5f, 1f, 2f });

                    // Add headers
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                    var headers = new string[] { "Item Name", "Current", "Minimum", "Shortage", "Unit", "Supplier" };

                    foreach (var header in headers)
                    {
                        var cell = new PdfPCell(new Phrase(header, headerFont))
                        {
                            BackgroundColor = BaseColor.Red,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 8
                        };
                        cell.Phrase.Font.Color = BaseColor.White;
                        table.AddCell(cell);
                    }

                    // Add data
                    var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                    foreach (var stock in lowStockItems)
                    {
                        var shortage = (stock.MinimumStock ?? 0) - stock.Quantity;

                        var itemCell = new PdfPCell(new Phrase(stock.ItemName, dataFont)) { Padding = 5 };
                        var currentCell = new PdfPCell(new Phrase(stock.Quantity.ToString(), dataFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER };
                        var minCell = new PdfPCell(new Phrase(stock.MinimumStock?.ToString() ?? "0", dataFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER };
                        var shortageCell = new PdfPCell(new Phrase(shortage.ToString(), dataFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER };
                        var unitCell = new PdfPCell(new Phrase(stock.Unit, dataFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER };
                        var supplierCell = new PdfPCell(new Phrase(stock.Supplier, dataFont)) { Padding = 5 };

                        // Highlight critical items (out of stock)
                        if (stock.Quantity == 0)
                        {
                            var pinkColor = new BaseColor(255, 192, 203); // Pink
                            itemCell.BackgroundColor = pinkColor;
                            currentCell.BackgroundColor = pinkColor;
                            minCell.BackgroundColor = pinkColor;
                            shortageCell.BackgroundColor = pinkColor;
                            unitCell.BackgroundColor = pinkColor;
                            supplierCell.BackgroundColor = pinkColor;
                        }

                        table.AddCell(itemCell);
                        table.AddCell(currentCell);
                        table.AddCell(minCell);
                        table.AddCell(shortageCell);
                        table.AddCell(unitCell);
                        table.AddCell(supplierCell);
                    }

                    document.Add(table);

                    // Add recommendations
                    document.Add(new Paragraph("\n"));
                    var recFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    document.Add(new Paragraph("Recommendations:", recFont));

                    var recommendations = "1. Contact suppliers immediately for out-of-stock items\n" +
                                        "2. Place orders for low stock items before they run out\n" +
                                        "3. Review minimum stock levels for frequently depleted items\n" +
                                        "4. Consider increasing safety stock for critical ingredients";

                    document.Add(new Paragraph(recommendations, infoFont));
                }

                document.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GenerateLowStockReportText(List<Stock> lowStockItems)
        {
            var report = "SAKANA HOUSE - LOW STOCK ALERT REPORT\n";
            report += "=====================================\n\n";
            report += $"Generated on: {DateTime.Now:MM/dd/yyyy HH:mm}\n";
            report += $"Total Low Stock Items: {lowStockItems.Count}\n\n";

            if (lowStockItems.Count == 0)
            {
                report += "✅ No low stock items found!\n";
                report += "All inventory levels are above minimum thresholds.\n";
            }
            else
            {
                report += "⚠️  ITEMS REQUIRING IMMEDIATE ATTENTION:\n";
                report += "----------------------------------------\n\n";

                foreach (var item in lowStockItems.OrderBy(x => x.Quantity))
                {
                    var shortage = (item.MinimumStock ?? 0) - item.Quantity;
                    var status = item.Quantity == 0 ? "🔴 OUT OF STOCK" : "🟡 LOW STOCK";

                    report += $"{status}\n";
                    report += $"Item: {item.ItemName}\n";
                    report += $"Current Stock: {item.Quantity} {item.Unit}\n";
                    report += $"Minimum Required: {item.MinimumStock} {item.Unit}\n";
                    report += $"Shortage: {shortage} {item.Unit}\n";
                    report += $"Supplier: {item.Supplier}\n";
                    if (!string.IsNullOrEmpty(item.Notes))
                        report += $"Notes: {item.Notes}\n";
                    report += "------------------------\n\n";
                }

                report += "📋 RECOMMENDED ACTIONS:\n";
                report += "1. Contact suppliers immediately for out-of-stock items\n";
                report += "2. Place orders for low stock items before they run out\n";
                report += "3. Review minimum stock levels for frequently depleted items\n";
                report += "4. Consider increasing safety stock for critical ingredients\n\n";
            }

            report += "Report generated by Sakana House POS System\n";
            return report;
        }
    }
}
