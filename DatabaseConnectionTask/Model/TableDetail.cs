using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionTask.Model
{
    public class TableDetail
    {
        public string tableName { get; set; }
        public List<TableView> tableDetail { get; set; }
       
    }
    public class TableView
    {
        public string tableName { get; set; }
        public string columnName { get; set; }
        public string dataType { get; set; }
        public string maxLength { get; set; }
        public string nullable { get; set; }
    }
}
