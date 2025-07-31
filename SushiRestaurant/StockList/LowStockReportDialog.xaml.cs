using System.Windows;

namespace SushiRestaurant.StockList
{
    public partial class LowStockReportDialog : Window
    {
        public string SelectedAction { get; private set; } = "View";

        public LowStockReportDialog()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (rbView.IsChecked == true)
                SelectedAction = "View";
            else if (rbExportExcel.IsChecked == true)
                SelectedAction = "Export Excel";
            else if (rbExportPdf.IsChecked == true)
                SelectedAction = "Export PDF";
            
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
