using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Dilithium;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private AsymmetricCipherKeyPair keyPair;
        private byte[] signature;
        private byte[] fileContent;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerateKeys_Click(object sender, EventArgs e)
        {
            keyPair = GenerateKeys();
            byte[] publicKeyBytes = ((DilithiumPublicKeyParameters)keyPair.Public).GetEncoded();
            byte[] privateKeyBytes = ((DilithiumPrivateKeyParameters)keyPair.Private).GetEncoded();

            richTextBoxOutput.Clear();
            richTextBoxOutput.AppendText("PublicKey:\n");
            richTextBoxOutput.AppendText(BitConverter.ToString(publicKeyBytes).Replace("-", " ") + "\n\n");
            richTextBoxOutput.AppendText("PrivateKey:\n");
            richTextBoxOutput.AppendText(BitConverter.ToString(privateKeyBytes).Replace("-", " ") + "\n");
        }
        private void btnSignMessage_Click(object sender, EventArgs e)
        {
            if (keyPair == null)
            {
                MessageBox.Show("Please generate keys first.");
                return;
            }

            if (fileContent == null)
            {
                MessageBox.Show("Please select a file first.");
                return;
            }

            signature = Sign((DilithiumPrivateKeyParameters)keyPair.Private, fileContent);

            richTextBoxOutputsig.Clear();
            richTextBoxOutputsig.AppendText("Signature:\n");
            richTextBoxOutputsig.AppendText(BitConverter.ToString(signature).Replace("-", " ") + "\n");
        }

        private void btnVerifySignature_Click(object sender, EventArgs e)
        {
            if (keyPair == null || signature == null)
            {
                richTextBoxverify.AppendText("Please generate keys and sign a file first.\n");
                return;
            }

            if (fileContent == null)
            {
                richTextBoxverify.AppendText("Please select a file first.\n");
                return;
            }

            bool isValid = Verify((DilithiumPublicKeyParameters)keyPair.Public, fileContent, signature);
            richTextBoxverify.AppendText(isValid ? "File is unchanged\n" : "Warning! File has been altered\n");

        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
                fileContent = null;
                fileContent = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                verify_txtfilePath.Text = openFileDialog.FileName;
                fileContent = null;
                fileContent = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private AsymmetricCipherKeyPair GenerateKeys()
        {
            var generator = new DilithiumKeyPairGenerator();
            generator.Init(new DilithiumKeyGenerationParameters(
                new SecureRandom(),
                DilithiumParameters.Dilithium5Aes
            ));

            return generator.GenerateKeyPair();
        }

        private byte[] Sign(DilithiumPrivateKeyParameters privateKey, byte[] message)
        {
            var signer = new DilithiumSigner();
            signer.Init(true, privateKey);

            return signer.GenerateSignature(message);
        }

        private bool Verify(DilithiumPublicKeyParameters publicKey, byte[] message, byte[] signature)
        {
            var verifier = new DilithiumSigner();
            verifier.Init(false, publicKey);

            return verifier.VerifySignature(message, signature);
        }
    }
}
