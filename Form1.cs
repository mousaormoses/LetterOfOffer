using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using System.Data.SQLite;
using LetterOfOffer.Tabs;

namespace LetterOfOffer
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            // Load the settings
            AppSettings settings = AppSettings.Load();
            try
            {
                // Connect to the SQLite database
                string dbPath = settings.DbPath;


                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Create the keys table if it doesn't exist
                    string sql = @"CREATE TABLE IF NOT EXISTS keys
                               (KeyName TEXT PRIMARY KEY,
                                KeyValue TEXT NOT NULL);";
                    using (var command = new SQLiteCommand(sql, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            FormView panel = new FormView();
            addUserControl(panel);

            // Create the directory if it doesn't exist
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LetterOfOffer", "Images");
            Directory.CreateDirectory(directoryPath);

            string path = Path.Combine(directoryPath, "LetterOfOffer.jpg");
            if (File.Exists(path))
            {
                // Create a new Bitmap from the file and set it to the PictureBox, disposing the old image
                using (var bmpTemp = new Bitmap(path))
                {
                    Image imgOld = pictureBox1.Image;
                    pictureBox1.Image = new Bitmap(bmpTemp);
                    if (imgOld != null)
                    {
                        imgOld.Dispose();
                    }
                }
            }

        }

        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(userControl);
            userControl.BringToFront();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            FormView panel = new FormView();
            addUserControl(panel);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TemplateView panel = new TemplateView();
            addUserControl(panel);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ParagraphView panel = new ParagraphView();
            addUserControl(panel);
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void panel3_MouseHover(object sender, EventArgs e)
        {
            // button4.BackColor = Color.Blue;
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            // button1.BackColor = Color.FromArgb(105, 105, 105);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            //button1.BackColor = Color.FromArgb(240, 240, 105);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            // button1.BackColor = Color.FromArgb(105,105,105);
        }

        private void settingsBox_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Settings settings = new Settings(this);
            settings.ShowDialog();

        }

        private void OpenSettings()
        {
            Settings settingsForm = new Settings(this);
            settingsForm.Show();
        }

        public void SetImage(Image image)
        {
            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }


        public void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}