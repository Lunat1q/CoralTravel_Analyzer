using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.PriceSearch
{
    public class DayPrice
    {
        public string Id { get; set; }
        public int day { get; set; }
        public int Price { get; set; }
        public int Allotment { get; set; }
        public int FlightStatus { get; set; }
        public string TotalPrice { get; set; }
        public string Currency { get; set; }
        public string Date { get; set; }
        public string FullDate { get; set; }
}
}
