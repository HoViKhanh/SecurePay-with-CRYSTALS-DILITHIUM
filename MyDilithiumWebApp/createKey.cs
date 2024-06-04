using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Dilithium;

class createKey
{
    static void Main()
    {
        // Tạo cặp khóa
        KeyGenerationParameters keyGenParams = new KeyGenerationParameters(new SecureRandom(), 256);
        DilithiumKeyPairGenerator keyPairGenerator = new DilithiumKeyPairGenerator();
        keyPairGenerator.Init(keyGenParams);s
        AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();

        DilithiumPrivateKeyParameters privateKey = (DilithiumPrivateKeyParameters)keyPair.Private;
        DilithiumPublicKeyParameters publicKey = (DilithiumPublicKeyParameters)keyPair.Public;

        Console.WriteLine("Khóa đã tạo: OK");
    }
}