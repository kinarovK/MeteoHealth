using System.ComponentModel.DataAnnotations;
using SQLite;

namespace SQLite_Database_service.Models
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
