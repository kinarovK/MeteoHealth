using SQLite;
using System.ComponentModel.DataAnnotations;

namespace SQLite_Database_service.Models
{
    public class WeatherModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [DataType(DataType.DateTime), Unique]
        public string DateTime { get; set; }
        //[DataType(DataType.Date)]
        //[System.ComponentModel.DataAnnotations.Schema.Column(TypeName ="text")]

        //public string description { get; set; }
        [DataType(DataType.DateTime)]
        public string RequestDate { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public int PrecipitationProbability { get; set; }
        public double PrecipitationVolume { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }
}
