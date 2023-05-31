using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LetterOfOffer_18
{
    public partial class SelectColumnsForm : Form
    {
        // Store the list of columns
        private List<string> columnNames;
        public List<string> SelectedColumns { get; set; }

        public SelectColumnsForm(List<string> columnNames)
        {
            InitializeComponent();

            this.columnNames = columnNames;

            // Dynamically create CheckBoxes for each column
            for (int i = 0; i < this.columnNames.Count; i++)
            {
                var checkBox = new CheckBox();
                checkBox.Text = this.columnNames[i];
                checkBox.Checked = true; // Default to checked
                checkBox.Location = new Point(10, 10 + i * 25); // Vary vertical position
                this.Controls.Add(checkBox);
            }

            this.FormClosing += SelectColumnsForm_FormClosing;
        }

        private void SelectColumnsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SelectedColumns = GetSelectedColumns();
        }

        public List<string> GetSelectedColumns()
        {
            List<string> selectedColumns = new List<string>();

            // Check each CheckBox control to see if it's checked
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox checkBox = control as CheckBox;
                    if (checkBox.Checked)
                    {
                        selectedColumns.Add(checkBox.Text);
                    }
                }
            }

            return selectedColumns;
        }

        private void SelectColumnsForm_Load(object sender, EventArgs e)
        {

        }
    }


}
