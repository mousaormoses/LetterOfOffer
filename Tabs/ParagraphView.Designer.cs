namespace LetterOfOffer.Tabs
{
    partial class ParagraphView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelParagraph = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave_dataParagraphs = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelToolBar = new System.Windows.Forms.Panel();
            this.toolStripEditor = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripDropDownFontFamily = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripBold = new System.Windows.Forms.ToolStripButton();
            this.toolStripItalic = new System.Windows.Forms.ToolStripButton();
            this.toolStripUnderline = new System.Windows.Forms.ToolStripButton();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelToolBar.SuspendLayout();
            this.toolStripEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelParagraph
            // 
            this.panelParagraph.AutoScroll = true;
            this.panelParagraph.Location = new System.Drawing.Point(3, 123);
            this.panelParagraph.Name = "panelParagraph";
            this.panelParagraph.Size = new System.Drawing.Size(572, 372);
            this.panelParagraph.TabIndex = 10;
            this.panelParagraph.Paint += new System.Windows.Forms.PaintEventHandler(this.panelParagraph_Paint);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(103)))), ((int)(((byte)(79)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(718, 44);
            this.label2.TabIndex = 12;
            this.label2.Text = "Paragraphs";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave_dataParagraphs
            // 
            this.btnSave_dataParagraphs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(103)))), ((int)(((byte)(79)))));
            this.btnSave_dataParagraphs.FlatAppearance.BorderSize = 0;
            this.btnSave_dataParagraphs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave_dataParagraphs.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnSave_dataParagraphs.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSave_dataParagraphs.Location = new System.Drawing.Point(6, 10);
            this.btnSave_dataParagraphs.Name = "btnSave_dataParagraphs";
            this.btnSave_dataParagraphs.Size = new System.Drawing.Size(148, 35);
            this.btnSave_dataParagraphs.TabIndex = 13;
            this.btnSave_dataParagraphs.Text = "Save";
            this.btnSave_dataParagraphs.UseVisualStyleBackColor = false;
            this.btnSave_dataParagraphs.Click += new System.EventHandler(this.btnSave_dataParagraphs_Click_1);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonAdd);
            this.panel2.Controls.Add(this.btnSave_dataParagraphs);
            this.panel2.Location = new System.Drawing.Point(0, 501);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(575, 49);
            this.panel2.TabIndex = 37;
            // 
            // buttonAdd
            // 
            this.buttonAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(103)))), ((int)(((byte)(79)))));
            this.buttonAdd.FlatAppearance.BorderSize = 0;
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdd.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.buttonAdd.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonAdd.Location = new System.Drawing.Point(160, 10);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(148, 35);
            this.buttonAdd.TabIndex = 11;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click_1);
            // 
            // listView1
            // 
            this.listView1.AllowColumnReorder = true;
            this.listView1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 34);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(137, 414);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Location = new System.Drawing.Point(581, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(137, 503);
            this.panel1.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "Keys";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "Paragraphs";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelToolBar
            // 
            this.panelToolBar.Controls.Add(this.toolStripEditor);
            this.panelToolBar.Location = new System.Drawing.Point(3, 84);
            this.panelToolBar.Name = "panelToolBar";
            this.panelToolBar.Size = new System.Drawing.Size(500, 33);
            this.panelToolBar.TabIndex = 39;
            // 
            // toolStripEditor
            // 
            this.toolStripEditor.AutoSize = false;
            this.toolStripEditor.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.toolStripEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBold,
            this.toolStripItalic,
            this.toolStripUnderline,
            this.toolStripDropDownFontSize,
            this.toolStripDropDownFontFamily});
            this.toolStripEditor.Location = new System.Drawing.Point(0, 0);
            this.toolStripEditor.Name = "toolStripEditor";
            this.toolStripEditor.Size = new System.Drawing.Size(500, 33);
            this.toolStripEditor.TabIndex = 0;
            this.toolStripEditor.Text = "toolStrip1";
            // 
            // toolStripDropDownFontSize
            // 
            this.toolStripDropDownFontSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "26",
            "28",
            "32",
            "36",
            "40",
            "46",
            "52",
            "60",
            "72"});
            this.toolStripDropDownFontSize.Name = "toolStripDropDownFontSize";
            this.toolStripDropDownFontSize.Size = new System.Drawing.Size(75, 33);
            this.toolStripDropDownFontSize.Text = "Font Size";
            // 
            // toolStripDropDownFontFamily
            // 
            this.toolStripDropDownFontFamily.Name = "toolStripDropDownFontFamily";
            this.toolStripDropDownFontFamily.Size = new System.Drawing.Size(120, 33);
            this.toolStripDropDownFontFamily.Text = "Font Family";
            // 
            // toolStripBold
            // 
            this.toolStripBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBold.Image = global::LetterOfOffer.Properties.Resources.icons8_bold_10;
            this.toolStripBold.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripBold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBold.Name = "toolStripBold";
            this.toolStripBold.Size = new System.Drawing.Size(23, 30);
            this.toolStripBold.Text = "Bold";
            // 
            // toolStripItalic
            // 
            this.toolStripItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripItalic.Image = global::LetterOfOffer.Properties.Resources.icons8_italic_10;
            this.toolStripItalic.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripItalic.Name = "toolStripItalic";
            this.toolStripItalic.Size = new System.Drawing.Size(23, 30);
            this.toolStripItalic.Text = "Italic";
            // 
            // toolStripUnderline
            // 
            this.toolStripUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripUnderline.Image = global::LetterOfOffer.Properties.Resources.icons8_underline_12;
            this.toolStripUnderline.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripUnderline.Name = "toolStripUnderline";
            this.toolStripUnderline.Size = new System.Drawing.Size(23, 30);
            this.toolStripUnderline.Text = "Underline";
            // 
            // ParagraphView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelToolBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panelParagraph);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ParagraphView";
            this.Size = new System.Drawing.Size(718, 550);
            this.Load += new System.EventHandler(this.ParagraphView_Load);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelToolBar.ResumeLayout(false);
            this.toolStripEditor.ResumeLayout(false);
            this.toolStripEditor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelParagraph;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave_dataParagraphs;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelToolBar;
        private System.Windows.Forms.ToolStrip toolStripEditor;
        private System.Windows.Forms.ToolStripButton toolStripBold;
        private System.Windows.Forms.ToolStripButton toolStripItalic;
        private System.Windows.Forms.ToolStripButton toolStripUnderline;
        private System.Windows.Forms.ToolStripComboBox toolStripDropDownFontSize;
        private System.Windows.Forms.ToolStripComboBox toolStripDropDownFontFamily;
    }
}
