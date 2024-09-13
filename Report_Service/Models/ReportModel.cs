namespace Report_Service.Models
{
    public class ReportModel /*: IEnumerable<double>*/
    {
        public double TemperatureRelation { get; set; }
        public double HumidityRelation { get; set; }
        public double PressureRelation { get; set; }
        public double PrecProbabilityRelation { get; set; }
        public double PrecVolRelation { get; set; }
        public double WindRelation { get; set; }
        public double FullRelation { get; set; }
        public string FirstDate { get; set; }
        public string LastDate { get; set; }
    
    }
}
