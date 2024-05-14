using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DatabaseConnectionTask.Model;

namespace DatabaseConnectionTask
{
    public partial class TableList : Form
    {
        private SqlConnection connection;
        private string connectionString;
        private ListView listViewTables;
        private Button submitButton;

        public TableList(List<string> tableNames, string Dbname, string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            InitializeListView(tableNames, Dbname);
            submitButton.Click += SubmitButton_Click;
        }

        private void InitializeListView(List<string> tableNames, string Dbname)
        {
            // Create and style the title label
            Label titleLabel = new Label();
            titleLabel.Text = "Tables List of " + Dbname;
            titleLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 50;
            this.Controls.Add(titleLabel);

            // Create and style the list view
            listViewTables = new ListView();
            listViewTables.View = View.Details;
            listViewTables.CheckBoxes = true;
            listViewTables.FullRowSelect = true;
            listViewTables.Size = new Size(450, 250);
            listViewTables.Location = new Point((this.ClientSize.Width - listViewTables.Width) / 2, (this.ClientSize.Height - listViewTables.Height) / 2);
            listViewTables.Anchor = AnchorStyles.None;
            listViewTables.Columns.Add("Tables", 300, HorizontalAlignment.Left);
            listViewTables.Columns.Add("Action", 100, HorizontalAlignment.Center);

            foreach (var tableName in tableNames)
            {
                ListViewItem item = new ListViewItem(tableName);
                item.SubItems.Add("Details");
                listViewTables.Items.Add(item);
            }
            listViewTables.MouseClick += ListViewTables_MouseClick;

            this.Controls.Add(listViewTables);

            // Create and style the submit button
            submitButton = new Button();
            submitButton.Text = "Submit";
            submitButton.Font = new Font("Arial", 12, FontStyle.Regular);
            submitButton.BackColor = Color.GhostWhite; // Setting a different background color
            submitButton.ForeColor = Color.Black; // Setting text color
            submitButton.Width = 100; // Setting a small width size
            submitButton.Height = 35; // Setting a specific height
            submitButton.Anchor = AnchorStyles.Bottom; // Anchor to bottom
            submitButton.Location = new Point((this.ClientSize.Width - submitButton.Width) / 2, this.ClientSize.Height - submitButton.Height - 20); // Adjust location
            this.Controls.Add(submitButton);

            // Adjust form layout to center contents vertically
            int totalHeight = titleLabel.Height + listViewTables.Height + submitButton.Height + 20;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            int verticalPosition = (screenHeight - totalHeight) / 2;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, verticalPosition);
        }

        private void ListViewTables_MouseClick(object sender, MouseEventArgs e)
        {
            var hitTest = listViewTables.HitTest(e.Location);
            if (hitTest.SubItem != null && hitTest.SubItem.Text == "Details")
            {
                string tableName = hitTest.Item.Text;
                ShowTableDetails(tableName);
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // Get checked items and their values
            List<string> checkedItems = new List<string>();
            foreach (ListViewItem item in listViewTables.CheckedItems)
            {
                checkedItems.Add(item.Text);
            }

            // Do something with the checked items
            TableDetails(checkedItems, connectionString);
            MessageBox.Show("Table data submitted");
            this.Close();
        }

        private void ShowTableDetails(string tableName)
        {
            connection = new SqlConnection(connectionString);
            connection.Open(); // Open the connection

            string query = $"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}';";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            TableDetail tableDetail = new TableDetail();
            if (reader.HasRows)
            {
                List<TableView> tables = new List<TableView>();

                while (reader.Read())
                {
                    TableView tableView = new TableView();
                    tableView.ColumnName = reader["COLUMN_NAME"].ToString();
                    tableView.DataType = reader["DATA_TYPE"].ToString();
                    tableView.MaxLength = reader["CHARACTER_MAXIMUM_LENGTH"].ToString();
                    tableView.Nullable = reader["IS_NULLABLE"].ToString();

                    tables.Add(tableView);
                }
                tableDetail.TableName = tableName.ToString();
                tableDetail.tableViews = tables;
            }
            else
            {
                MessageBox.Show("No tables found in the database.");
            }
            reader.Close();
            TableDetailModel tableDetailsForm = new TableDetailModel(tableDetail);
            tableDetailsForm.Show();
        }

        private void TableDetails(List<string> checkedItems, string connectionString)
        {
            List<TableDetail> tableDetailsList = new List<TableDetail>();
            try
            {
                foreach (string item in checkedItems)
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open(); // Open the connection

                    string query = $"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{item}';";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    TableDetail tableDetail = new TableDetail();
                    if (reader.HasRows)
                    {
                        List<TableView> tables = new List<TableView>();

                        while (reader.Read())
                        {
                            TableView tableView = new TableView();
                            tableView.ColumnName = reader["COLUMN_NAME"].ToString();
                            tableView.DataType = reader["DATA_TYPE"].ToString();
                            tableView.MaxLength = reader["CHARACTER_MAXIMUM_LENGTH"].ToString();
                            tableView.Nullable = reader["IS_NULLABLE"].ToString();

                            tables.Add(tableView);
                        }
                        tableDetail.TableName = item.ToString();
                        tableDetail.tableViews = tables;
                    }
                    else
                    {
                        MessageBox.Show("No tables found in the database.");
                    }
                    reader.Close();
                    tableDetailsList.Add(tableDetail);
                }
                TableDetails tableDetailsForm = new TableDetails(tableDetailsList);
                tableDetailsForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while filling checkbox list: " + ex.Message);
            }
        }
    }
}
