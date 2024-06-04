using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Dilithium;
using Org.BouncyCastle.Security;

class PaymentRequestHandler
{
    private readonly DilithiumPrivateKeyParameters privateKey;
    private readonly DilithiumPublicKeyParameters publicKey;

    public PaymentRequestHandler()
    {
        // Tạo cặp khóa
        KeyGenerationParameters keyGenParams = new KeyGenerationParameters(new SecureRandom(), 256);
        DilithiumKeyPairGenerator keyPairGenerator = new DilithiumKeyPairGenerator();
        keyPairGenerator.Init(keyGenParams);
        AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();
        privateKey = (DilithiumPrivateKeyParameters)keyPair.Private;
        publicKey = (DilithiumPublicKeyParameters)keyPair.Public;
    }

    public byte[] SignInvoice(string invoiceData)
    {
        DilithiumSigner signer = new DilithiumSigner();
        signer.Init(true, privateKey);

        byte[] message = System.Text.Encoding.UTF8.GetBytes(invoiceData);
        byte[] signature = signer.GenerateSignature(message);

        return signature;
    }

    public bool VerifyInvoice(string invoiceData, byte[] signature)
    {
        DilithiumSigner verifier = new DilithiumSigner();
        verifier.Init(false, publicKey);

        byte[] message = System.Text.Encoding.UTF8.GetBytes(invoiceData);
        return verifier.VerifySignature(message, signature);
    }
}

class Program
{
    static void Main()
    {
        // Tạo hệ thống xử lý thanh toán
        PaymentRequestHandler handler = new PaymentRequestHandler();

        string invoiceData = "Thông tin hóa đơn";
        byte[] signature = handler.SignInvoice(invoiceData);

        bool isVerified = handler.VerifyInvoice(invoiceData, signature);
        Console.WriteLine("Chữ ký xác thực: " + isVerified);
    }
}