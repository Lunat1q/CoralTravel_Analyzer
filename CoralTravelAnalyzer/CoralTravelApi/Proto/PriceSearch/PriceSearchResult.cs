using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.PriceSearch
{
    public class PriceSearchResult
    {
        public string Id { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public ResultPrice Result { get; set; }
        public int ElapsedMilliseconds { get; set; }
}
}
