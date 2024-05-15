using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionTask.Constants
{
    public class Messages
    {
        #region DBConnectionForm
        public const string servernamerequired = "Server name is required.";
        public const string datbaserequired = "Database name is required.";
        public const string usernamerequired = "Username is required.";
        public const string passwordrequired = "Password is required.";
        public const string connectsucessfully = "Connected to database successfully!";
        public const string connectionfaild = "Failed to connect to database!";
        public const string notablefound = "No tables found in the database.";
        public const string errorforcheckbox = "Error while filling checkbox list: ";
        #endregion
    }
}
