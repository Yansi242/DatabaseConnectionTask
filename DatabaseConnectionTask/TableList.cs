﻿using DatabaseConnectionTask.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DatabaseConnectionTask
{
    public partial class TableList : Form
    {
        private SqlConnection connection;
        private string connectionString;
        private CheckedListBox checkedListBoxTables;
        private Button submitButton;

        public TableList(List<string> tableNames, string Dbname, string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            InitializeCheckedListBox(tableNames, Dbname);
            submitButton.Click += SubmitButton_Click;
        }

        private void InitializeCheckedListBox(List<string> tableNames, string Dbname)
        {
            // Create and style the title label
            Label titleLabel = new Label();
            titleLabel.Text = "Tables List of " + Dbname;
            titleLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 50;
            this.Controls.Add(titleLabel);

            // Create and style the checked list box
            checkedListBoxTables = new CheckedListBox();
            checkedListBoxTables.BorderStyle = BorderStyle.None; // Remove borders
            checkedListBoxTables.BackColor = Color.White; // Remove background color
            checkedListBoxTables.Dock = DockStyle.None;
            checkedListBoxTables.Size = new Size(450, 250); // Set the size as per your requirement
            checkedListBoxTables.Location = new Point((this.ClientSize.Width - checkedListBoxTables.Width) / 2, (this.ClientSize.Height - checkedListBoxTables.Height) / 2);
            this.Controls.Add(checkedListBoxTables);

            // Add table names to the checkedListBoxTables
            checkedListBoxTables.Items.AddRange(tableNames.ToArray());

            // Create and style the submit button
            // 
            // submit
            submitButton = new Button();
            submitButton.Text = "Submit";
            submitButton.Font = new Font("Arial", 12, FontStyle.Regular);
            submitButton.BackColor = Color.GhostWhite; // Setting a different background color
            submitButton.ForeColor = Color.Black; // Setting text color
            submitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; // Anchor to bottom-right
            submitButton.Width = 100; // Setting a small width size
            submitButton.Height = 35; // Setting a specific height
            submitButton.Location = new Point((this.ClientSize.Width - submitButton.Width) / 2, this.ClientSize.Height - submitButton.Height - 20); // Adjust location
            this.Controls.Add(submitButton);

            // Adjust form layout to center contents vertically
            int totalHeight = titleLabel.Height + checkedListBoxTables.Height + submitButton.Height;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            int verticalPosition = (screenHeight - totalHeight) / 2;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, verticalPosition);
        }
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // Get checked items and their values
            List<string> checkedItems = new List<string>();
            foreach (object item in checkedListBoxTables.CheckedItems)
            {
                checkedItems.Add(item.ToString());
            }

            // Do something with the checked items
            string message = string.Join("$", checkedItems);
            TableDetails(checkedItems, connectionString);
            //MessageBox.Show("Table data submitted");
            this.Close();
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
