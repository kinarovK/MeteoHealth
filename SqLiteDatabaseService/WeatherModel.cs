﻿using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqLiteDatabaseService
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
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public double Precipitation { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }
}
