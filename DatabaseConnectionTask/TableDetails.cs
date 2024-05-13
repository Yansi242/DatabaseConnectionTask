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
using System.Xml.Linq;

namespace DatabaseConnectionTask
{
    public partial class TableDetails : Form
    {
        public TableDetails(List<TableDetail> tabledetails)
        {
            InitializeComponent();
            InitializeTableDetailsBox(tabledetails);
        }

        private void InitializeTableDetailsBox(List<TableDetail> tabledetails)
        {
            // Create and style the title label
            Label titleLabel = new Label();
            titleLabel.Text = "Details of Tables";
            titleLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 50;
            this.Controls.Add(titleLabel);

            // Create TableLayoutPanel to organize controls
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.RowCount = tabledetails.Count + 1; // +1 for title
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Title row

            // Add controls to TableLayoutPanel
            tableLayoutPanel.Controls.Add(titleLabel, 0, 0);

            int rowIndex = 1; // Start from 1 to skip the title row
            foreach(var data in tabledetails) 
            {

                // Assuming tableDetail is an object containing TableName, ColumnName, DataType, MaxLength, IsNullable
                // You can adjust this part according to your actual data structure        
                // Create CheckBox for table selection
                CheckBox tableCheckBox = new CheckBox();
                tableCheckBox.Text = data.TableName; // TableName
                tableCheckBox.CheckedChanged += TableCheckBox_CheckedChanged;

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

                foreach(TableView row in data.tableViews)
                {
                    tableDataGridView.Rows.Add(row.ColumnName, row.DataType,row.MaxLength,row.Nullable);

                }
                // Add data

                // Add controls to TableLayoutPanel
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // AutoSize for each row
                tableLayoutPanel.Controls.Add(tableCheckBox, 0, rowIndex);
                tableLayoutPanel.Controls.Add(tableDataGridView, 1, rowIndex);

                rowIndex++;
            }

            this.Controls.Add(tableLayoutPanel);
        }

        private void TableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Handling select all/deselect all logic
            CheckBox checkBox = sender as CheckBox;
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox && control != checkBox)
                {
                    ((CheckBox)control).Checked = checkBox.Checked;
                }
            }
        }
    }
}
