using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.IO;

namespace encrypt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool check_param()
        {
            if (tbInputFile.Text.Length <= 0)
            {
                MessageBox.Show("InputFile is empty!");
                return false;
            }

            if (!File.Exists(tbInputFile.Text))
            {
                MessageBox.Show("Can not find InputFile!");
                return false;
            }

            if (tbOutputFile.Text.Length <= 0)
            {
                MessageBox.Show("OutputFile is empty!");
                return false;
            }

            if (tbKey.Text.Length <= 0)
            {
                MessageBox.Show("Key is empty!");
                return false;
            }

            return true;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (!check_param())
            {
                return;
            }

            byte[] input_bytes = null;
            using (var fs = new FileStream(tbInputFile.Text, FileMode.Open))
            {
                input_bytes = new byte[fs.Length];
                fs.Read(input_bytes, 0, (int)fs.Length);
            }

            var ms = new MemoryStream();
            var key = Encoding.UTF8.GetBytes(tbKey.Text);
            byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            var provider = new DESCryptoServiceProvider();
            var cs = new CryptoStream(ms, provider.CreateEncryptor(key, iv), CryptoStreamMode.Write);

            try
            {
                cs.Write(input_bytes, 0, input_bytes.Length);
                cs.FlushFinalBlock();

                using (var fs = new FileStream(tbOutputFile.Text, FileMode.OpenOrCreate))
                {
                    fs.Write(ms.ToArray(), 0, (int)ms.Length);
                }
            }
            finally
            {
                cs.Close();
                ms.Close();
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (!check_param())
            {
                return;
            }

            byte[] input_bytes = null;
            using (var fs = new FileStream(tbInputFile.Text, FileMode.Open))
            {
                input_bytes = new byte[fs.Length];
                fs.Read(input_bytes, 0, (int)fs.Length);
            }

            var ms = new MemoryStream();
            var key = Encoding.UTF8.GetBytes(tbKey.Text);
            byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            var provider = new DESCryptoServiceProvider();
            var cs = new CryptoStream(ms, provider.CreateDecryptor(key, iv), CryptoStreamMode.Write);

            try
            {
                cs.Write(input_bytes, 0, input_bytes.Length);
                cs.FlushFinalBlock();

                using (var fs = new FileStream(tbOutputFile.Text, FileMode.OpenOrCreate))
                {
                    fs.Write(ms.ToArray(), 0, (int)ms.Length);
                }
            }
            finally
            {
                cs.Close();
                ms.Close();
            }
        }

        private void btnChooseInputFile_Click(object sender, EventArgs e)
        {
            using (var fd = new OpenFileDialog())
            {
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    tbInputFile.Text = fd.FileName;
                }
            }
        }

        private void btnChooseOutputFile_Click(object sender, EventArgs e)
        {
            using (var fd = new OpenFileDialog())
            {
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    tbOutputFile.Text = fd.FileName;
                }
            }
        }
    }
}
