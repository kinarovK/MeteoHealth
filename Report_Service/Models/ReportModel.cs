using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //public IEnumerator<double> GetEnumerator()
        //{
        //    yield return TemperatureRelation;
        //    yield return HumidityRelation;
        //    yield return PressureRelation;
        //    yield return PrecProbabilityRelation;
        //    yield return PrecVolRelation;
        //    yield return WindRelation;
        //    yield return FullRelation;
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
