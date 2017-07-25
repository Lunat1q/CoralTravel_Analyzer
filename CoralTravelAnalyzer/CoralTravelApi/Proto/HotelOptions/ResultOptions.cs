using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.HotelOptions
{
    public class ResultOptions
    {
        public string Id { get; set; }
        public List<HotelOptions> List { get; set; }
        public double MinPrice { get; set; }
        public string Date { get; set; }
        public bool HasOtherResults { get; set; }
        public double AnotherDepartureCityPrice { get; set; }
        public object AnotherDepartureCityName { get; set; }
        public object AnotherDepartureCityAreaEeId { get; set; }
        public string CurrencyCss { get; set; }
        public string SearchKey { get; set; }
        public List<int> RoomEeIds { get; set; }
        public List<int> MealEeIds { get; set; }
        public List<int> HotelEeIds { get; set; }
        public List<int> AllotmentStatus { get; set; }
}
}
