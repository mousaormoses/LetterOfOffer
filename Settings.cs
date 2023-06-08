using LetterOfOffer.Tabs;
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

namespace LetterOfOffer
{
    public partial class Settings : Form
    {
        // The main form of the application
        private Form1 mainForm;

        // Load the settings from an external file
        AppSettings settings = AppSettings.Load();

        // Constructor of the Settings class
        // This also initiates the SQLite database for storing settings
        public Settings(Form1 form1)
        {
            // Initialize the form components
            InitializeComponent();

            // Get the reference to the main form
            mainForm = form1;

            // Display the path of the database in a label
            label30.Text = settings.DbPath;

            // Assign event handler for form loading
            this.Load += new System.EventHandler(this.Form_Load);

            try
            {
                // Retrieve database path from the settings
                string dbPath = settings.DbPath;

                // Ensure the directory for the database exists, create if not
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Formulate the connection string for SQLite database
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                // Create a new SQLite connection with the connection string
                using (var conn = new SQLiteConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    // SQL statement for creating a new table in the database if it does not exist
                    string createTableSql = @"CREATE TABLE IF NOT EXISTS header_footer 
                                    (
                                        Type TEXT PRIMARY KEY, 
                                        Content TEXT
                                    );";

                    // Execute the SQL statement
                    using (var command = new SQLiteCommand(createTableSql, conn))
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

            try
            {
                // Retrieve database path from the settings
                string dbPath = settings.DbPath;

                // Ensure the directory for the database exists, create if not
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Formulate the connection string for SQLite database
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                // Create a new SQLite connection with the connection string
                using (var conn = new SQLiteConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    // SQL statement for retrieving all records from the 'header_footer' table
                    string sql = $"SELECT Type, Content FROM header_footer;";

                    // Execute the SQL statement
                    using (var command = new SQLiteCommand(sql, conn))
                    {
                        // Process the data read from the database
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string type = reader.GetString(0);
                                string content = reader.GetString(1);

                                // Populate the header and footer text boxes in the form
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
                // Get the path to the SQLite database from the settings
                string dbPath = settings.DbPath;

                // Ensure the directory for the database file exists. If not, create it.
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Create a SQLite connection string using the path to the database
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                // Create and manage a SQLite connection using the connection string
                using (var conn = new SQLiteConnection(connectionString))
                {
                    // Open the SQLite connection
                    conn.Open();

                    // Iterate through each Control in the 'Keys' container
                    foreach (Control control in Keys.Controls) // Replace yourPanelOrGroupBox with your actual container name
                    {
                        // If the control is a TextBox
                        if (control is TextBox textBox)
                        {
                            // Get the name of the TextBox (to be used as the key)
                            string keyName = textBox.Name;
                            // Get the text value of the TextBox (to be used as the value)
                            string keyValue = textBox.Text;

                            // Create SQL statement to insert or replace a record in the 'keys' table
                            string sql = @"INSERT OR REPLACE INTO keys (KeyName, KeyValue) 
                                   VALUES (@KeyName, @KeyValue);";
                            // Create and manage a SQLite command
                            using (var command = new SQLiteCommand(sql, conn))
                            {
                                // Add parameters to the SQL statement
                                command.Parameters.AddWithValue("@KeyName", keyName);
                                command.Parameters.AddWithValue("@KeyValue", keyValue);
                                // Execute the SQL command
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                // Show a message box indicating that the keys were saved successfully
                MessageBox.Show("Keys saved successfully!");
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
                // Show a message box indicating that an error occurred while saving
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
            // Call the method to retrieve key-value pairs from the 'keys' table
            Dictionary<string, string> keyValues = RetrieveKeyValuesFromTable("keys");

            // Iterate through each key-value pair
            foreach (KeyValuePair<string, string> keyValue in keyValues)
            {
                // Find the TextBox control whose name matches the key
                var textbox = this.Controls.Find(keyValue.Key, true).FirstOrDefault() as TextBox;

                // If the TextBox was found, set its text to the corresponding value
                if (textbox != null)
                {
                    textbox.Text = keyValue.Value;
                }
            }

            try
            {
                // Use the DbPath from the settings
                string dbPath = settings.DbPath;

                // Ensure the directory for the SQLite database exists. If not, create it.
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Create a SQLite connection string using the path to the database
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                // Create and manage a SQLite connection
                using (var conn = new SQLiteConnection(connectionString))
                {
                    // Open the SQLite connection
                    conn.Open();

                    // SQL query to create a table named 'signatures' if it does not exist
                    string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS signatures (
                id TEXT PRIMARY KEY,
                textBoxSign TEXT,
                richTextSign TEXT
            )";

                    // An array of ids for pre-filling the 'signatures' table
                    string[] ids = { "S1", "S2", "S3", "S4" };

                    // Execute the create table SQL query
                    using (var command = new SQLiteCommand(createTableQuery, conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Iterate through the ids and pre-fill the 'signatures' table with empty records if they do not exist
                    for (int i = 0; i < 4; i++)
                    {
                        string insertQuery = @"
                    INSERT OR IGNORE INTO signatures (id, textBoxSign, richTextSign)
                    VALUES (@id, '', '')
                ";

                        using (var command = new SQLiteCommand(insertQuery, conn))
                        {
                            // Add the id parameter to the SQL query
                            command.Parameters.AddWithValue("@id", ids[i]);
                            // Execute the insert query
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
                // Show a message box indicating that an error occurred while saving
                MessageBox.Show("An error occurred while loading Settings.");
            }
        }

        /// <summary>
        /// Retrieves all key-value pairs from the specified table in the SQLite database.
        /// </summary>
        /// <param name="tableName">The name of the SQLite table from which to retrieve the key-value pairs.</param>
        /// <returns>A dictionary containing the key-value pairs.</returns>
        private Dictionary<string, string> RetrieveKeyValuesFromTable(string tableName)
        {
            // Initialize a new Dictionary to hold the key-value pairs
            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            try
            {
                // Get the path to the SQLite database from the settings
                string dbPath = settings.DbPath;

                // Ensure the directory for the SQLite database file exists. If not, create it.
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Create a SQLite connection string using the path to the database
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                // Create and manage a SQLite connection
                using (var conn = new SQLiteConnection(connectionString))
                {
                    // Open the SQLite connection
                    conn.Open();

                    // Create a SQL statement to select all records from the specified table
                    string sql = $"SELECT KeyName, KeyValue FROM {tableName};";

                    // Create and manage a SQLite command
                    using (var command = new SQLiteCommand(sql, conn))
                    {
                        // Execute the command and manage a SQLite data reader
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // While there are records to read
                            while (reader.Read())
                            {
                                // Get the key and value from the current record
                                string key = reader.GetString(0);
                                string value = reader.GetString(1);

                                // Add the key-value pair to the dictionary
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

            // Return the dictionary containing the key-value pairs
            return keyValues;
        }


        private void Data_Click(object sender, EventArgs e)
        {

        }


        private void headerUpload_Click(object sender, EventArgs e)
        {
            // Create a new OpenFileDialog instance
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                // Set the filter for file extensions, to allow only text files
                Filter = "Text files (*.txt;)|*.txt;"
            };

            // Show the OpenFileDialog and continue if the user selects a file and clicks "OK"
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Get the full path of the selected file
                    string pathToTextFile = openFileDialog.FileName;

                    // Read all the text from the selected file
                    string text = File.ReadAllText(pathToTextFile);

                    // Set the RichTextBox's text to the text from the file
                    richTextHeader.Text = text;

                    // Get the full path of the directory where the executable is located
                    string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    // Define the full path of the "Resources" directory within the executable directory
                    string resourcesDirectory = Path.Combine(exeDirectory, "Resources");

                    // Create the "Resources" directory if it doesn't already exist
                    Directory.CreateDirectory(resourcesDirectory);

                    // Define the full path of the new text file within the "Resources" directory
                    string newTextFilePath = Path.Combine(resourcesDirectory, "header.txt");

                    // Write the text to the new text file, creating it if it doesn't already exist
                    File.WriteAllText(newTextFilePath, text);
                }
                catch (Exception ex)
                {
                    // Show a message box with an error message if there's an exception
                    MessageBox.Show($"There was an error loading the text file: {ex.Message}\nPlease ensure the file is a valid text format and try again.");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve the database path from the settings
                string dbPath = settings.DbPath;

                // Ensure the directory for the SQLite database file exists. If not, create it.
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Create a SQLite connection string using the path to the database
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                // Create and manage a SQLite connection
                using (var conn = new SQLiteConnection(connectionString))
                {
                    // Open the SQLite connection
                    conn.Open();

                    // Create a SQL statement to insert or replace the 'header' record in the 'header_footer' table
                    string headerSql = @"INSERT OR REPLACE INTO header_footer (Type, Content) 
                         VALUES ('header', @HeaderContent);";
                    using (var command = new SQLiteCommand(headerSql, conn))
                    {
                        // Add the RTF content of the richTextHeader control as a parameter
                        command.Parameters.AddWithValue("@HeaderContent", richTextHeader.Rtf);
                        // Execute the command
                        command.ExecuteNonQuery();
                    }

                    // Create a SQL statement to insert or replace the 'footer' record in the 'header_footer' table
                    string footerSql = @"INSERT OR REPLACE INTO header_footer (Type, Content) 
                         VALUES ('footer', @FooterContent);";
                    using (var command = new SQLiteCommand(footerSql, conn))
                    {
                        // Add the RTF content of the richTextFooter control as a parameter
                        command.Parameters.AddWithValue("@FooterContent", richTextFooter.Rtf);
                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }
                // Notify the user of successful save operation
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
            // Create a new OpenFileDialog instance
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            // Set the filter for file extensions, to allow only SQLite database files
            openFileDialog.Filter = "SQLite Database (*.sqlite)|*.sqlite";
            // Set the title of the dialog
            openFileDialog.Title = "Import SQLite Database";
            

            // Show the OpenFileDialog and continue if the user selects a file and clicks "OK"
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the full path of the selected file
                string sourceFilePath = openFileDialog.FileName;
                // Retrieve the application database path from the settings
                string dbPath = settings.DbPath;

                try
                {

                    // Ensure the source file exists before attempting to copy it
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        // Copy the source file to the destination path, overwriting it if it already exists
                        System.IO.File.Copy(sourceFilePath, dbPath, true); // Overwrite existing file
                                                                           // Notify the user of successful import operation
                        MessageBox.Show("Database has been imported successfully!");
                    }
                    else
                    {
                        // Notify the user if the source file does not exist
                        MessageBox.Show("Error: The source file does not exist.");
                    }
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show($"An error occurred while importing the database: {ioEx.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}");
                }
            }
        }

        private void ExportBtn_Click(object sender, EventArgs e)
        {
            // Create a new SaveFileDialog instance
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            // Set the filter for file extensions, to save the file as SQLite database file
            saveFileDialog.Filter = "SQLite Database (*.sqlite)|*.sqlite";
            // Set the title of the dialog
            saveFileDialog.Title = "Export SQLite Database";
            // Set the default name for the file
            saveFileDialog.FileName = "MyDatabase.sqlite";
            

            // Show the SaveFileDialog and continue if the user selects a location and clicks "Save"
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Retrieve the application database path from the settings
                string dbPath = settings.DbPath;
                // Get the full path where the file will be saved
                string destinationFilePath = saveFileDialog.FileName;

                try
                {
                    // Ensure the source file exists before attempting to copy it
                    if (System.IO.File.Exists(dbPath))
                    {
                        // Copy the source file to the destination path, overwriting it if it already exists
                        System.IO.File.Copy(dbPath, destinationFilePath, true); // Overwrite existing file
                                                                                // Notify the user of successful export operation
                        MessageBox.Show("Database has been exported successfully!");
                    }
                    else
                    {
                        // Notify the user if the source file does not exist
                        MessageBox.Show("Error: The source file does not exist.");
                    }
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show($"An error occurred while exporting the database: {ioEx.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}");
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
            // Gather the text from the TextBoxes and RichTextBoxes
            string[] textBoxSigns = { sign1.Text, sign2.Text, sign3.Text, sign4.Text };
            string[] richSigns = { richTextSign1.Rtf, richTextSign2.Rtf, richTextSign3.Rtf, richTextSign4.Rtf };

            // Retrieve the application database path from the settings
            string dbPath = settings.DbPath;

            // Ensure the directory containing the database file exists
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

            // Construct the SQLite connection string
            string connectionString = "Data Source=" + dbPath + ";Version=3;";

            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Loop through each signature textbox
                    for (int i = 0; i < 4; i++)
                    {
                        string updateQuery = @"
                UPDATE signatures 
                SET textBoxSign = @textBoxSign, richTextSign = @richTextSign
                WHERE id = @id
            ";

                        using (var command = new SQLiteCommand(updateQuery, conn))
                        {
                            // Bind the textbox and richtextbox contents to the command parameters
                            command.Parameters.AddWithValue("@textBoxSign", textBoxSigns[i]);
                            command.Parameters.AddWithValue("@richTextSign", richSigns[i]);
                            command.Parameters.AddWithValue("@id", "S" + (i + 1));  // IDs are S1, S2, S3, S4

                            // Execute the query
                            command.ExecuteNonQuery();
                        }
                    }
                }

                // Notify the user of successful save operation
                MessageBox.Show("Signatures saved successfully!");
            }
            catch (Exception ex)
            {
                // Log the exception to the console
                Console.WriteLine(ex.ToString());
                // Notify the user if an error occurs during the save operation
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
                // Retrieve the application database path from the settings
                string dbPath = settings.DbPath;

                // Ensure the directory containing the database file exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Construct the SQLite connection string
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                // Create arrays of TextBoxes and RichTextBoxes
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
                            // As long as there are more rows and we haven't filled all the TextBoxes
                            while (reader.Read() && i < textBoxes.Length)
                            {
                                // Populate the TextBoxes and RichTextBoxes with the corresponding database fields
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

        private void uploadImageBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Initialize a new OpenFileDialog
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                // Specify filter options and filter index for the OpenFileDialog
                openFileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Specify a path to a local directory and create the directory if it doesn't exist
                    string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LetterOfOffer", "Images");
                    Directory.CreateDirectory(directoryPath);

                    // Define the path to the new image file
                    string path = Path.Combine(directoryPath, "LetterOfOffer.jpg");

                    // Save the selected image as a new JPEG file
                    using (var img = Image.FromFile(openFileDialog.FileName))
                    {
                        img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    // Update the image displayed in the PictureBox on the main form
                    using (var bmpTemp = new Bitmap(path))
                    {
                        Image imgOld = mainForm.pictureBox1.Image;
                        mainForm.pictureBox1.Image = new Bitmap(bmpTemp);

                        // Dispose of the old image to free up resources
                        if (imgOld != null)
                        {
                            imgOld.Dispose();
                        }
                    }

                    // Update the image displayed in the PictureBox on the current form
                    using (var bmpTemp = new Bitmap(path))
                    {
                        Image imgOld = pictureBox2.Image;
                        pictureBox2.Image = new Bitmap(bmpTemp);

                        // Dispose of the old image to free up resources
                        if (imgOld != null)
                        {
                            imgOld.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Display an error message to the user
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void newPathBtn_Click(object sender, EventArgs e)
        {
            // Create a FolderBrowserDialog to allow the user to pick a new directory
            using (var dialog = new FolderBrowserDialog())
            {
                // Show the dialog and get the result
                DialogResult result = dialog.ShowDialog();

                // If the user clicked OK and selected a non-empty path, change the destination path
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    // Combine the selected path with the filename to get the new destination file path
                    string destinationFilePath = Path.Combine(dialog.SelectedPath, "MyDatabase.sqlite");

                    // Load the settings
                    AppSettings settings = AppSettings.Load();

                    // Update the settings with the new path
                    settings.DbPath = destinationFilePath;

                    // Save the updated settings
                    settings.Save();

                    // Display a message box to inform the user of the new file path
                    MessageBox.Show($"The new file path is: {destinationFilePath}");
                }
            }
        }

    }
}
