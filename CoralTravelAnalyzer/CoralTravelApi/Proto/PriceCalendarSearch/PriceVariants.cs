using System.Collections.Generic;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.PriceCalendarSearch
{
    public class PriceVariants
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DayPrice> Days { get; set; }
}
}
