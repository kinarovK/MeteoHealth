using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SQLite_Database_service
{
    public class HealthStateModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
     
        [DataType(DataType.DateTime), Unique]
        public string Date { get; set; }
        public byte HealthLevel { get; set; }
    }
}
