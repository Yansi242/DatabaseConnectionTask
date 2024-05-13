using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionTask.Model
{
    public class TableDetail
    {
        public string TableName { get; set; }
        public List<TableView> tableViews { get; set; }
       
    }
    public class TableView
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string MaxLength { get; set; }
        public string Nullable { get; set; }
    }
}
