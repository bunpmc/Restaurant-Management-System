using System;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SushiRestaurant.StockList
{
    public partial class ReportViewerWindow : Window
    {
        private string _reportContent;

        public ReportViewerWindow(string reportContent, string title)
        {
            InitializeComponent();
            _reportContent = reportContent;
            txtReportTitle.Text = title;
            txtReportContent.Text = reportContent;
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(_reportContent);
                MessageBox.Show("Report content copied to clipboard!", "Copy Successful", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy to clipboard: {ex.Message}", "Copy Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // Create a FlowDocument for printing
                    var flowDocument = new FlowDocument();
                    
                    // Add title
                    var titleParagraph = new Paragraph(new Run(txtReportTitle.Text))
                    {
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(0, 0, 0, 20)
                    };
                    flowDocument.Blocks.Add(titleParagraph);
                    
                    // Add content
                    var contentParagraph = new Paragraph(new Run(_reportContent))
                    {
                        FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                        FontSize = 10,
                        LineHeight = 14
                    };
                    flowDocument.Blocks.Add(contentParagraph);
                    
                    // Set page size
                    flowDocument.PageWidth = printDialog.PrintableAreaWidth;
                    flowDocument.PageHeight = printDialog.PrintableAreaHeight;
                    flowDocument.PagePadding = new Thickness(50);
                    flowDocument.ColumnGap = 0;
                    flowDocument.ColumnWidth = printDialog.PrintableAreaWidth - 100;
                    
                    // Print
                    IDocumentPaginatorSource idpSource = flowDocument;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, txtReportTitle.Text);
                    
                    MessageBox.Show("Report sent to printer successfully!", "Print Successful", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to print report: {ex.Message}", "Print Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
