using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Dilithium;
using Org.BouncyCastle.Security;

public class PaymentGateway
{
    public bool ProcessPayment(string invoiceData, string signatureBase64, DilithiumPublicKeyParameters publicKey)
    {
        DilithiumSigner verifier = new DilithiumSigner();
        verifier.Init(false, publicKey);

        byte[] message = System.Text.Encoding.UTF8.GetBytes(invoiceData);
        byte[] signature = Convert.FromBase64String(signatureBase64);

        if (verifier.VerifySignature(message, signature))
        {
            // Chữ ký hợp lệ => Xử lý thanh toán
            Console.WriteLine("Thanh toán thành công");
            return true;
        }
        else
        {
            // Chữ ký không hợp lệ
            Console.WriteLine("Xác thực thất bại");
            return false;
        }
    }
}