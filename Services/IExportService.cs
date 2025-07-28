using BusinessObjects;
using System.Collections.Generic;

namespace Services
{
    public interface IExportService
    {
        bool ExportStockToExcel(List<Stock> stocks, string filePath);
        bool ExportStockToPdf(List<Stock> stocks, string filePath);
        bool ExportLowStockReportToExcel(List<Stock> lowStockItems, string filePath);
        bool ExportLowStockReportToPdf(List<Stock> lowStockItems, string filePath);
        string GenerateLowStockReportText(List<Stock> lowStockItems);
    }
}
