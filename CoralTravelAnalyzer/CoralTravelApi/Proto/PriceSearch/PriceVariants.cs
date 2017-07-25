using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.PriceSearch
{
    public class PriceVariants
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DayPrice> Days { get; set; }
}
}
