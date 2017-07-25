using System.Collections.Generic;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.PriceCalendarSearch
{
    public class PriceResult
    {
        public string Id { get; set; }
        public List<PriceVariants> List { get; set; }
        public bool OnlyHotels { get; set; }
}
}
