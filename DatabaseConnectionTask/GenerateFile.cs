using DatabaseConnectionTask.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseConnectionTask
{
    public partial class GenerateFile : Form
    {
        List<TableDetail> TableDetailsList;
        private ListView listViewTables;
        private List<TableDetail> tableDetails;
        private List<string> tableNames;
        private string connectionString;
        private string dbName;
        TextBox projectNameTextBox = new TextBox();
        public GenerateFile(List<TableDetail> tableDetailsList, List<TableDetail> tableDetails, List<string> tableNames, string dbName, string connectionString)
        {
            this.TableDetailsList = tableDetailsList;
            this.tableDetails = tableDetails;
            this.tableNames = tableNames;
            this.dbName = dbName;
            this.connectionString = connectionString;
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Create and style the title label
            Label titleLabel = new Label();
            titleLabel.Text = "Generate File Page";
            titleLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 50;
            this.Controls.Add(titleLabel);

            // Create and style the label and input field for project name
            Label projectNameLabel = new Label();
            projectNameLabel.Text = "Project Name:";
            projectNameLabel.AutoSize = true;
            projectNameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            projectNameLabel.TextAlign = ContentAlignment.MiddleCenter;
            projectNameLabel.Location = new Point(263, 149);
            this.Controls.Add(projectNameLabel);

            
            projectNameTextBox.Location = new Point(projectNameLabel.Right + 10, projectNameLabel.Top);
            projectNameTextBox.Width = 200;
            projectNameTextBox.Name = "projectName";
            this.Controls.Add(projectNameTextBox);

            // Create and style the list view
            listViewTables = new ListView();
            listViewTables.View = View.Details;
            listViewTables.FullRowSelect = true;
            listViewTables.AutoArrange = true;
            listViewTables.Width = 400; // Adjust width as needed
            listViewTables.Location = new Point((this.ClientSize.Width - listViewTables.Width) / 2,
                                                (this.ClientSize.Height - listViewTables.Height) / 2);
            listViewTables.Anchor = AnchorStyles.None;
            listViewTables.Columns.Add("Tables List", 300, HorizontalAlignment.Center);

            foreach (var tableName in TableDetailsList)
            {
                ListViewItem item = new ListViewItem(tableName.tableName);
                listViewTables.Items.Add(item);
            }

            this.Controls.Add(listViewTables);

            // Center the list view horizontally
            int listViewCenterX = (this.ClientSize.Width - listViewTables.Width) / 2;
            listViewTables.Location = new Point(listViewCenterX, listViewTables.Top);

            // Create and style the back button
            Button backButton = new Button();
            backButton.Text = "Back";
            backButton.BackColor = Color.GhostWhite; 
            backButton.ForeColor = Color.Black; 
            backButton.Width = 100;
            backButton.Height = 35; 
            backButton.Location = new Point(this.ClientSize.Width / 2 - 100, listViewTables.Bottom + 20);
            backButton.Click += BackButton_Click;
            this.Controls.Add(backButton);

            // Create and style the submit button
            Button submitButton = new Button();
            submitButton.Text = "Generate File";
            submitButton.BackColor = Color.GhostWhite; 
            submitButton.ForeColor = Color.Black; 
            submitButton.Width = 100; 
            submitButton.Height = 35; 
            submitButton.Location = new Point(this.ClientSize.Width / 2 + 10, listViewTables.Bottom + 20);
            submitButton.Click += SubmitButton_Click;
            this.Controls.Add(submitButton);
        }


        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close(); 
            TableDetails tabledetails = new TableDetails(tableDetails,tableNames,dbName,connectionString);
            tabledetails.Show();
        }

        private async void SubmitButton_Click(object sender, EventArgs e)
        {
            string projectName = projectNameTextBox.Text;
            generatefilemodel generateFile = new generatefilemodel();
            generateFile.projectName = projectName;
            generateFile.tableDetailList = TableDetailsList;

            string filePath = await GenerateProjectFile(generateFile);
            if (filePath != null)
            {
                this.Hide();
                MessageBox.Show("File saved at: " + filePath);
            }
            else
            {
                MessageBox.Show("File generation failed.");
            }

        }

        protected async Task<string> GenerateProjectFile(generatefilemodel generateFile)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var apiUrLexport = "http://192.168.3.5:4001/api/file-generator/export";

                    // Serialize generateFile object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(generateFile), Encoding.UTF8, "application/json");

                    // Send POST request to the API
                    HttpResponseMessage response = await client.PostAsync(apiUrLexport, jsonContent);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read response content as byte array
                        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                        // Ask user for folder path to save the zip file
                        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Save the zip file
                            string folderPath = folderBrowserDialog.SelectedPath;
                            string filePath = Path.Combine(folderPath, $"{projectNameTextBox.Text}.zip");
                            File.WriteAllBytes(filePath, fileBytes);
                            return filePath; // Return the file path                         
                        }
                        
                    }
                    else
                    {
                        // Handle failure
                        MessageBox.Show("File generation request failed: " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error
                MessageBox.Show("Error: " + ex.Message);
            }
            return null; // Return null if there's an error
        }

    }
}
