using DatabaseConnectionTask.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DatabaseConnectionTask
{
    public partial class TableDetails : Form
    {
        private List<TableDetail> tableDetails;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Panel panel1;
        private List<string> tableNames;
        private string connectionString;
        private string dbName;
        private Button submitButton;
        private Button backButton;

        public TableDetails(List<TableDetail> tableDetails, List<string> tableNames, string dbName, string connectionString)
        {
            InitializeComponent();
            this.tableDetails = tableDetails;
            this.tableNames = tableNames;
            this.dbName = dbName;
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
            panel1.Size = new Size(1030, 420);
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
                tableCheckBox.Text = data.TableName;
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

                foreach (var row in data.tableViews)
                {
                    tableDataGridView.Rows.Add(row.ColumnName, row.DataType, row.MaxLength, row.Nullable);
                }

                // Set DataGridView height based on the number of rows
                int dgvHeight = (data.tableViews.Count + 1) * 25; // Header + rows
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
            MessageBox.Show("Submit button clicked");
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
            TableList tableListForm = new TableList(tableNames, dbName, connectionString);
            tableListForm.Show();
        }
    }
}
