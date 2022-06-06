using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Lab4_mmh_task1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            cbFomat.Items.Add("Text");
            cbFomat.Items.Add("Hex");
            cbFomat.Items.Add("File");

        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            string data = rtbData.Text;
            byte[] input = null;

            if(cbFomat.Text == "")
            {
                MessageBox.Show("Please choose format","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if(cbFomat.Text == "Hex")
            {
                input = StringToByteArray(data);
            }
            else if (cbFomat.Text == "File")
            {
                if(tbFile.Text != "")
                    input = File.ReadAllBytes(tbFile.Text);
                else
                {
                    MessageBox.Show("Please choose file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            tbMD5.Text = MD5(data,input);
            tbSHA1.Text = SHA1(data, input);
            tbSHA256.Text = SHA256(data, input);
            tbSHA512.Text = SHA3_512(data, input);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public string byteToString (byte[] result)
        {
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public string MD5(string plainText="", byte[] hex = null)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            if (hex == null)
            {
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(plainText));
            }
            else
            {
                md5.ComputeHash(hex);
            }
                     
            byte[] result = md5.Hash;
            return byteToString(result);
        }

        public string SHA1(string plainText = "", byte[] hex = null)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            if (hex == null)
            {
                sha1.ComputeHash(ASCIIEncoding.ASCII.GetBytes(plainText));
            }
            else
            {
                sha1.ComputeHash(hex);
            }

            byte[] result = sha1.Hash;
            return byteToString(result);
        }

        public string SHA256(string plainText = "", byte[] hex = null)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();

            if (hex == null)
            {
                sha256.ComputeHash(ASCIIEncoding.ASCII.GetBytes(plainText));
            }
            else
            {
                sha256.ComputeHash(hex);
            }

            byte[] result = sha256.Hash;
            return byteToString(result);
        }

        public string SHA3_512(string plainText = "", byte[] hex = null)
        {
            var hashAlgorithm = new Org.BouncyCastle.Crypto.Digests.Sha3Digest(512);

            // Choose correct encoding based on your usecase
            byte[] input;
            if (hex == null)
            {
                input = Encoding.ASCII.GetBytes(plainText);
            }
            else
            {
                input = hex;
            }

            hashAlgorithm.BlockUpdate(input, 0, input.Length);

            byte[] result = new byte[64]; 
            hashAlgorithm.DoFinal(result, 0);

            string hashString = BitConverter.ToString(result);
            return  hashString.Replace("-", "").ToLowerInvariant();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            tbFile.Text = ofd.FileName.ToString();
        }
    }
}
