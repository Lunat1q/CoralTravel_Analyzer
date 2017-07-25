using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoralTravelAnalyzer.CoralTravelApi.Proto.HotelOptions
{
    public class HotelOptionsResult
    {
        public string Id { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public ResultOptions Result { get; set; }
        public int ElapsedMilliseconds { get; set; }
}
}
