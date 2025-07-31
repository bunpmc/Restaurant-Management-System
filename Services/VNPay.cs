
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Services
{
    public class VNPay
    {
        public static BitmapImage CreateVNPayQRCode(
             string tmnCode,
             string hashSecret,
             decimal amount,
             string orderId,
             string returnUrl)
        {
            // 1. Tạo URL thanh toán VNPay
            string paymentUrl = CreatePaymentUrl(tmnCode, hashSecret, amount, orderId, returnUrl);

            // 2. Tạo QR code từ URL
            return GenerateQRCode(paymentUrl);
        }

        private static string CreatePaymentUrl(
            string tmnCode,
            string hashSecret,
            decimal amount,
            string orderId,
            string returnUrl)
        {
            var vnp_Amount = ((long)(amount * 100)).ToString(); // VNPay yêu cầu nhân 100
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");

            var vnpParams = new Dictionary<string, string>
            {
                {"vnp_Version", "2.1.0"},
                {"vnp_Command", "pay"},
                {"vnp_TmnCode", tmnCode},
                {"vnp_Amount", vnp_Amount},
                {"vnp_CreateDate", vnp_CreateDate},
                {"vnp_CurrCode", "VND"},
                {"vnp_IpAddr", "127.0.0.1"},
                {"vnp_Locale", "vn"},
                {"vnp_OrderInfo", $"Thanh toan don hang #{orderId}"},
                {"vnp_OrderType", "other"},
                {"vnp_ReturnUrl", returnUrl},
                {"vnp_TxnRef", orderId}
            };

            var signData = string.Join("&", vnpParams
                .OrderBy(p => p.Key)
                .Select(p => p.Key + "=" + p.Value));

            var hash = HmacSHA256(hashSecret, signData);
            var query = signData + "&vnp_SecureHash=" + hash;

            return "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?" + query;
        }

        private static string HmacSHA256(string key, string inputData)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using var hmac = new HMACSHA256(keyBytes);
            byte[] hashBytes = hmac.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
        }

        private static BitmapImage GenerateQRCode(string paymentUrl)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(paymentUrl, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            var qrBytes = qrCode.GetGraphic(20);

            using var ms = new MemoryStream(qrBytes);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();
            image.Freeze(); // để thread khác có thể dùng
            return image;
        }
    }
}