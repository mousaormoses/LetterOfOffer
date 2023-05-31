using LetterOfOffer_18.Tabs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LetterOfOffer_18
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.Form_Load);
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string createTableSql = @"CREATE TABLE IF NOT EXISTS header_footer 
                                    (
                                        Type TEXT PRIMARY KEY, 
                                        Content TEXT
                                    );";

                    using (var command = new SQLiteCommand(createTableSql, conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS signatures (
                        id TEXT PRIMARY KEY,
                        textBoxSign TEXT,
                        richTextSign TEXT
                    )";

                    string[] ids = { "S1", "S2", "S3", "S4" };


                    using (var command = new SQLiteCommand(createTableQuery, conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Pre-fill the four records
                    for (int i = 0; i < 4; i++)
                    {
                        string insertQuery = @"
                            INSERT OR IGNORE INTO signatures (id, textBoxSign, richTextSign)
                            VALUES (@id, '', '')
                        ";

                        using (var command = new SQLiteCommand(insertQuery, conn))
                        {
                            command.Parameters.AddWithValue("@id", ids[i]);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
            }


            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = $"SELECT Type, Content FROM header_footer;";

                    using (var command = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string type = reader.GetString(0);
                                string content = reader.GetString(1);
                                if (type == "header")
                                    richTextHeader.Rtf = content;
                                else if (type == "footer")
                                    richTextFooter.Rtf = content;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
            }
        }




        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Keys_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Iterate through each TextBox in the specific container
                    foreach (Control control in Keys.Controls) // Replace yourPanelOrGroupBox with your actual container name
                    {
                        if (control is TextBox textBox)
                        {
                            string keyName = textBox.Name;
                            string keyValue = textBox.Text;

                            string sql = @"INSERT OR REPLACE INTO keys (KeyName, KeyValue) 
                               VALUES (@KeyName, @KeyValue);";
                            using (var command = new SQLiteCommand(sql, conn))
                            {
                                command.Parameters.AddWithValue("@KeyName", keyName);
                                command.Parameters.AddWithValue("@KeyValue", keyValue);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                MessageBox.Show("Keys saved successfully!");
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
                MessageBox.Show("An error occurred while saving signatures.");
            }
        }


        private void keyBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxEX_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxPosition_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxSector_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxStartDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxLanguage_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxSalaryFrom_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxStartTo_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxHR_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxSecurity_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxProvince_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxCity_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxPostal_TextChanged(object sender, EventArgs e)
        {

        }

        private void keyBoxSignature_TextChanged(object sender, EventArgs e)
        {

        }

        private void Settings_Load(object sender, EventArgs e)
        {
            // Retrieve keys and values from the keys table
            Dictionary<string, string> keyValues = RetrieveKeyValuesFromTable("keys");

            // Load each value into the corresponding textbox
            foreach (KeyValuePair<string, string> keyValue in keyValues)
            {
                // Find the textbox that matches the key
                var textbox = this.Controls.Find(keyValue.Key, true).FirstOrDefault() as TextBox;

                // If the textbox was found, set its text to the value
                if (textbox != null)
                {
                    textbox.Text = keyValue.Value;
                }
            }
        }

        private Dictionary<string, string> RetrieveKeyValuesFromTable(string tableName)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = $"SELECT KeyName, KeyValue FROM {tableName};";

                    using (var command = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string key = reader.GetString(0);
                                string value = reader.GetString(1);
                                keyValues[key] = value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
            }

            return keyValues;
        }

        private void Data_Click(object sender, EventArgs e)
        {

        }

        private void headerUpload_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                // Filter for text files
                Filter = "Text files (*.txt;)|*.txt;"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string pathToTextFile = openFileDialog.FileName;

                    // Load the text from the file
                    string text = File.ReadAllText(pathToTextFile);

                    // Assign the text to the RichTextBox
                    richTextHeader.Text = text;

                    // Get the directory of the executable
                    string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    // Define the path to the "Resources" directory
                    string resourcesDirectory = Path.Combine(exeDirectory, "Resources");

                    // Create the "Resources" directory if it doesn't already exist
                    Directory.CreateDirectory(resourcesDirectory);

                    // Save the text to the "Resources" directory
                    string newTextFilePath = Path.Combine(resourcesDirectory, "header.txt");
                    File.WriteAllText(newTextFilePath, text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error loading the text file: {ex.Message}\nPlease ensure the file is a valid text format and try again.");
                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string headerSql = @"INSERT OR REPLACE INTO header_footer (Type, Content) 
                             VALUES ('header', @HeaderContent);";
                    using (var command = new SQLiteCommand(headerSql, conn))
                    {
                        command.Parameters.AddWithValue("@HeaderContent", richTextHeader.Rtf);
                        command.ExecuteNonQuery();
                    }

                    string footerSql = @"INSERT OR REPLACE INTO header_footer (Type, Content) 
                             VALUES ('footer', @FooterContent);";
                    using (var command = new SQLiteCommand(footerSql, conn))
                    {
                        command.Parameters.AddWithValue("@FooterContent", richTextFooter.Rtf);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Header and Footer saved successfully!");
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
                MessageBox.Show("An error occurred while saving signatures.");
            }
        }

        private void ImportBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "SQLite Database (*.sqlite)|*.sqlite";
            openFileDialog.Title = "Import SQLite Database";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string sourceFilePath = openFileDialog.FileName;
                string destinationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Make sure the source file exists before trying to copy it
                if (System.IO.File.Exists(sourceFilePath))
                {
                    System.IO.File.Copy(sourceFilePath, destinationFilePath, true); // Overwrite existing file
                    MessageBox.Show("Database has been imported successfully!");
                }
                else
                {
                    MessageBox.Show("Error: The source file does not exist.");
                }
            }
        }


        private void ExportBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "SQLite Database (*.sqlite)|*.sqlite";
            saveFileDialog.Title = "Export SQLite Database";
            saveFileDialog.FileName = "MyDatabase.sqlite"; // Default file name

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string sourceFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");
                string destinationFilePath = saveFileDialog.FileName;

                // Make sure the source file exists before trying to copy it
                if (System.IO.File.Exists(sourceFilePath))
                {
                    System.IO.File.Copy(sourceFilePath, destinationFilePath, true); // Overwrite existing file
                    MessageBox.Show("Database has been exported successfully!");
                }
                else
                {
                    MessageBox.Show("Error: The source file does not exist.");
                }
            }
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextFooter_TextChanged(object sender, EventArgs e)
        {

        }

        private void sign1_TextChanged(object sender, EventArgs e)
        {

        }

        private void sign2_TextChanged(object sender, EventArgs e)
        {

        }

        private void sign3_TextChanged(object sender, EventArgs e)
        {

        }

        private void sign4_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextSign1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextSign2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextSign3_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextSign4_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSign_Click(object sender, EventArgs e)
        {
            string[] textBoxSigns = { sign1.Text, sign2.Text, sign3.Text, sign4.Text };
            string[] richSigns = { richTextSign1.Rtf, richTextSign2.Rtf, richTextSign3.Rtf, richTextSign4.Rtf };

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

            // Use the dbPath variable when creating your SQLite connection
            string connectionString = "Data Source=" + dbPath + ";Version=3;";

            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    for (int i = 0; i < 4; i++)
                    {
                        string updateQuery = @"
                    UPDATE signatures 
                    SET textBoxSign = @textBoxSign, richTextSign = @richTextSign
                    WHERE id = @id
                ";

                        using (var command = new SQLiteCommand(updateQuery, conn))
                        {
                            command.Parameters.AddWithValue("@textBoxSign", textBoxSigns[i]);
                            command.Parameters.AddWithValue("@richTextSign", richSigns[i]);
                            command.Parameters.AddWithValue("@id", "S" + (i + 1));  // S1, S2, S3, S4

                            command.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Signatures saved successfully!");
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
                MessageBox.Show("An error occurred while saving signatures.");
            }
        }





        private void Signature_Click(object sender, EventArgs e)
        {

        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                TextBox[] textBoxes = { sign1, sign2, sign3, sign4 };
                RichTextBox[] richTexts = { richTextSign1, richTextSign2, richTextSign3, richTextSign4 };

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string selectQuery = "SELECT * FROM signatures;";

                    using (var command = new SQLiteCommand(selectQuery, conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            int i = 0;
                            while (reader.Read() && i < textBoxes.Length)
                            {
                                textBoxes[i].Text = reader["textBoxSign"].ToString();
                                richTexts[i].Rtf = reader["richTextSign"].ToString();

                                i++;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
            }
        }


        private void richTextHeader_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
