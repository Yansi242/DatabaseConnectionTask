using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionTask.Model
{
    public class DBModel
    {
        public string ConnectionString { get; set; }

        // Constructor to initialize the model with connection string
        public DBModel(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
