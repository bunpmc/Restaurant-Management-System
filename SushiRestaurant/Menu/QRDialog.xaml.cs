using Services; // để gọi VNPayHelper
using System;
using System.Windows;

namespace SushiRestaurant.Menu
{
    public partial class QRDialog : Window
    {
        public QRDialog(decimal payable)
        {
            InitializeComponent();
            GenerateQRCode(payable);
        }

        private void GenerateQRCode(decimal payable)
        {
            QrImage.Source = VNPay.CreateVNPayQRCode(
                tmnCode: "4Q0AGO8S", // Thay mã thật của bạn
                hashSecret: "AN0RNCIIMYZSJTHU47SJQJWL8IULFP80", // Thay key thật của bạn
                amount: payable,
                orderId: DateTime.Now.Ticks.ToString(),
                returnUrl: "https://fe-gender-healthcare-service-manage.vercel.app/payment-result"
            );
        }
    }
}
