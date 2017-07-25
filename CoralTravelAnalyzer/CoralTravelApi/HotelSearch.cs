using System;
using CoralTravelAnalyzer.CoralTravelApi.Proto.HotelSearch;

namespace CoralTravelAnalyzer.CoralTravelApi
{
    public class HotelSearch : ApiBase<HotelSearchResult>
    {
        private static string _apiUrl = "smartsearch?cl=true&key=";

        private string _hotelName;

        protected override string GetRequestUrl()
        {
            return _apiUrl + _hotelName;
        }

        public override void SetRequestParameters(params string[] parameters)
        {
            if (parameters.Length > 1 || parameters.Length == 0)
            {
                throw new ArgumentException();
            }
            _hotelName = parameters[0];
        }
    }
}
