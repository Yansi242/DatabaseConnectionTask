using DatabaseConnectionTask.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseConnectionTask
{
    public partial class TableDetailModel : Form
    {
        public TableDetailModel(TableDetail tableDetails)
        {
            InitializeComponent();
            InitializeTableDetailsBox(tableDetails);
        }

        private void InitializeTableDetailsBox(TableDetail tableDetails)
        {
            // Create and style the title label
            Label titleLabel = new Label();
            titleLabel.Text = $"Details of Table: {tableDetails.tableName}"; // Modify title to include table name
            titleLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 50;
            this.Controls.Add(titleLabel);

            // Create TableLayoutPanel to organize controls
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.RowCount = tableDetails.tableDetail.Count + 1; // +1 for title
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Title row

            // Add controls to TableLayoutPanel
            tableLayoutPanel.Controls.Add(titleLabel, 0, 0);

            // Create DataGridView to display table details
            DataGridView tableDataGridView = new DataGridView();
            tableDataGridView.Dock = DockStyle.Fill;
            tableDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableDataGridView.RowHeadersVisible = false;
            tableDataGridView.AllowUserToAddRows = false;
            tableDataGridView.AllowUserToDeleteRows = false;
            tableDataGridView.AllowUserToResizeRows = false;
            tableDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Define columns
            tableDataGridView.Columns.Add("ColumnName", "Column Name");
            tableDataGridView.Columns.Add("DataType", "Data Type");
            tableDataGridView.Columns.Add("MaxLength", "Max Length");
            tableDataGridView.Columns.Add("Nullable", "Is Nullable");

            // Add rows to DataGridView
            foreach (TableView tableView in tableDetails.tableDetail)
            {
                tableDataGridView.Rows.Add(tableView.columnName, tableView.dataType, tableView.maxLength, tableView.nullable);
            }

            // Add DataGridView to TableLayoutPanel
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // AutoSize for each row
            tableLayoutPanel.Controls.Add(tableDataGridView, 0, 1); // Add DataGridView to second row

            // Add TableLayoutPanel to the form
            this.Controls.Add(tableLayoutPanel);
        }

        private void TableDetails_Load(object sender, EventArgs e)
        {

        }

        private void TableDetailModel_Load(object sender, EventArgs e)
        {

        }
    }
}
