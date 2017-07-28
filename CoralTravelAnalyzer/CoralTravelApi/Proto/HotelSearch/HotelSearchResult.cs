namespace CoralTravelAnalyzer.CoralTravelApi.Proto.HotelSearch
{
    public class HotelSearchResult
    {
        public string Id { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public ResultHotels Result { get; set; }
        public int ElapsedMilliseconds { get; set; }
}
}
