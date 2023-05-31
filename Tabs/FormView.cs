using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace LetterOfOffer_18.Tabs
{
    public partial class FormView : UserControl
    {
        public FormView()
        {
            InitializeComponent();
        }

        private void LoadItemsFromDatabase(ComboBox comboBox)
        {
            string tableName = GenerateTableName(comboBox);
            string selectSql = $"SELECT ItemName FROM {tableName}";
            try
            {

                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var selectCommand = new SQLiteCommand(selectSql, connection))
                    {
                        using (var reader = selectCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string itemName = reader.GetString(0);
                                comboBox.Items.Add(itemName);
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

        private void EditWishlistItems(ComboBox comboBox)
        {
            // Open a new form for entering and modifying wishlist items
            Form prompt = new Form();
            prompt.Width = 300;
            prompt.Text = "Edit list Items";
            prompt.FormBorderStyle = FormBorderStyle.FixedSingle; // Disable resizing
            prompt.MaximizeBox = false; // Disable maximize button
            prompt.MinimizeBox = false; // Disable minimize button
            prompt.ShowIcon = false; // Hide the form icon

            // Create a TextBox for entering and displaying the items
            TextBox itemsTextBox = new TextBox() { Left = 20, Top = 20, Width = 250, Height = 100, Multiline = true };
            prompt.Controls.Add(itemsTextBox);

            // Populate the TextBox with the current items from the ComboBox
            itemsTextBox.Text = string.Join(Environment.NewLine, comboBox.Items.Cast<string>());

            // Create a Button for saving the modified items
            Button saveButton = new Button()
            {
                Text = "Save",
                Left = 20,
                Top = itemsTextBox.Bottom + 20,
                Width = 100,
                BackColor = System.Drawing.Color.Black,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            prompt.Controls.Add(saveButton);

            saveButton.Click += (s, ev) =>
            {
                // Split the modified items by newline
                string[] items = itemsTextBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                // Clear the ComboBox
                comboBox.Items.Clear();

                // Add the modified items to the ComboBox
                comboBox.Items.AddRange(items);

                // Generate the table name for the combo box
                string tableName = GenerateTableName(comboBox);

                try
                {

                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");

                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                    // Use the dbPath variable when creating your SQLite connection
                    string connectionString = "Data Source=" + dbPath + ";Version=3;";

                    // Save the modified items to the corresponding table in the database
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        // Create the table if it doesn't exist
                        string createTableSql = $"CREATE TABLE IF NOT EXISTS {tableName} (ID INTEGER PRIMARY KEY AUTOINCREMENT, ItemName TEXT)";
                        using (var createTableCommand = new SQLiteCommand(createTableSql, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                        }

                        // Clear the existing items in the table
                        string deleteSql = $"DELETE FROM {tableName}";
                        using (var deleteCommand = new SQLiteCommand(deleteSql, connection))
                        {
                            deleteCommand.ExecuteNonQuery();
                        }

                        // Insert the modified items into the table
                        string insertSql = $"INSERT INTO {tableName} (ItemName) VALUES (@ItemName)";
                        using (var insertCommand = new SQLiteCommand(insertSql, connection))
                        {
                            foreach (string item in items)
                            {
                                insertCommand.Parameters.AddWithValue("@ItemName", item);
                                insertCommand.ExecuteNonQuery();
                                insertCommand.Parameters.Clear();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or display the exception as needed
                    Console.WriteLine(ex.ToString());
                }

                // Close the prompt form
                prompt.Close();
            };

            // Calculate the form's height based on the position of the save button
            prompt.Height = saveButton.Bottom + 50;

            prompt.StartPosition = FormStartPosition.CenterScreen; // Set the start position to center on the screen
            prompt.ShowDialog();
        }


        private void FormView_Load(object sender, EventArgs e)
        {
            PopulateTableNames();
            load_signatures();


            try
            {
                // Create the tables if they don't exist
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    CreateTableIfNotExists(connection, sector_FormBox);
                    CreateTableIfNotExists(connection, ex_FormBox);
                    CreateTableIfNotExists(connection, salaryFrom_FormBox);
                    CreateTableIfNotExists(connection, hr_FormBox);
                    CreateTableIfNotExists(connection, province_FormBox);
                    CreateTableIfNotExists(connection, signature_FormBox);
                    CreateTableIfNotExists(connection, language_FormBox);
                    CreateTableIfNotExists(connection, salaryTo_FormBox);
                    CreateTableIfNotExists(connection, security_FormBox);
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
            }


            try
            {
                // Load the items from the corresponding tables in the database and populate the combo boxes
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    LoadItemsFromDatabase(sector_FormBox);
                    LoadItemsFromDatabase(ex_FormBox);
                    LoadItemsFromDatabase(salaryFrom_FormBox);
                    LoadItemsFromDatabase(hr_FormBox);
                    LoadItemsFromDatabase(province_FormBox);
                    LoadItemsFromDatabase(signature_FormBox);
                    LoadItemsFromDatabase(language_FormBox);
                    LoadItemsFromDatabase(salaryTo_FormBox);
                    LoadItemsFromDatabase(security_FormBox);
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
            }


        }

        private void CreateTableIfNotExists(SQLiteConnection connection, ComboBox comboBox)
        {
            string tableName = GenerateTableName(comboBox);
            string createTableSql = $"CREATE TABLE IF NOT EXISTS {tableName} (ID INTEGER PRIMARY KEY AUTOINCREMENT, ItemName TEXT)";

            using (var createTableCommand = new SQLiteCommand(createTableSql, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PopulateTableNames()
        {
            // Clear any existing items in the ComboBox
            comboBox1.Items.Clear();
            try
            {
                // Establish a connection to the SQLite database
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve the table names from the database schema
                    System.Data.DataTable tableNames = connection.GetSchema("Tables");

                    // Filter and add only the table names starting with "template_"
                    foreach (DataRow row in tableNames.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();

                        // Check if the table name starts with "template_"
                        if (tableName.StartsWith("template_"))
                        {
                            // Remove the "template_" prefix from the table name
                            string displayTableName = tableName.Substring("template_".Length);
                            comboBox1.Items.Add(displayTableName);
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


        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }






        private string GenerateTableName(ComboBox comboBox)
        {
            // Modify this method according to your naming convention for table names
            return $"wishlist_{comboBox.Name}";
        }





        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            EditWishlistItems(province_FormBox);

        }

        private void btnEXlevel_Click(object sender, EventArgs e)
        {
            EditWishlistItems(ex_FormBox);
        }

        private void btnSector_Click(object sender, EventArgs e)
        {
            EditWishlistItems(sector_FormBox);
        }

        private void btnLanguage_Click(object sender, EventArgs e)
        {
            EditWishlistItems(language_FormBox);
        }

        private void btnSalaryFrom_Click(object sender, EventArgs e)
        {
            EditWishlistItems(salaryFrom_FormBox);
        }

        private void btnSalaryTo_Click(object sender, EventArgs e)
        {
            EditWishlistItems(security_FormBox);
        }

        private void btnHR_Click(object sender, EventArgs e)
        {
            EditWishlistItems(hr_FormBox);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            EditWishlistItems(security_FormBox);
        }
        private void load_signatures()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");

            // Use the dbPath variable when creating your SQLite connection
            string connectionString = "Data Source=" + dbPath + ";Version=3;";

            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string selectQuery = "SELECT textBoxSign FROM signatures;";

                    using (var command = new SQLiteCommand(selectQuery, conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            signature_FormBox.Items.Clear();
                            while (reader.Read())
                            {
                                signature_FormBox.Items.Add(reader["textBoxSign"].ToString());
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportToWord();

        }


        private void ExportToWord()
        {
            if (comboBox1.SelectedItem != null)
            {
                // Get the selected table name from comboBox1
                string selectedTableName = comboBox1.SelectedItem.ToString();

                // Remove the "template_" prefix from the selected table name
                string tableNameWithoutPrefix = selectedTableName.StartsWith("template_")
                    ? selectedTableName.Substring("template_".Length)
                    : selectedTableName;

                // Add the "template_" prefix to the table name
                string tableNameWithPrefix = "template_" + tableNameWithoutPrefix;

                // Retrieve the content from the selected table
                List<string> contentList = RetrieveContentFromTable(tableNameWithPrefix);

                string fullName = fullName_FormBox.Text;
                string ex = ex_FormBox.Text;
                string sector = sector_FormBox.Text;
                string position = position_FormBox.Text;
                string startDate = startDate_FormBox.Text;
                string language = language_FormBox.Text;
                string salaryFrom = salaryFrom_FormBox.Text;
                string salaryTo = salaryTo_FormBox.Text;
                string hr = hr_FormBox.Text;
                string security = security_FormBox.Text;
                string address = address_FormBox.Text;
                string province = province_FormBox.Text;
                string city = city_FormBox.Text;
                string postal = postal_FormBox.Text;
                string textBoxSign = signature_FormBox.Text;
                string signature = RetrieveRichTextSignatureForTextBoxSign(signature_FormBox.Text);

                string keyBoxName = RetrieveSpecificKeyValueFromTable("keys", "keyBoxName");
                string keyBoxEX = RetrieveSpecificKeyValueFromTable("keys", "keyBoxEX");
                string keyBoxSector = RetrieveSpecificKeyValueFromTable("keys", "keyBoxSector");
                string keyBoxPosition = RetrieveSpecificKeyValueFromTable("keys", "keyBoxPosition");
                string keyBoxStartDate = RetrieveSpecificKeyValueFromTable("keys", "keyBoxStartDate");
                string keyBoxLanguage = RetrieveSpecificKeyValueFromTable("keys", "keyBoxLanguage");
                string keyBoxSalaryFrom = RetrieveSpecificKeyValueFromTable("keys", "keyBoxSalaryFrom");
                string keyBoxStartTo = RetrieveSpecificKeyValueFromTable("keys", "keyBoxStartTo");
                string keyBoxHR = RetrieveSpecificKeyValueFromTable("keys", "keyBoxHR");
                string keyBoxSecurity = RetrieveSpecificKeyValueFromTable("keys", "keyBoxSecurity");
                string keyBoxProvince = RetrieveSpecificKeyValueFromTable("keys", "keyBoxProvince");
                string keyBoxCity = RetrieveSpecificKeyValueFromTable("keys", "keyBoxCity");
                string keyBoxPostal = RetrieveSpecificKeyValueFromTable("keys", "keyBoxPostal");
                string keyBoxSignature = RetrieveSpecificKeyValueFromTable("keys", "keyBoxSignature");
                string keyBoxAddress = RetrieveSpecificKeyValueFromTable("keys", "keyBoxAddress");

                for (int i = 0; i < contentList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(keyBoxName))
                        contentList[i] = contentList[i].Replace(keyBoxName, fullName);
                    if (!string.IsNullOrEmpty(keyBoxEX))
                        contentList[i] = contentList[i].Replace(keyBoxEX, ex);
                    if (!string.IsNullOrEmpty(keyBoxSector))
                        contentList[i] = contentList[i].Replace(keyBoxSector, sector);
                    if (!string.IsNullOrEmpty(keyBoxPosition))
                        contentList[i] = contentList[i].Replace(keyBoxPosition, position);
                    if (!string.IsNullOrEmpty(keyBoxStartDate))
                        contentList[i] = contentList[i].Replace(keyBoxStartDate, startDate);
                    if (!string.IsNullOrEmpty(keyBoxLanguage))
                        contentList[i] = contentList[i].Replace(keyBoxLanguage, language);
                    if (!string.IsNullOrEmpty(keyBoxSalaryFrom))
                        contentList[i] = contentList[i].Replace(keyBoxSalaryFrom, salaryFrom);
                    if (!string.IsNullOrEmpty(keyBoxStartTo))
                        contentList[i] = contentList[i].Replace(keyBoxStartTo, salaryTo);
                    if (!string.IsNullOrEmpty(keyBoxHR))
                        contentList[i] = contentList[i].Replace(keyBoxHR, hr);
                    if (!string.IsNullOrEmpty(keyBoxSecurity))
                        contentList[i] = contentList[i].Replace(keyBoxSecurity, security);
                    if (!string.IsNullOrEmpty(keyBoxAddress))
                        contentList[i] = contentList[i].Replace(keyBoxAddress, address);
                    if (!string.IsNullOrEmpty(keyBoxProvince))
                        contentList[i] = contentList[i].Replace(keyBoxProvince, province);
                    if (!string.IsNullOrEmpty(keyBoxCity))
                        contentList[i] = contentList[i].Replace(keyBoxCity, city);
                    if (!string.IsNullOrEmpty(keyBoxPostal))
                        contentList[i] = contentList[i].Replace(keyBoxPostal, postal);
                    if (!string.IsNullOrEmpty(keyBoxSignature))
                        contentList[i] = contentList[i].Replace(keyBoxSignature, signature);
                }


                // Create a new Word document
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Document doc = wordApp.Documents.Add();

                string headerText = RetrieveHeaderFooterContent("header");
                string footerText = RetrieveHeaderFooterContent("footer");
                // Before trying to add a header, check if the headerText is not null or empty.
                if (!string.IsNullOrEmpty(headerText.Trim())) // Trim is used to remove leading or trailing whitespaces.
                {
                    AddHeader(doc, headerText);
                }

                // Do the same for the footer.
                if (!string.IsNullOrEmpty(footerText.Trim())) // Trim is used to remove leading or trailing whitespaces.
                {
                    AddFooter(doc, footerText);
                }




                // Insert each content item into the Word document
                foreach (string content in contentList)
                {
                    bool success = false;
                    int retryCount = 0;

                    while (!success && retryCount < 3)
                    {
                        try
                        {
                            Clipboard.SetText(content, TextDataFormat.Rtf);
                            doc.Application.Selection.Paste();
                            success = true;
                        }
                        catch (System.Runtime.InteropServices.COMException)
                        {
                            // The clipboard was unavailable or the paste operation failed. 
                            // We'll retry a few times in case the issue is temporary.
                            retryCount++;
                            System.Threading.Thread.Sleep(100); // Wait for a short period before retrying.
                        }
                    }

                    if (!success)
                    {
                        // The paste operation failed after several attempts. 
                        // At this point, you'll have to decide how to handle the failure.
                    }

                    doc.Application.Selection.InsertParagraphAfter();
                }


                // Show the save file dialog to choose the save location
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Word Document (*.docx)|*.docx";
                saveFileDialog.Title = "Save Word Document";
                saveFileDialog.FileName = "document.docx"; // Default file name

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    // Save the Word document
                    doc.SaveAs2(filePath);

                    // Show success message
                    MessageBox.Show("The content has been exported to Word successfully!");
                }

                // Close the Word document and release resources
                // This is done whether the user chooses to save the document or not.
                if (doc != null)
                {
                    // Close without saving any changes.
                    doc.Close(SaveChanges: false);
                    doc = null; // Prevent any further action on this document.
                }

                if (wordApp != null)
                {
                    wordApp.Quit();
                    wordApp = null; // Prevent any further action on this Word application.
                }
            }
            else
            {
                MessageBox.Show("Please select a table from the list.");
            }
        }

        private void pdfButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                // Get the selected table name from comboBox1
                string selectedTableName = comboBox1.SelectedItem.ToString();

                // Remove the "template_" prefix from the selected table name
                string tableNameWithoutPrefix = selectedTableName.StartsWith("template_")
                    ? selectedTableName.Substring("template_".Length)
                    : selectedTableName;

                // Add the "template_" prefix to the table name
                string tableNameWithPrefix = "template_" + tableNameWithoutPrefix;

                // Retrieve the content from the selected table
                List<string> contentList = RetrieveContentFromTable(tableNameWithPrefix);

                // Replace placeholders with actual values
                string fullName = fullName_FormBox.Text;
                string ex = ex_FormBox.Text;
                string sector = sector_FormBox.Text;
                string position = position_FormBox.Text;
                string startDate = startDate_FormBox.Text;
                string language = language_FormBox.Text;
                string salaryFrom = salaryFrom_FormBox.Text;
                string salaryTo = salaryTo_FormBox.Text;
                string hr = hr_FormBox.Text;
                string security = security_FormBox.Text;
                string address = address_FormBox.Text;
                string province = province_FormBox.Text;
                string city = city_FormBox.Text;
                string postal = postal_FormBox.Text;
                string textBoxSign = signature_FormBox.Text;
                string signature = RetrieveRichTextSignatureForTextBoxSign(signature_FormBox.Text);

                string keyBoxName = RetrieveSpecificKeyValueFromTable("keys", "keyBoxName");
                string keyBoxEX = RetrieveSpecificKeyValueFromTable("keys", "keyBoxEX");
                string keyBoxSector = RetrieveSpecificKeyValueFromTable("keys", "keyBoxSector");
                string keyBoxPosition = RetrieveSpecificKeyValueFromTable("keys", "keyBoxPosition");
                string keyBoxStartDate = RetrieveSpecificKeyValueFromTable("keys", "keyBoxStartDate");
                string keyBoxLanguage = RetrieveSpecificKeyValueFromTable("keys", "keyBoxLanguage");
                string keyBoxSalaryFrom = RetrieveSpecificKeyValueFromTable("keys", "keyBoxSalaryFrom");
                string keyBoxStartTo = RetrieveSpecificKeyValueFromTable("keys", "keyBoxStartTo");
                string keyBoxHR = RetrieveSpecificKeyValueFromTable("keys", "keyBoxHR");
                string keyBoxSecurity = RetrieveSpecificKeyValueFromTable("keys", "keyBoxSecurity");
                string keyBoxProvince = RetrieveSpecificKeyValueFromTable("keys", "keyBoxProvince");
                string keyBoxCity = RetrieveSpecificKeyValueFromTable("keys", "keyBoxCity");
                string keyBoxPostal = RetrieveSpecificKeyValueFromTable("keys", "keyBoxPostal");
                string keyBoxSignature = RetrieveSpecificKeyValueFromTable("keys", "keyBoxSignature");
                string keyBoxAddress = RetrieveSpecificKeyValueFromTable("keys", "keyBoxAddress");

                for (int i = 0; i < contentList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(keyBoxName))
                        contentList[i] = contentList[i].Replace(keyBoxName, fullName);
                    if (!string.IsNullOrEmpty(keyBoxEX))
                        contentList[i] = contentList[i].Replace(keyBoxEX, ex);
                    if (!string.IsNullOrEmpty(keyBoxSector))
                        contentList[i] = contentList[i].Replace(keyBoxSector, sector);
                    if (!string.IsNullOrEmpty(keyBoxPosition))
                        contentList[i] = contentList[i].Replace(keyBoxPosition, position);
                    if (!string.IsNullOrEmpty(keyBoxStartDate))
                        contentList[i] = contentList[i].Replace(keyBoxStartDate, startDate);
                    if (!string.IsNullOrEmpty(keyBoxLanguage))
                        contentList[i] = contentList[i].Replace(keyBoxLanguage, language);
                    if (!string.IsNullOrEmpty(keyBoxSalaryFrom))
                        contentList[i] = contentList[i].Replace(keyBoxSalaryFrom, salaryFrom);
                    if (!string.IsNullOrEmpty(keyBoxStartTo))
                        contentList[i] = contentList[i].Replace(keyBoxStartTo, salaryTo);
                    if (!string.IsNullOrEmpty(keyBoxHR))
                        contentList[i] = contentList[i].Replace(keyBoxHR, hr);
                    if (!string.IsNullOrEmpty(keyBoxSecurity))
                        contentList[i] = contentList[i].Replace(keyBoxSecurity, security);
                    if (!string.IsNullOrEmpty(keyBoxAddress))
                        contentList[i] = contentList[i].Replace(keyBoxAddress, address);
                    if (!string.IsNullOrEmpty(keyBoxProvince))
                        contentList[i] = contentList[i].Replace(keyBoxProvince, province);
                    if (!string.IsNullOrEmpty(keyBoxCity))
                        contentList[i] = contentList[i].Replace(keyBoxCity, city);
                    if (!string.IsNullOrEmpty(keyBoxPostal))
                        contentList[i] = contentList[i].Replace(keyBoxPostal, postal);
                    if (!string.IsNullOrEmpty(keyBoxSignature))
                        contentList[i] = contentList[i].Replace(keyBoxSignature, signature);
                }


                // Create a new Word document
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Document doc = wordApp.Documents.Add();

                string headerText = RetrieveHeaderFooterContent("header");
                string footerText = RetrieveHeaderFooterContent("footer");
                // Before trying to add a header, check if the headerText is not null or empty.
                if (!string.IsNullOrEmpty(headerText.Trim())) // Trim is used to remove leading or trailing whitespaces.
                {
                    AddHeader(doc, headerText);
                }

                // Do the same for the footer.
                if (!string.IsNullOrEmpty(footerText.Trim())) // Trim is used to remove leading or trailing whitespaces.
                {
                    AddFooter(doc, footerText);
                }



                // Insert each content item into the Word document
                foreach (string content in contentList)
                {
                    bool success = false;
                    int retryCount = 0;

                    while (!success && retryCount < 3)
                    {
                        try
                        {
                            Clipboard.SetText(content, TextDataFormat.Rtf);
                            doc.Application.Selection.Paste();
                            success = true;
                        }
                        catch (System.Runtime.InteropServices.COMException)
                        {
                            // The clipboard was unavailable or the paste operation failed. 
                            // We'll retry a few times in case the issue is temporary.
                            retryCount++;
                            System.Threading.Thread.Sleep(100); // Wait for a short period before retrying.
                        }
                    }

                    if (!success)
                    {
                        // The paste operation failed after several attempts. 
                        // At this point, you'll have to decide how to handle the failure.
                    }

                    doc.Application.Selection.InsertParagraphAfter();
                }


                // Show the save file dialog to choose the save location
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Document (*.pdf)|*.pdf";
                saveFileDialog.Title = "Save PDF Document";
                saveFileDialog.FileName = "document.pdf"; // Default file name

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    // Save the Word document as a PDF
                    doc.SaveAs2(filePath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);

                    // Show success message
                    MessageBox.Show("The content has been exported to PDF successfully!");
                }

                // Close the Word document and release resources
                if (doc != null)
                {
                    doc.Close(SaveChanges: false);
                    doc = null;
                }

                if (wordApp != null)
                {
                    wordApp.Quit();
                    wordApp = null;
                }
            }
            else
            {
                MessageBox.Show("Please select a table from the list.");
            }
        }

        private string RetrieveRichTextSignatureForTextBoxSign(string textBoxSign)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");
            string connectionString = "Data Source=" + dbPath + ";Version=3;";

            string richTextSign = "";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string selectQuery = "SELECT richTextSign FROM signatures WHERE textBoxSign = @textBoxSign;";

                using (var command = new SQLiteCommand(selectQuery, conn))
                {
                    command.Parameters.AddWithValue("@textBoxSign", textBoxSign);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            richTextSign = reader["richTextSign"].ToString();
                        }
                    }
                }
            }

            return richTextSign;
        }



        private void AddHeader(Document doc, string headerText)
        {
            if (string.IsNullOrEmpty(headerText))
            {
                // If headerText is null or empty, we assign a default value.
                // Or you can simply return from here, if you don't want to add anything.
                headerText = "Default Header";
            }

            foreach (Section section in doc.Sections)
            {
                HeadersFooters headers = section.Headers;
                foreach (HeaderFooter header in headers)
                {
                    int retries = 5;
                    while (retries > 0)
                    {
                        try
                        {
                            Debug.WriteLine($"Header Text: {headerText}");  // Debug line
                            Clipboard.SetText(headerText, TextDataFormat.Rtf);
                            header.Range.Paste();
                            retries = 0;
                        }
                        catch (System.Runtime.InteropServices.COMException)
                        {
                            if (retries > 0)
                            {
                                retries--;
                                System.Threading.Thread.Sleep(500); // wait 500 milliseconds before retrying
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
        }






        private void AddFooter(Document doc, string footerText)
        {
            if (footerText == null)
            {
                throw new ArgumentNullException(nameof(footerText), "footerText cannot be null");
            }

            foreach (Section section in doc.Sections)
            {
                HeadersFooters footers = section.Footers;
                foreach (HeaderFooter footer in footers)
                {
                    int retries = 5;
                    while (retries > 0)
                    {
                        try
                        {
                            Debug.WriteLine($"Footer Text: {footerText}");  // Debug line
                            Clipboard.SetText(footerText, TextDataFormat.Rtf);
                            footer.Range.Paste();
                            retries = 0;
                        }
                        catch (System.Runtime.InteropServices.COMException)
                        {
                            if (retries > 0)
                            {
                                retries--;
                                System.Threading.Thread.Sleep(500); // wait 500 milliseconds before retrying
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
        }




        private string RetrieveHeaderFooterContent(string type)
        {
            string content = "";
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = $"SELECT Content FROM header_footer WHERE Type = @Type;";

                    using (var command = new SQLiteCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@Type", type);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                content = reader.GetString(0);
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
            return content;
        }


        private string RetrieveSpecificKeyValueFromTable(string tableName, string specificKey)
        {
            string keyValue = "";
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = $"SELECT KeyValue FROM {tableName} WHERE KeyName = @KeyName;";

                    using (var command = new SQLiteCommand(sql, conn))
                    {
                        // Use a parameter to avoid SQL Injection
                        command.Parameters.AddWithValue("@KeyName", specificKey);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                keyValue = reader.GetString(0);
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
            return keyValue;
        }



        private List<string> RetrieveContentFromTable(string tableName)
        {
            List<string> contentList = new List<string>();
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string selectSql = $"SELECT p.content FROM paragraphs p INNER JOIN \"{tableName}\" t ON p.id = t.paragraph_id ORDER BY t.itemOrder";

                    Debug.WriteLine(selectSql);

                    using (var command = new SQLiteCommand(selectSql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string content = reader.GetString(0);
                                contentList.Add(content);
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
            return contentList;
        }








        private void panelTemp_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void signature_FormBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}