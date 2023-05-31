using Microsoft.Office.Interop.Word;
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

namespace LetterOfOffer_18.Tabs
{
    public partial class ParagraphView : UserControl
    {

        public ParagraphView()
        {
            InitializeComponent();
            buttonAdd.Click += ButtonAdd_Click;

            // Create the database and table on initialization of the control
            CreateDatabaseAndTable();
        }

        private void CreateDatabaseAndTable()
        {
            if (!File.Exists("MyDatabase.sqlite"))
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string sql = "CREATE TABLE IF NOT EXISTS paragraphs (ID INTEGER PRIMARY KEY AUTOINCREMENT, content TEXT)";

                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void newRichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void newRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (Clipboard.ContainsData(DataFormats.Rtf))
                {
                    var richTextBox = sender as RichTextBox;
                    if (richTextBox != null)
                    {
                        richTextBox.SelectedRtf = Clipboard.GetData(DataFormats.Rtf).ToString();
                        e.Handled = true;
                    }
                }
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            // Create new RichTextBox and Delete button
            var newRichTextBox = new RichTextBox() { Width = 520, Height = 160, DetectUrls = true, BorderStyle = BorderStyle.None };
            var deleteButton = new Button() {
                Image = Properties.Resources.icons8_delete_15__1_,
                BackgroundImageLayout = ImageLayout.Zoom,
                Left = newRichTextBox.Width + 10, Width = 35 };

            newRichTextBox.LinkClicked += new LinkClickedEventHandler(newRichTextBox_LinkClicked);
            newRichTextBox.KeyDown += new KeyEventHandler(newRichTextBox_KeyDown);

            // Set location for the new controls
            if (panelParagraph.Controls.Count > 0)
            {
                newRichTextBox.Top = panelParagraph.Controls[panelParagraph.Controls.Count - 1].Bottom + 140;
                deleteButton.Top = newRichTextBox.Top;
            }

            try
            {
                // Add paragraph to the SQLite database
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO paragraphs (content) VALUES (@Content)";

                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Content", newRichTextBox.Rtf);
                        command.ExecuteNonQuery();
                        newRichTextBox.Tag = connection.LastInsertRowId;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
            }

            // Now that we have the Tag value, create the ID label
            var idLabel = new Label() { Text = newRichTextBox.Tag.ToString(), Width = 50, Height = 20, Left = newRichTextBox.Left - 60, Top = newRichTextBox.Top };

            // Add Click event to delete button
            deleteButton.Click += (s, args) =>
            {
                try
                {
                    // Remove from the database
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                    // Use the dbPath variable when creating your SQLite connection
                    string connectionString = "Data Source=" + dbPath + ";Version=3;";

                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string sql = $"DELETE FROM paragraphs WHERE ID = @ID";
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@ID", newRichTextBox.Tag);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or display the exception as needed
                    Console.WriteLine(ex.ToString());
                }

                // Remove controls
                panelParagraph.Controls.Remove(newRichTextBox);
                panelParagraph.Controls.Remove(deleteButton);
                panelParagraph.Controls.Remove(idLabel);
            };

            // Add new RichTextBox, ID label and Delete button to the panel
            panelParagraph.Controls.Add(newRichTextBox);
            panelParagraph.Controls.Add(idLabel);
            panelParagraph.Controls.Add(deleteButton);
        }

        private void ParagraphView_Load(object sender, EventArgs e)
        {
            // Load the paragraphs from the SQLite database

            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create the paragraphs table if it doesn't exist
                    string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS paragraphs (
                    ID INTEGER PRIMARY KEY,
                    content TEXT
                    )";
                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string sql = "SELECT * FROM paragraphs";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create the idLabel first
                                var idLabel = new System.Windows.Forms.Label() { Text = reader["ID"].ToString(), Width = 0, Height = 20 };

                                // Create a new RichTextBox and Delete button
                                var newRichTextBox = new RichTextBox() { Width = 520, Height = 160, BorderStyle = BorderStyle.None };
                                var deleteButton = new Button()
                                {
                                    Image = Properties.Resources.icons8_delete_15__1_,
                                    BackgroundImageLayout = ImageLayout.Zoom,
                                    Left = newRichTextBox.Width + 10,
                                    Width = 35
                                };
                                if (reader["content"] != DBNull.Value)
                                {
                                    newRichTextBox.Rtf = (string)reader["content"];
                                }

                                // Set the location for the new controls
                                if (panelParagraph.Controls.Count > 0)
                                {
                                    idLabel.Top = panelParagraph.Controls[panelParagraph.Controls.Count - 1].Bottom + 140;
                                }

                                // Adjust the Left and Top properties of newRichTextBox and deleteButton based on idLabel
                                newRichTextBox.Left = idLabel.Right;
                                newRichTextBox.Top = idLabel.Top;
                                deleteButton.Left = newRichTextBox.Right + 10;
                                deleteButton.Top = newRichTextBox.Top;

                                // Set the Tag of the RichTextBox to the ID from the database
                                newRichTextBox.Tag = reader["ID"];

                                // Add the Click event to the Delete button
                                deleteButton.Click += (s, args) =>
                                {
                                    try
                                    {
                                        // Remove from the database
                                        using (var connection2 = new SQLiteConnection(connectionString))
                                        {
                                            connection2.Open();
                                            string sql2 = $"DELETE FROM paragraphs WHERE ID = @ID";
                                            using (var command2 = new SQLiteCommand(sql2, connection2))
                                            {
                                                command2.Parameters.AddWithValue("@ID", newRichTextBox.Tag);
                                                command2.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // Log or display the exception as needed
                                        Console.WriteLine(ex.ToString());
                                    }

                                    // Remove controls
                                    panelParagraph.Controls.Remove(newRichTextBox);
                                    panelParagraph.Controls.Remove(deleteButton);
                                    panelParagraph.Controls.Remove(idLabel);
                                };

                                // Add the new RichTextBox, ID label, and Delete button to the panel
                                //panelParagraph.Controls.Add(idLabel);
                                panelParagraph.Controls.Add(newRichTextBox);
                                panelParagraph.Controls.Add(deleteButton);
                            }
                        }

                        // Add context menu strip to each RichTextBox control
                        foreach (Control control in panelParagraph.Controls)
                        {
                            if (control is RichTextBox richTextBox)
                            {
                                richTextBox.ContextMenuStrip = new ContextMenuStrip();
                                ToolStripMenuItem pasteMenuItem = new ToolStripMenuItem("Paste");
                                pasteMenuItem.Click += (s, args) =>
                                {
                                    richTextBox.Paste();
                                };
                                richTextBox.ContextMenuStrip.Items.Add(pasteMenuItem);

                                ToolStripMenuItem selectAllMenuItem = new ToolStripMenuItem("Select All");
                                selectAllMenuItem.Click += (s, args) =>
                                {
                                    richTextBox.SelectAll();
                                };
                                richTextBox.ContextMenuStrip.Items.Add(selectAllMenuItem);
                            }
                        }

                    }
                }
            }

            catch (SQLiteException ex)
            {
                Console.WriteLine("SQLiteException: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            // Initialize ListView properties
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            // Create a context menu strip
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem copyMenuItem = new ToolStripMenuItem("Copy");
            copyMenuItem.Click += (sender2, e2) =>
            {
                ListViewItem selectedItem = listView1.FocusedItem;
                if (selectedItem != null)
                {
                    StringBuilder dataBuilder = new StringBuilder();
                    foreach (ListViewItem.ListViewSubItem subItem in selectedItem.SubItems)
                    {
                        dataBuilder.Append(subItem.Text);
                        dataBuilder.Append(" ");
                    }
                    string data = dataBuilder.ToString().Trim();

                    if (!string.IsNullOrEmpty(data))
                    {
                        Clipboard.SetText(data);
                    }
                }
            };
            contextMenuStrip.Items.Add(copyMenuItem);

            try
            {
                // Open the SQLite connection.
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                // Use the dbPath variable when creating your SQLite connection
                string connectionString = "Data Source=" + dbPath + ";Version=3;";

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Retrieve the keyvalues.
                    string sql = "SELECT keyvalue FROM keys";
                    using (var command = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // Create a new list to hold the current row's items.
                            List<string> rowItems = new List<string>();

                            // Iterate over the results.
                            while (reader.Read())
                            {
                                // Add the current item to the row items.
                                rowItems.Add(reader.GetString(0));

                                // If the number of row items exceeds the column limit, add them to the ListView and start a new row.
                                if (rowItems.Count >= listView1.Columns.Count)
                                {
                                    listView1.Items.Add(new ListViewItem(rowItems.ToArray()));
                                    rowItems.Clear();
                                }
                            }

                            // If there are any remaining items that didn't form a complete row, add them to the ListView.
                            if (rowItems.Count > 0)
                            {
                                listView1.Items.Add(new ListViewItem(rowItems.ToArray()));
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

            // Automatically adjust column widths to fit the contents.
            foreach (ColumnHeader column in listView1.Columns)
            {
                column.Width = -2;
            }

            // Handle MouseClick event to show the context menu
            listView1.MouseClick += (sender2, e2) =>
            {
                if (e2.Button == MouseButtons.Right)
                {
                    ListViewItem selectedItem = listView1.FocusedItem;
                    if (selectedItem != null)
                    {
                        contextMenuStrip.Show(listView1, e2.Location);
                    }
                }
            };
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            foreach (var control in panelParagraph.Controls.OfType<RichTextBox>())
            {
                try
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                    // Ensure the directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                    // Use the dbPath variable when creating your SQLite connection
                    string connectionString = "Data Source=" + dbPath + ";Version=3;";

                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "UPDATE paragraphs SET content = @Content WHERE ID = @ID";
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@ID", control.Tag);
                            command.Parameters.AddWithValue("@Content", control.Rtf);
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
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panelParagraph_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonAdd_Click_1(object sender, EventArgs e)
        {

        }
    }
}

