using System.Collections.Generic;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.HotelPriceOptions
{
    public class HotelOptions
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Nights { get; set; }
        public List<Room> Rooms { get; set; }
}
}
