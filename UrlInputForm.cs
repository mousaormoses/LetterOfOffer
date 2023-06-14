using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LetterOfOffer
{
    public partial class UrlInputForm : Form
    {
        public string Url { get; set; }

        public UrlInputForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Url = urlTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
