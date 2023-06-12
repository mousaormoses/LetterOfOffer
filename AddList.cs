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

        private bool isMaximized = false; // Track the maximized state
        private Size originalSize; // Store the original size of panel1

        private void AddList_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                if (!isMaximized)
                {
                    // Store the original size of panel1
                    originalSize = panel1.Size;

                    // Calculate the new size for panel1 and dataGridView1 based on the available form size
                    int newWidth = ClientSize.Width;
                    int newHeight = ClientSize.Height;
                    int panel1Height = (int)(newHeight * 0.9);
                    int dataGridView1Height = panel1Height - panel1.Padding.Top - panel1.Padding.Bottom;

                    // Adjust the size and position of panel1 and dataGridView1
                    panel1.Size = new Size(newWidth, panel1Height);
                    dataGridView1.Size = new Size(newWidth - dataGridView1.Margin.Left - dataGridView1.Margin.Right, dataGridView1Height);
                    dataGridView1.Location = new Point(dataGridView1.Margin.Left, dataGridView1.Margin.Top);

                    isMaximized = true;
                }
            }
            else if (WindowState == FormWindowState.Normal)
            {
                if (isMaximized)
                {
                    // Restore the original size and position of panel1 and dataGridView1
                    panel1.Size = originalSize;
                    dataGridView1.Size = new Size(originalSize.Width - dataGridView1.Margin.Left - dataGridView1.Margin.Right, originalSize.Height - dataGridView1.Margin.Top - dataGridView1.Margin.Bottom);
                    dataGridView1.Location = new Point(dataGridView1.Margin.Left, dataGridView1.Margin.Top);

                    isMaximized = false;
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if it's a DataGridViewTextBoxCell in the Content column
            if (e.ColumnIndex == dataGridView1.Columns["Content"].Index && e.RowIndex >= 0)
            {
                DataGridViewTextBoxCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewTextBoxCell;
                if (cell != null)
                {
                    // Get the content of the cell
                    string content = cell.Value?.ToString();

                    // Calculate the desired row height based on the number of lines
                    int numLines = content.Split('\n').Length;
                    int lineHeight = TextRenderer.MeasureText("Sample", dataGridView1.Font).Height;
                    int desiredHeight = numLines * lineHeight + dataGridView1.RowTemplate.DefaultCellStyle.Padding.Vertical;

                    // Set the row height
                    if (dataGridView1.Rows[e.RowIndex].Height != desiredHeight)
                    {
                        dataGridView1.Rows[e.RowIndex].Height = desiredHeight +10;
                    }
                }
            }
        }
    }
}
