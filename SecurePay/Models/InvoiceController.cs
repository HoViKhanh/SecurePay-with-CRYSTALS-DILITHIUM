using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Pqc.Crypto.Lms;
//using Org.BouncyCastle.Pqc.Crypto.Dilithium;
using Newtonsoft.Json;
using System.Text;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Dilithium;

namespace SecurePay.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult CreateInvoice()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateInvoice(string invoiceDetails)
        {
            // Tạo khóa công khai và riêng tư Dilithium
            var generator = new DilithiumKeyPairGenerator();
            generator.Init(new DilithiumKeyGenerationParameters(new SecureRandom(), DilithiumParameters.Dilithium5Aes));
            AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();

            var publicKey = (DilithiumPublicKeyParameters)keyPair.Public;
            var publicKeyEncoded = publicKey.GetEncoded();

            var jwk = new
            {
                kty = "OKP",
                alg = "Dilithium",
                crv = "Dilithium5Aes",
                x = Base64UrlEncode(publicKeyEncoded)
            };

            var invoice = new
            {
                Details = invoiceDetails,
                PublicKey = jwk
            };

            var json = JsonConvert.SerializeObject(invoice, Newtonsoft.Json.Formatting.Indented);

            var fileName = "invoice.json";
            var mimeType = "application/json";
            return File(Encoding.UTF8.GetBytes(json), mimeType, fileName);
        }

        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Loại bỏ padding
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }
    }
}
