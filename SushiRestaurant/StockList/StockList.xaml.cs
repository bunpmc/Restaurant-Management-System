using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;
using System.Globalization;
using Microsoft.Win32;
using System.IO;

namespace SushiRestaurant.StockList
{
    /// <summary>
    /// Interaction logic for StockList.xaml
    /// </summary>
    public partial class StockList : Window
    {
        private readonly IStockService _stockService;
        private readonly IExportService _exportService;
        private List<Stock> _allStocks;
        private List<Stock> _filteredStocks;
        private Stock _selectedStock;

        public StockList()
        {
            InitializeComponent();
            _stockService = new StockService();
            _exportService = new ExportService();

            InitializeFilters();
            LoadStocks();
        }

        private void InitializeFilters()
        {
            try
            {
                // Load categories for filters
                LoadCategoryFilters();

                // Load suppliers for filters
                LoadSupplierFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing filters: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCategoryFilters()
        {
            try
            {
                var categories = _stockService.GetAllCategories();

                // Clear existing items
                cmbCategoryFilter.Items.Clear();
                cmbCategory.Items.Clear();

                // Add "All Categories" option for filter
                cmbCategoryFilter.Items.Add(new ComboBoxItem { Content = "All Categories", Tag = null });

                // Add categories to both filter and form comboboxes
                foreach (var category in categories)
                {
                    cmbCategoryFilter.Items.Add(new ComboBoxItem { Content = category, Tag = category });
                    cmbCategory.Items.Add(category);
                }

                cmbCategoryFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSupplierFilters()
        {
            try
            {
                var suppliers = _stockService.GetAllSuppliers();

                // Clear existing items
                cmbSupplierFilter.Items.Clear();
                cmbSupplier.Items.Clear();

                // Add "All Suppliers" option for filter
                cmbSupplierFilter.Items.Add(new ComboBoxItem { Content = "All Suppliers", Tag = null });

                // Add suppliers to both filter and form comboboxes
                foreach (var supplier in suppliers)
                {
                    cmbSupplierFilter.Items.Add(new ComboBoxItem { Content = supplier, Tag = supplier });
                    cmbSupplier.Items.Add(supplier);
                }

                cmbSupplierFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadStocks()
        {
            try
            {
                _allStocks = _stockService.GetAllStocks();
                _filteredStocks = new List<Stock>(_allStocks);

                ApplyFilters();
                UpdateStockDisplay();
                UpdateAlerts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stocks: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilters()
        {
            _filteredStocks = new List<Stock>(_allStocks);

            // Apply category filter
            if (cmbCategoryFilter.SelectedItem is ComboBoxItem categoryItem && categoryItem.Tag != null)
            {
                var selectedCategory = categoryItem.Tag.ToString();
                _filteredStocks = _filteredStocks.Where(s => s.Category == selectedCategory).ToList();
            }

            // Apply supplier filter
            if (cmbSupplierFilter.SelectedItem is ComboBoxItem supplierItem && supplierItem.Tag != null)
            {
                var selectedSupplier = supplierItem.Tag.ToString();
                _filteredStocks = _filteredStocks.Where(s => s.Supplier == selectedSupplier).ToList();
            }

            // Apply status filter
            if (cmbStatusFilter.SelectedItem is ComboBoxItem statusItem)
            {
                var selectedStatus = statusItem.Content.ToString();
                switch (selectedStatus)
                {
                    case "Low Stock":
                        _filteredStocks = _stockService.GetLowStockItems();
                        break;
                    case "Expiring Soon":
                        _filteredStocks = _stockService.GetExpiringItems(7);
                        break;
                    case "Out of Stock":
                        _filteredStocks = _filteredStocks.Where(s => s.Quantity == 0).ToList();
                        break;
                        // "All Items" - no additional filtering needed
                }
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchTerm = txtSearch.Text.ToLower();
                _filteredStocks = _filteredStocks.Where(s =>
                    s.ItemName.ToLower().Contains(searchTerm) ||
                    s.Category.ToLower().Contains(searchTerm) ||
                    s.Supplier.ToLower().Contains(searchTerm)
                ).ToList();
            }
        }

        private void UpdateStockDisplay()
        {
            dgStocks.ItemsSource = _filteredStocks;
            txtStockCount.Text = $"Total Items: {_filteredStocks.Count}";
        }

        private void UpdateAlerts()
        {
            try
            {
                var lowStockCount = _stockService.GetLowStockItems().Count;
                var expiringCount = _stockService.GetExpiringItems(7).Count;

                txtLowStockAlert.Text = $"Low Stock: {lowStockCount}";
                txtExpiringAlert.Text = $"Expiring Soon: {expiringCount}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating alerts: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            txtItemName.Text = "";
            cmbCategory.Text = "";
            txtQuantity.Text = "";
            txtUnit.Text = "";
            txtUnitPrice.Text = "";
            cmbSupplier.Text = "";
            txtMinimumStock.Text = "";
            dpExpiryDate.SelectedDate = null;
            txtNotes.Text = "";

            _selectedStock = null;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnAdd.IsEnabled = true;
        }

        private void LoadStockToForm(Stock stock)
        {
            if (stock == null) return;

            txtItemName.Text = stock.ItemName;
            cmbCategory.Text = stock.Category;
            txtQuantity.Text = stock.Quantity.ToString();
            txtUnit.Text = stock.Unit;
            txtUnitPrice.Text = stock.UnitPrice?.ToString("F2") ?? "";
            cmbSupplier.Text = stock.Supplier;
            txtMinimumStock.Text = stock.MinimumStock?.ToString() ?? "";
            dpExpiryDate.SelectedDate = stock.ExpiryDate;
            txtNotes.Text = stock.Notes;

            _selectedStock = stock;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnAdd.IsEnabled = false;
        }

        private Stock CreateStockFromForm()
        {
            var stock = new Stock
            {
                ItemName = txtItemName.Text.Trim(),
                Category = cmbCategory.Text.Trim(),
                Unit = txtUnit.Text.Trim(),
                Supplier = cmbSupplier.Text.Trim(),
                Notes = txtNotes.Text.Trim(),
                ExpiryDate = dpExpiryDate.SelectedDate,
                Status = "Active"
            };

            // Parse numeric values
            if (int.TryParse(txtQuantity.Text, out int quantity))
                stock.Quantity = quantity;

            if (decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice))
                stock.UnitPrice = unitPrice;

            if (int.TryParse(txtMinimumStock.Text, out int minStock))
                stock.MinimumStock = minStock;

            return stock;
        }

        // Event Handlers
        private void dgStocks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStocks.SelectedItem is Stock selectedStock)
            {
                LoadStockToForm(selectedStock);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
            UpdateStockDisplay();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
            UpdateStockDisplay();
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            ApplyFilters();
            UpdateStockDisplay();
        }

        private void cmbCategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ApplyFilters();
                UpdateStockDisplay();
            }
        }

        private void cmbSupplierFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ApplyFilters();
                UpdateStockDisplay();
            }
        }

        private void cmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ApplyFilters();
                UpdateStockDisplay();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadStocks();
            LoadCategoryFilters();
            LoadSupplierFilters();
        }

        // CRUD Operations
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var stock = CreateStockFromForm();

                if (_stockService.SaveStock(stock))
                {
                    MessageBox.Show("Stock item added successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadStocks();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to add stock item.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding stock item: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedStock == null)
                {
                    MessageBox.Show("Please select a stock item to update.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var stock = CreateStockFromForm();
                stock.Id = _selectedStock.Id;

                if (_stockService.UpdateStock(stock))
                {
                    MessageBox.Show("Stock item updated successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadStocks();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to update stock item.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating stock item: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedStock == null)
                {
                    MessageBox.Show("Please select a stock item to delete.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to delete '{_selectedStock.ItemName}'?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (_stockService.DeleteStock(_selectedStock.Id))
                    {
                        MessageBox.Show("Stock item deleted successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadStocks();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete stock item.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting stock item: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void btnExportStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Show export options dialog
                var exportDialog = new ExportOptionsDialog();
                if (exportDialog.ShowDialog() == true)
                {
                    var exportFormat = exportDialog.SelectedFormat;
                    var includeAllData = exportDialog.IncludeAllData;

                    // Get data to export
                    var dataToExport = includeAllData ? _allStocks : _filteredStocks;

                    // Show save file dialog
                    var saveDialog = new SaveFileDialog();
                    if (exportFormat == "Excel")
                    {
                        saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                        saveDialog.DefaultExt = "xlsx";
                    }
                    else
                    {
                        saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                        saveDialog.DefaultExt = "pdf";
                    }

                    saveDialog.FileName = $"Stock_Inventory_{DateTime.Now:yyyyMMdd_HHmmss}";

                    if (saveDialog.ShowDialog() == true)
                    {
                        bool success = false;

                        if (exportFormat == "Excel")
                        {
                            success = _exportService.ExportStockToExcel(dataToExport, saveDialog.FileName);
                        }
                        else
                        {
                            success = _exportService.ExportStockToPdf(dataToExport, saveDialog.FileName);
                        }

                        if (success)
                        {
                            var result = MessageBox.Show($"Stock data exported successfully to:\n{saveDialog.FileName}\n\nWould you like to open the file?",
                                "Export Successful", MessageBoxButton.YesNo, MessageBoxImage.Information);

                            if (result == MessageBoxResult.Yes)
                            {
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = saveDialog.FileName,
                                    UseShellExecute = true
                                });
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to export stock data. Please try again.", "Export Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting stock data: {ex.Message}", "Export Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLowStockReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var lowStockItems = _stockService.GetLowStockItems();

                if (lowStockItems.Count == 0)
                {
                    MessageBox.Show("✅ No low stock items found!\nAll inventory levels are above minimum thresholds.",
                        "Low Stock Report", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Show low stock report options dialog
                var reportDialog = new LowStockReportDialog();
                if (reportDialog.ShowDialog() == true)
                {
                    var action = reportDialog.SelectedAction;

                    switch (action)
                    {
                        case "View":
                            // Show detailed text report
                            var reportText = _exportService.GenerateLowStockReportText(lowStockItems);
                            var reportWindow = new ReportViewerWindow(reportText, "Low Stock Report");
                            reportWindow.ShowDialog();
                            break;

                        case "Export Excel":
                            ExportLowStockReport(lowStockItems, "Excel");
                            break;

                        case "Export PDF":
                            ExportLowStockReport(lowStockItems, "PDF");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating low stock report: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportLowStockReport(List<Stock> lowStockItems, string format)
        {
            try
            {
                var saveDialog = new SaveFileDialog();
                if (format == "Excel")
                {
                    saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    saveDialog.DefaultExt = "xlsx";
                }
                else
                {
                    saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                    saveDialog.DefaultExt = "pdf";
                }

                saveDialog.FileName = $"Low_Stock_Report_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveDialog.ShowDialog() == true)
                {
                    bool success = false;

                    if (format == "Excel")
                    {
                        success = _exportService.ExportLowStockReportToExcel(lowStockItems, saveDialog.FileName);
                    }
                    else
                    {
                        success = _exportService.ExportLowStockReportToPdf(lowStockItems, saveDialog.FileName);
                    }

                    if (success)
                    {
                        var result = MessageBox.Show($"Low stock report exported successfully to:\n{saveDialog.FileName}\n\nWould you like to open the file?",
                            "Export Successful", MessageBoxButton.YesNo, MessageBoxImage.Information);

                        if (result == MessageBoxResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = saveDialog.FileName,
                                UseShellExecute = true
                            });
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to export low stock report. Please try again.", "Export Failed",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting low stock report: {ex.Message}", "Export Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
