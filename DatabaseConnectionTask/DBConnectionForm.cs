using DatabaseConnectionTask.Constants;
using DatabaseConnectionTask.Model;
using System.Data.SqlClient;

namespace DatabaseConnectionTask
{
    public partial class DBConnection : Form
    {
        private SqlConnection connection;
        private CheckedListBox checkboxList;
        private ErrorProvider errorProviderServerName;
        private ErrorProvider errorProviderDBName;
        private ErrorProvider errorProviderDBLoginID;
        private ErrorProvider errorProviderDBPassword;
        public DBConnection()
        {
            InitializeComponent();
            InitializeCheckboxList();
            InitializeErrorProviders();
        }
        private void InitializeErrorProviders()
        {
            errorProviderServerName = new ErrorProvider();
            errorProviderServerName.SetIconAlignment(ServerName, ErrorIconAlignment.MiddleRight);

            errorProviderDBName = new ErrorProvider();
            errorProviderDBName.SetIconAlignment(DBName, ErrorIconAlignment.MiddleRight);

            errorProviderDBLoginID = new ErrorProvider();
            errorProviderDBLoginID.SetIconAlignment(DBLoginID, ErrorIconAlignment.MiddleRight);

            errorProviderDBPassword = new ErrorProvider();
            errorProviderDBPassword.SetIconAlignment(DBPassword, ErrorIconAlignment.MiddleRight);
        }

        private void InitializeCheckboxList()
        {
            checkboxList = new CheckedListBox();
            checkboxList.Dock = DockStyle.Fill;
            this.Controls.Add(checkboxList);
        }

        private void submit_Click(object sender, EventArgs e)
        {
            errorProviderServerName.Clear();
            errorProviderDBName.Clear();
            errorProviderDBLoginID.Clear();
            errorProviderDBPassword.Clear();

            bool isValid = true;
            string serverName = ServerName.Text.Trim();
            string dbName = DBName.Text.Trim();
            string username = DBLoginID.Text.Trim();
            string password = DBPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(serverName))
            {
                errorProviderServerName.SetError(ServerName, Messages.servernamerequired);
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(dbName))
            {
                errorProviderDBName.SetError(DBName, Messages.datbaserequired);
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                errorProviderDBLoginID.SetError(DBLoginID, Messages.usernamerequired);
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errorProviderDBPassword.SetError(DBPassword, Messages.passwordrequired);
                isValid = false;
            }

            if (!isValid)
                return;

            Loader loader = new Loader();

            string connectionString = $"data source={serverName};initial catalog={dbName};user id={username};password={password};MultipleActiveResultSets=True;";

            try
            {
                loader.Show();
                this.Hide();
                connection = new SqlConnection(connectionString);
                connection.Open();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    loader.Hide();
                    MessageBox.Show(Messages.connectsucessfully);
                    this.Hide();

                    exportModel exportmodel = new exportModel();
                    exportmodel.serverName = serverName;
                    exportmodel.login = username;
                    exportmodel.password = password;
                    exportmodel.databaseName = dbName;

                    List<string> tableNames = new List<string>();
                    FillCheckboxList(dbName, tableNames);

                    TableList tableListForm = new TableList(tableNames, dbName, connectionString, exportmodel);
                    tableListForm.Show();
                }
                else
                {
                    loader.Hide();
                    this.Show();
                    MessageBox.Show(Messages.connectionfaild);
                }
            }
            catch (Exception ex)
            {
                loader.Hide();
                this.Show();
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FillCheckboxList(string dbName, List<string> tableNames)
        {
            try
            {
                string query = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '{dbName}';"; // Modify this query according to your table structure
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string itemName = reader["TABLE_NAME"].ToString();
                        checkboxList.Items.Add(itemName);
                        tableNames.Add(itemName);
                    }
                }
                else
                {
                    MessageBox.Show(Messages.notablefound);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Messages.errorforcheckbox + ex.Message);
            }
        }
    }
}
