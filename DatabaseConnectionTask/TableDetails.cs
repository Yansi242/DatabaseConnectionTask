using DatabaseConnectionTask.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DatabaseConnectionTask
{
    public partial class TableDetails : Form
    {
        private SqlConnection connection;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Panel panel1;
        private List<TableDetail> tableDetails;
        private List<string> tableNames;
        private string connectionString;
        private string dbName;
        private Button submitButton;
        private Button backButton;
        private exportModel exportModel;

        public TableDetails(List<TableDetail> tableDetails, List<string> tableNames, string dbName, string connectionString,exportModel model)
        {
            InitializeComponent();
            this.tableDetails = tableDetails;
            this.tableNames = tableNames;
            this.dbName = dbName;
            this.exportModel = model;
            this.connectionString = connectionString;
            InitializeControls();
            InitializeButtons();
        }

        private void InitializeControls()
        {
            // Create and style the title label
            Label titleLabel = new Label();
            titleLabel.Text = "Details of Tables";
            titleLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 50;
            this.Controls.Add(titleLabel);

            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            panel1 = new Panel();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.Location = new Point(61, 40);
            panel1.Name = "panel1";
            panel1.Size = new Size(1040, 420);
            panel1.TabIndex = 0;
            panel1.AutoScroll = true;
            this.Controls.Add(panel1);

            // Create a panel to contain table details
            //Panel panel = new Panel();
            //panel.Dock = DockStyle.Fill;
            //panel.Height = 30;
            //this.Controls.Add(panel);

            int currentTop = titleLabel.Bottom + 10; // Start position for table names and details

            foreach (var data in tableDetails)
            {
                // Create CheckBox for table selection
                CheckBox tableCheckBox = new CheckBox();
                tableCheckBox.Text = data.tableName;
                tableCheckBox.AutoSize = true;
                tableCheckBox.Top = currentTop;
                tableCheckBox.Left = 20;
                panel1.Controls.Add(tableCheckBox);

                // Create DataGridView to display table details
                DataGridView tableDataGridView = new DataGridView();
                tableDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tableDataGridView.RowHeadersVisible = false;
                tableDataGridView.AllowUserToAddRows = false;
                tableDataGridView.AllowUserToDeleteRows = false;
                tableDataGridView.AllowUserToResizeRows = false;
                tableDataGridView.Width = 1000;
                tableDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                tableDataGridView.Location = new Point(20, currentTop + tableCheckBox.Height + 5); // Position below checkbox

                // Define columns
                tableDataGridView.Columns.Add("ColumnName", "Column Name");
                tableDataGridView.Columns.Add("DataType", "Data Type");
                tableDataGridView.Columns.Add("MaxLength", "Max Length");
                tableDataGridView.Columns.Add("Nullable", "Is Nullable");

                foreach (var row in data.tableDetail)
                {
                    tableDataGridView.Rows.Add(row.columnName, row.dataType, row.maxLength, row.nullable);
                }

                // Set DataGridView height based on the number of rows
                int dgvHeight = (data.tableDetail.Count + 1) * 25; // Header + rows
                tableDataGridView.Height = Math.Min(dgvHeight, 150); // Limit to 150 pixels max

                panel1.Controls.Add(tableDataGridView);

                currentTop += tableCheckBox.Height + 5 + tableDataGridView.Height + 10; // Adjust for next set
            }
        }

        private void InitializeButtons()
        {
            // Create and style the submit button
            submitButton = new Button();
            submitButton.Text = "Submit";
            submitButton.Font = new Font("Arial", 12, FontStyle.Regular);
            submitButton.BackColor = Color.GhostWhite;
            submitButton.ForeColor = Color.Black;
            submitButton.Width = 100;
            submitButton.Height = 35;
            submitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; // Anchor to bottom-right
            submitButton.Location = new Point(panel1.Left + panel1.Width - submitButton.Width, panel1.Bottom + 20); // Below the panel
            submitButton.Click += SubmitButton_Click;
            this.Controls.Add(submitButton);

            // Create and style the back button
            backButton = new Button();
            backButton.Text = "Back";
            backButton.Font = new Font("Arial", 12, FontStyle.Regular);
            backButton.BackColor = Color.GhostWhite;
            backButton.ForeColor = Color.Black;
            backButton.Width = 100;
            backButton.Height = 35;
            backButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; // Anchor to bottom-right
            backButton.Location = new Point(submitButton.Left - backButton.Width - 10, submitButton.Top); // Align with submit button
            this.Controls.Add(backButton);
            backButton.Click += BackButton_Click;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            List<string> selectedTables = new List<string>();


            // Iterate through the controls in panel1
            foreach (Control control in panel1.Controls)
            {
                if (control is CheckBox checkBox && checkBox.Checked)
                {
                    // If it's a CheckBox and it's checked, add its text (table name) to the list
                    selectedTables.Add(checkBox.Text);
                }
            }

            List<TableDetail> tableDetailsList = new List<TableDetail>();
            foreach (string item in selectedTables)
            {
                connection = new SqlConnection(connectionString);
                connection.Open(); // Open the connection

                string query = $"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{item}';";
                //string query = $"SELECT IU.COLUMN_NAME,IS1.DATA_TYPE  FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE IU INNER JOIN INFORMATION_SCHEMA.COLUMNS IS1 ON IU.TABLE_NAME = IS1.TABLE_NAME AND IU.COLUMN_NAME = IS1.COLUMN_NAME  WHERE IU.TABLE_NAME = '{item}' AND IU.CONSTRAINT_NAME LIKE '%PK%'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                TableDetail tableDetail = new TableDetail();
                if (reader.HasRows)
                {
                    List<TableView> tables = new List<TableView>();

                    while (reader.Read())
                    {
                        TableView tableView = new TableView();
                        tableView.tableName = item;
                        tableView.columnName = reader["COLUMN_NAME"].ToString();
                        tableView.dataType = reader["DATA_TYPE"].ToString();
                        tableView.maxLength = reader["CHARACTER_MAXIMUM_LENGTH"].ToString();
                        tableView.nullable = reader["IS_NULLABLE"].ToString();

                        tables.Add(tableView);
                    }
                    tableDetail.tableName = item.ToString();
                    tableDetail.tableDetail = tables;
                }
                else
                {
                    MessageBox.Show("No tables found in the database.");
                }
                reader.Close();
                tableDetailsList.Add(tableDetail);
            }
            this.Hide();
            GenerateFile generateFile = new GenerateFile(tableDetailsList,tableDetails,tableNames,dbName,connectionString,exportModel);
            generateFile.Show();

            // Now selectedTables list contains the names of checked tables
            // You can use this list as needed, for example, display the names in a message box
            //string selectedTablesMessage = "Selected tables: " + string.Join(", ", selectedTables);
            //MessageBox.Show(selectedTablesMessage);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
            TableList tableListForm = new TableList(tableNames, dbName, connectionString,exportModel);
            tableListForm.Show();
        }
    }
}
