using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt_AES
{
    public partial class DocumentEncryption : Form
    {
        public DocumentEncryption()
        {
            InitializeComponent();
        }

        private void DocumentEncryption_Load(object sender, EventArgs e)
        {

        }

        private void chooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }
    }
}
