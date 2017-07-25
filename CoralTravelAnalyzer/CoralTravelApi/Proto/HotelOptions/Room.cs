using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.HotelOptions
{
    public class Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public object Description { get; set; }
        public bool Opened { get; set; }
        public string ImageUrl { get; set; }
        public object ImageCaption { get; set; }
        public List<Option> Options { get; set; }
}
}
