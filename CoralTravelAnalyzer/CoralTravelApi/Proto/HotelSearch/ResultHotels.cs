using System.Collections.Generic;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.HotelSearch
{
    public class ResultHotels
    {
        public string Id { get; set; }
        public List<HotelSearchEntry> List { get; set; }
}
}
