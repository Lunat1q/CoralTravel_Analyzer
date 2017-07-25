using System.Collections.Generic;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.HotelSearch
{
    public class Result
    {
        public string Id { get; set; }
        public List<HotelSearchEntry> List { get; set; }
}
}
