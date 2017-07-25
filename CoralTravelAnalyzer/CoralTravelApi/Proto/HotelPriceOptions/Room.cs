using System.Collections.Generic;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.HotelPriceOptions
{
    public class Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public object Description { get; set; }
        public bool Opened { get; set; }
        public string ImageUrl { get; set; }
        public object ImageCaption { get; set; }
        public List<PriceOption> Options { get; set; }
}
}
