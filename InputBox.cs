﻿using System;
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
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
            okButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
        }

        public string InputText
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
    }

}
