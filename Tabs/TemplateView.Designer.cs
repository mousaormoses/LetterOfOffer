namespace LetterOfOffer.Tabs
{
    partial class TemplateView
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
            this.newTemp = new System.Windows.Forms.Button();
            this.panelTemp = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // newTemp
            // 
            this.newTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(140)))), ((int)(((byte)(194)))));
            this.newTemp.FlatAppearance.BorderSize = 0;
            this.newTemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newTemp.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.newTemp.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.newTemp.Location = new System.Drawing.Point(558, 4);
            this.newTemp.Margin = new System.Windows.Forms.Padding(2);
            this.newTemp.Name = "newTemp";
            this.newTemp.Size = new System.Drawing.Size(148, 35);
            this.newTemp.TabIndex = 0;
            this.newTemp.Text = "New Template";
            this.newTemp.UseVisualStyleBackColor = false;
            this.newTemp.Click += new System.EventHandler(this.newTemp_Click);
            // 
            // panelTemp
            // 
            this.panelTemp.AutoScroll = true;
            this.panelTemp.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.panelTemp.Location = new System.Drawing.Point(3, 46);
            this.panelTemp.Margin = new System.Windows.Forms.Padding(2);
            this.panelTemp.Name = "panelTemp";
            this.panelTemp.Size = new System.Drawing.Size(715, 450);
            this.panelTemp.TabIndex = 1;
            this.panelTemp.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTemp_Paint);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(140)))), ((int)(((byte)(194)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(718, 44);
            this.label2.TabIndex = 2;
            this.label2.Text = "Templates";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.newTemp);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 501);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(718, 49);
            this.panel2.TabIndex = 36;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // TemplateView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panelTemp);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TemplateView";
            this.Size = new System.Drawing.Size(718, 550);
            this.Load += new System.EventHandler(this.TemplateView_Load);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button newTemp;
        private System.Windows.Forms.Panel panelTemp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
    }
}
