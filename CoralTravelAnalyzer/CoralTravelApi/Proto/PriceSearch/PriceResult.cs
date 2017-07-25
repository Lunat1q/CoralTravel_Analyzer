using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.PriceSearch
{
    public class PriceResult
    {
        public string Id { get; set; }
        public List<PriceVariants> List { get; set; }
        public bool OnlyHotels { get; set; }
}
}
