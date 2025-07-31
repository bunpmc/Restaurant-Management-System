using System.Windows;

namespace SushiRestaurant.StockList
{
    public partial class ExportOptionsDialog : Window
    {
        public string SelectedFormat { get; private set; } = "Excel";
        public bool IncludeAllData { get; private set; } = false;

        public ExportOptionsDialog()
        {
            InitializeComponent();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            SelectedFormat = rbExcel.IsChecked == true ? "Excel" : "PDF";
            IncludeAllData = rbAllData.IsChecked == true;
            
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
