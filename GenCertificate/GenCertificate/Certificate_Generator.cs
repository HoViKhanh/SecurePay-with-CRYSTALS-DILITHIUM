using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenCertificate
{
    public partial class Certificate_Generator : Form
    {
        public Certificate_Generator()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
            FolderBrowser.Description = "Select the folder to store the certificates";
            DialogResult result = FolderBrowser.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                tbFolderName.Text = FolderBrowser.SelectedPath;
                btnGenerate.Enabled = tbFolderName.Text.Length > 0;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                AsymmetricCipherKeyPair CertificateKey;

                //let us first generate the root certificate
                X509Certificate2 X509RootCert = Cryptography.CreateCertificate(tbSubject.Text, tbIssuer.Text, (int)numericUpDownMonths.Value, out CertificateKey);

                //now let us write the certificates files to the folder 
                File.WriteAllBytes(tbFolderName.Text + "\\" + "X509Cert.der", X509RootCert.RawData);

                string PublicPEMFile = tbFolderName.Text + "\\" + "X509Cert-public.pem";
                string PrivatePEMFile = tbFolderName.Text + "\\" + "X509Cert-private.pem";

                //now let us also create the PEM file as well in case we need it
                using (TextWriter textWriter = new StreamWriter(PublicPEMFile, false))
                {
                    PemWriter pemWriter = new PemWriter(textWriter);
                    pemWriter.WriteObject(CertificateKey.Public);
                    pemWriter.Writer.Flush();
                }

                //now let us also create the PEM file as well in case we need it
                using (TextWriter textWriter = new StreamWriter(PrivatePEMFile, false))
                {
                    PemWriter pemWriter = new PemWriter(textWriter);
                    pemWriter.WriteObject(CertificateKey.Private);
                    pemWriter.Writer.Flush();
                }

                MessageBox.Show("The security certificates have been succcessfully generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating certificates. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
 }

