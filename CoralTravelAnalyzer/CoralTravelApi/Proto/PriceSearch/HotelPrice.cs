using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.PriceSearch
{
    public class HotelPrice
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Nights { get; set; }
        public List<Room> Rooms { get; set; }
}
}
