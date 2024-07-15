using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SQLite_Database_service
{
    public class GeolocationModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [DataType(DataType.DateTime), Unique]
        public string DateTime { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
