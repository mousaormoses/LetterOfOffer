using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LetterOfOffer
{

    public partial class AddList : Form
    {
        public string SelectedTableName { get; set; }



        /*        public AddList()
                {
                    InitializeComponent();

                    // Create a dictionary to associate the preview text with the corresponding database values
                    Dictionary<string, Tuple<int, string>> previewTextToParagraphMap = new Dictionary<string, Tuple<int, string>>();

                    try
                    {
                        // Load data from the SQLite database
                        DatabasePath databasePath = new DatabasePath();
string dbPath = databasePath.MyPathDes();

                        // Ensure the directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                        // Use the dbPath variable when creating your SQLite connection
                        string connectionString = "Data Source=" + dbPath + ";Version=3;";

                        using (var connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "SELECT * FROM paragraphs";
                            using (var command = new SQLiteCommand(sql, connection))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        if (reader["content"] != DBNull.Value)
                                        {
                                            int id = Convert.ToInt32(reader["id"]);
                                            string content = reader["content"].ToString();

                                            // Convert RTF content to plain text
                                            string plainContent = string.Empty;
                                            using (RichTextBox rtb = new RichTextBox())
                                            {
                                                try
                                                {
                                                    rtb.Rtf = content;
                                                    plainContent = rtb.Text;
                                                }
                                                catch (ArgumentException)
                                                {
                                                    // If the content is not valid RTF, then just treat it as plain text
                                                    plainContent = content;
                                                }
                                            }

                                            previewTextToParagraphMap[$"Paragraph {previewTextToParagraphMap.Count + 1}"] = new Tuple<int, string>(id, plainContent); // Mapping the actual database value for the preview text
                                            addFormTemplate.Items.Add($"{id} - {plainContent}", false); // Adding items to the list box, default unchecked
                                        }
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


                    // Handle saveButton Click event
                    saveButton.Click += (s, args) =>
                    {
                        try
                        {
                            DatabasePath databasePath = new DatabasePath();
string dbPath = databasePath.MyPathDes();

                            // Ensure the directory exists
                            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                            // Use the dbPath variable when creating your SQLite connection
                            string connectionString = "Data Source=" + dbPath + ";Version=3;";

                            using (var connection = new SQLiteConnection(connectionString))
                            {
                                connection.Open();

                                // Iterate through each item in the list box
                                for (int i = 0; i < addFormTemplate.Items.Count; i++)
                                {
                                    // Check if the item is checked
                                    if (addFormTemplate.GetItemChecked(i))
                                    {
                                        // Get the item's preview text
                                        string previewText = addFormTemplate.Items[i].ToString();

                                        // Attempt to parse the ID from the preview text
                                        if (int.TryParse(previewText.Split(' ')[0], out int id))
                                        {
                                            // If successful, retrieve the corresponding database value from the dictionary using the ID
                                            var matchingEntry = previewTextToParagraphMap.First(entry => entry.Value.Item1 == id);

                                            // Insert the paragraph ID and itemOrder into the selected table
                                            string sql = $"INSERT INTO \"{SelectedTableName}\" (paragraph_id, itemOrder) VALUES (@ParagraphId, @ItemOrder)";
                                            using (var command = new SQLiteCommand(sql, connection))
                                            {
                                                command.Parameters.AddWithValue("@ParagraphId", matchingEntry.Value.Item1);
                                                command.Parameters.AddWithValue("@ItemOrder", matchingEntry.Value.Item1);  // itemOrder is now the same as ParagraphId
                                                command.ExecuteNonQuery();
                                            }
                                        }
                                        else
                                        {
                                            // If parsing failed, show an error message (or handle the error appropriately)
                                            MessageBox.Show("There was an error parsing the ID from the preview text. Please check the preview text format.");
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

                        MessageBox.Show("Checked items were saved to the database.");
                    };
                }*/


        public AddList()
        {
            InitializeComponent();

            // Create a dictionary to associate the preview text with the corresponding database values
            Dictionary<string, Tuple<int, string>> previewTextToParagraphMap = new Dictionary<string, Tuple<int, string>>();

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.ValueType = typeof(bool);
            checkBoxColumn.Name = "checkBoxColumn";
            checkBoxColumn.HeaderText = "Select";
            dataGridView1.Columns.Insert(0, checkBoxColumn);


            try
            {
                // Load data from the SQLite database
                // Load the settings
                AppSettings settings = AppSettings.Load();

                // Use the DbPath from the settings
                string dbPath = settings.DbPath;


                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM paragraphs";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["content"] != DBNull.Value)
                                {
                                    int id = Convert.ToInt32(reader["id"]);
                                    string content = reader["content"].ToString();

                                    // Convert RTF content to plain text
                                    string plainContent = string.Empty;
                                    using (RichTextBox rtb = new RichTextBox())
                                    {
                                        try
                                        {
                                            rtb.Rtf = content;
                                            plainContent = rtb.Text;
                                        }
                                        catch (ArgumentException)
                                        {
                                            // If the content is not valid RTF, then just treat it as plain text
                                            plainContent = content;
                                        }
                                    }

                                    previewTextToParagraphMap[$"Paragraph {previewTextToParagraphMap.Count + 1}"] = new Tuple<int, string>(id, content); // Mapping the actual database value for the preview text
                                    dataGridView1.Rows.Add(false, $"{id} - {plainContent}"); // Adding items to the DataGridView, default unchecked
                                }
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

            // Handle saveButton Click event
            saveButton.Click += (s, args) =>
            {
                try
                {
                    // Load the settings
                    AppSettings settings = AppSettings.Load();

                    // Use the DbPath from the settings
                    string dbPath = settings.DbPath;

                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                    // Use the dbPath variable when creating your SQLite connection
                    string connectionString = "Data Source=" + dbPath + ";Version=3;";
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        // Iterate through each row in the DataGridView
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            // Avoid operating on empty rows (which are null)
                            if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                            {
                                // Check if the checkbox is checked
                                bool isChecked = (bool)row.Cells[0].Value;
                                if (isChecked)
                                {
                                    // Get the paragraph ID from the row's data
                                    int id = Convert.ToInt32(row.Cells[1].Value.ToString().Split('-')[0].Trim());

                                    // Insert the paragraph ID and itemOrder into the selected table
                                    string sql = $"INSERT INTO \"{SelectedTableName}\" (paragraph_id, itemOrder) VALUES (@ParagraphId, @ItemOrder)";
                                    using (var command = new SQLiteCommand(sql, connection))
                                    {
                                        command.Parameters.AddWithValue("@ParagraphId", id);
                                        command.Parameters.AddWithValue("@ItemOrder", id); // itemOrder is now the same as ParagraphId
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }


                    MessageBox.Show("Checked items were saved to the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    // Log or display the exception as needed
                    Console.WriteLine(ex.ToString());
                }
            };
        }

        private void AddList_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {

        }
    }
}
