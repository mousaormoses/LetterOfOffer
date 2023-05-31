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
using System.Xml;

namespace LetterOfOffer_18.Tabs
{
    public partial class TemplateView : UserControl
    {
        private List<int> selectedParagraphs; // List to store IDs of selected paragraphs
        private string selectedTableName; // Variable to store the selected template table name

        public TemplateView()
        {
            InitializeComponent();
            selectedParagraphs = new List<int>();
        }

        private void RefreshTableList()
        {
            // Clear all existing controls
            panelTemp.Controls.Clear();

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
                    string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name LIKE 'template_%';";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {

                            // Get the width of a vertical scrollbar
                            int scrollbarWidth = System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;



                            int yPos = 19; // Initial vertical position for controls

                            // Set initial positions and dimensions for the buttons
                            int buttonWidth = 139;
                            int buttonHeight = 39;
                            int buttonTop = 10;

                            // Similarly, adjust the left positions of your buttons to fit within the panel
                            int addButtonLeft = panelTemp.Width - buttonWidth - scrollbarWidth - 10;
                            int editButtonLeft = addButtonLeft - buttonWidth - 10;
                            int deleteButtonLeft = editButtonLeft - buttonWidth - 10;

                            while (reader.Read())
                            {
                                string tableName = reader["name"].ToString();

                                // Create a label for the table name
                                //var nameLabel = new Label() { Text = tableName, Width = 200, Top = yPos, Left = 10 };


                                // Remove the "template_" prefix from the table name
                                string displayTableName = tableName.Substring("template_".Length);
                                // Create a label for the table name
                                var nameLabel = new Label() { Text = displayTableName, Width = 180, Top = yPos, Left = 10 };
                                nameLabel.TextAlign = ContentAlignment.MiddleLeft;

                                // Set the button's top position to be the same as the label
                                buttonTop = yPos;

                                // Create the Edit button
                                var editButton = new Button()
                                {
                                    Text = "Edit",
                                    Left = editButtonLeft,
                                    Top = buttonTop,
                                    Width = buttonWidth,
                                    Height = buttonHeight,
                                    FlatStyle = FlatStyle.Flat // make button flat
                                };
                                editButton.Click += (s, args) =>
                                {


                                    // Store the selected table name
                                    selectedTableName = tableName;

                                    // Open the new form for editing
                                    FormEdit editForm = new FormEdit();
                                    editForm.Text = "Edit Order List";
                                    editForm.Size = this.Size;

                                    // Reference the ListView in FormEdit
                                    ListView listView = editForm.listViewEdit;
                                    listView.Dock = DockStyle.Fill;
                                    listView.AllowDrop = true;
                                    listView.View = View.Details; // Set view to Details for column display

                                    // Add columns to ListView
                                    listView.Columns.Add("ID", 50);
                                    listView.Columns.Add("Content", 200);

                                    // Create a Context Menu Strip for deleting items
                                    ContextMenuStrip contextMenu = new ContextMenuStrip();
                                    ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete");
                                    contextMenu.Items.Add(deleteItem);
                                    listView.ContextMenuStrip = contextMenu;

                                    // Show the context menu when the user right-clicks an item
                                    listView.MouseClick += (s3, args3) =>
                                    {
                                        if (args3.Button == MouseButtons.Right)
                                        {
                                            // Make sure an item is selected
                                            if (listView.FocusedItem.Bounds.Contains(args3.Location))
                                            {
                                                contextMenu.Show(Cursor.Position);
                                            }
                                        }
                                    };
                                    // Delete the selected item when the "Delete" context menu item is clicked
                                    deleteItem.Click += (s4, args4) =>
                                    {
                                        // Make sure an item is selected
                                        if (listView.SelectedItems.Count > 0)
                                        {
                                            ListViewItem selectedItem = listView.SelectedItems[0];

                                            // Remove the item from the ListView
                                            listView.Items.Remove(selectedItem);

                                            try
                                            {
                                                // Remove the item from the database
                                                using (var conn = new SQLiteConnection(connectionString))
                                                {
                                                    conn.Open();
                                                    string sql3 = $"DELETE FROM \"{selectedTableName}\" WHERE ID = @ItemID";
                                                    using (var command3 = new SQLiteCommand(sql3, conn))
                                                    {
                                                        command3.Parameters.AddWithValue("@ItemID", selectedItem.Text); // assuming the ID of each item is the text of the first subitem
                                                        command3.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                // Log or display the exception as needed
                                                Console.WriteLine(ex.ToString());
                                            }
                                        }
                                    };

                                    try
                                    {
                                        // Retrieve items from the SQLite database
                                        using (var conn = new SQLiteConnection(connectionString))
                                        {
                                            conn.Open();
                                            string sql2 = $"SELECT t.ID, p.content FROM \"{selectedTableName}\" t INNER JOIN paragraphs p ON t.paragraph_id = p.id ORDER BY t.itemOrder";
                                            using (var command2 = new SQLiteCommand(sql2, conn))
                                            {
                                                using (var reader2 = command2.ExecuteReader())
                                                {
                                                    while (reader2.Read())
                                                    {
                                                        // Get ID and Content
                                                        string id = reader2["ID"].ToString();
                                                        string rtfContent = reader2["content"].ToString();

                                                        // Convert RTF content to plain text
                                                        string plainContent = string.Empty;
                                                        using (RichTextBox rtb = new RichTextBox())
                                                        {
                                                            try
                                                            {
                                                                rtb.Rtf = rtfContent;
                                                                plainContent = rtb.Text;
                                                            }
                                                            catch (ArgumentException)
                                                            {
                                                                // If the originalContent is not valid RTF,
                                                                // then just treat it as plain text
                                                                plainContent = rtfContent;
                                                            }
                                                        }

                                                        // Add ListViewItem with ID and Content
                                                        ListViewItem listViewItem = new ListViewItem(new[] { id, plainContent });
                                                        listView.Items.Add(listViewItem);
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

                                    // Implement drag-and-drop feature
                                    ListViewItem draggedItem = null;

                                    listView.ItemDrag += (s2, args2) =>
                                    {
                                        draggedItem = (ListViewItem)args2.Item;
                                        listView.DoDragDrop(draggedItem, DragDropEffects.Move);
                                    };

                                    listView.DragEnter += (s2, args2) =>
                                    {
                                        args2.Effect = DragDropEffects.Move;
                                    };

                                    listView.DragDrop += (s2, args2) =>
                                    {
                                        // Find the bounds of the item that is currently under the mouse pointer.
                                        Point cp = listView.PointToClient(new Point(args2.X, args2.Y));
                                        ListViewItem hoverItem = listView.GetItemAt(cp.X, cp.Y);

                                        // Find the index of the item under the mouse pointer.
                                        int hoverIndex = hoverItem != null ? hoverItem.Index : listView.Items.Count - 1; // Use count - 1 when no item is found

                                        // Insert the dragged item at this position.
                                        if (draggedItem != null) // Ensure draggedItem is not null
                                        {
                                            listView.Items.Remove(draggedItem);
                                            if (hoverIndex >= listView.Items.Count) // Check if hoverIndex is valid
                                            {
                                                listView.Items.Add(draggedItem);
                                            }
                                            else
                                            {
                                                listView.Items.Insert(hoverIndex, draggedItem);
                                            }
                                        }

                                        try
                                        {
                                            // Update the itemOrder in the SQLite database
                                            using (var conn = new SQLiteConnection(connectionString))
                                            {
                                                conn.Open();
                                                for (int i = 0; i < listView.Items.Count; i++)
                                                {
                                                    string sql2 = $"UPDATE \"{selectedTableName}\" SET itemOrder = @ItemOrder WHERE ID = @ItemID";
                                                    using (var command2 = new SQLiteCommand(sql2, conn))
                                                    {
                                                        command2.Parameters.AddWithValue("@ItemOrder", i);
                                                        command2.Parameters.AddWithValue("@ItemID", listView.Items[i].Text); // assuming the ID of each item is stored in Tag
                                                        command2.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // Log or display the exception as needed
                                            Console.WriteLine(ex.ToString());
                                        }
                                    };


                                    editForm.ShowDialog();
                                };



                                // Create the Delete button
                                var deleteButton = new Button()
                                {
                                    Text = "Delete",
                                    Left = deleteButtonLeft,
                                    Top = buttonTop,
                                    Width = buttonWidth,
                                    Height = buttonHeight,
                                    FlatStyle = FlatStyle.Flat // make button flat
                                };

                                deleteButton.Click += (s, args) =>
                                {
                                    // Ask for confirmation before deleting
                                    DialogResult result = MessageBox.Show("Are you sure you want to delete this table?", "Confirm Delete", MessageBoxButtons.YesNo);

                                    if (result == DialogResult.Yes)
                                    {
                                        try
                                        {
                                            // Delete the table from the database
                                            using (var conn2 = new SQLiteConnection(connectionString))
                                            {
                                                conn2.Open();
                                                string sql2 = $"DROP TABLE \"{tableName}\"";
                                                using (var command2 = new SQLiteCommand(sql2, conn2))
                                                {
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
                                        panelTemp.Controls.Remove(nameLabel);
                                        panelTemp.Controls.Remove(editButton);
                                        panelTemp.Controls.Remove(deleteButton);

                                        // Refresh the list
                                        RefreshTableList();
                                    }
                                };



                                // Create the Add button
                                var addButton = new Button()
                                {
                                    Text = "Add",
                                    Left = addButtonLeft,
                                    Top = buttonTop,
                                    Width = buttonWidth,
                                    Height = buttonHeight,
                                    FlatStyle = FlatStyle.Flat // make button flat
                                };

                                addButton.Click += (s, args) =>
                                {
                                    // Open the Add Form
                                    AddList addForm = new AddList();
                                    addForm.SelectedTableName = tableName; // Set selected table name
                                    addForm.ShowDialog();
                                };


                                // Add controls to the panel
                                panelTemp.Controls.Add(nameLabel);
                                panelTemp.Controls.Add(editButton);
                                panelTemp.Controls.Add(deleteButton);
                                panelTemp.Controls.Add(addButton);

                                yPos += 50; // Increment vertical position for next controls. Make sure to provide enough space for your buttons
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

        private void TemplateView_Load(object sender, EventArgs e)
        {
            RefreshTableList();
        }

        private void panelTemp_Paint(object sender, PaintEventArgs e)
        {

        }
        private void newTemp_Click(object sender, EventArgs e)
        {
            // Create a new form as a dialog
            Form prompt = new Form();
            prompt.Width = 300;
            prompt.Height = 150;
            prompt.Text = "Enter new template name";
            prompt.FormBorderStyle = FormBorderStyle.FixedSingle; // Disable resizing
            prompt.MaximizeBox = false; // Disable maximize button
            prompt.MinimizeBox = false; // Disable minimize button
            prompt.ShowIcon = false; // Hide the form icon

            // Create a TextBox on the form
            TextBox tableNameBox = new TextBox() { Left = 20, Top = 20, Width = 250 };
            prompt.Controls.Add(tableNameBox);

            // Create a Button on the form
            Button confirmationButton = new Button()
            {
                Text = "Save",
                Left = 200,
                Top = 80,
                Width = 70,
                Height = 30,
                BackColor = Color.Black,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            confirmationButton.Click += (s, er) =>
            {
                // Validate that the table name is not empty
                if (!string.IsNullOrEmpty(tableNameBox.Text))
                {
                    try
                    {
                        // Add new table to the SQLite database
                        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApplication", "MyDatabase.sqlite");

                        // Ensure the directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

                        // Use the dbPath variable when creating your SQLite connection
                        string connectionString = "Data Source=" + dbPath + ";Version=3;";

                        using (var connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();

                            // Check if table already exists
                            string checkSql = $"SELECT name FROM sqlite_master WHERE type='table' AND name='\"template_{tableNameBox.Text}\"';";
                            using (var checkCommand = new SQLiteCommand(checkSql, connection))
                            {
                                var result = checkCommand.ExecuteScalar();

                                // If table doesn't exist, create it
                                if (result == null)
                                {
                                    string createSql = $"CREATE TABLE IF NOT EXISTS \"template_{tableNameBox.Text}\" (ID INTEGER PRIMARY KEY AUTOINCREMENT, paragraph_id INTEGER, itemOrder INTEGER)";
                                    using (var createCommand = new SQLiteCommand(createSql, connection))
                                    {
                                        createCommand.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"The table \"template_{tableNameBox.Text}\" already exists.");
                                    return; // Exit the method
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or display the exception as needed
                        Console.WriteLine(ex.ToString());
                    }

                    // Refresh the table list
                    RefreshTableList();

                    // Close the dialog
                    prompt.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a table name.");
                }

            };

            // Calculate the form's height based on the position of the save button
            prompt.Height = confirmationButton.Bottom + 50;
            prompt.Controls.Add(confirmationButton);
            prompt.StartPosition = FormStartPosition.CenterScreen; // Set the start position to center on the screen
            prompt.ShowDialog();
        }


        private void saveNewTemp_Click(object sender, EventArgs e)
        {


            // Get the selected template table name
            foreach (Control control in panelTemp.Controls)
            {
                if (control is Label label)
                {
                    if (label.ForeColor == Color.Red)
                    {
                        selectedTableName = label.Text;
                        break;
                    }
                }
            }

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
                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (int paragraphID in selectedParagraphs)
                        {
                            string sql = $"INSERT INTO \"{selectedTableName}\" (content) SELECT content FROM paragraphs WHERE ID = @ParagraphID";
                            using (var command = new SQLiteCommand(sql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@ParagraphID", paragraphID);
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception as needed
                Console.WriteLine(ex.ToString());
            }

            // Clear the selected paragraphs list
            selectedParagraphs.Clear();

            // Refresh the table list
            RefreshTableList();

            MessageBox.Show("Selected paragraphs have been saved to the template.");
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    public class MyListView : ListView
    {
        public MyListView()
        {
            // This is necessary to manually draw items
            this.OwnerDraw = true;
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true; // Draw the item as usual
            using (var pen = new Pen(Color.Black))
            {
                // Draw a line at the bottom of the item
                e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            }
        }
    }

}
