using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionTask.Model
{
    public class generatefilemodel
    {
        public string projectName {  get; set; }
        public List<TableDetail> tableDetailList { get; set; }
    }
    public class exportModel
    {
        public string serverName { get; set; }
        public string login {  get; set; }
        public string password { get; set; }
        public string databaseName { get; set; }
    }
}
