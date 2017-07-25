using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.PriceSearch
{
    public class Room
    {
        public string id { get; set; }
        public string name { get; set; }
        public object description { get; set; }
        public bool opened { get; set; }
        public string imageUrl { get; set; }
        public object imageCaption { get; set; }
        public List<Option> options { get; set; }
}
}
