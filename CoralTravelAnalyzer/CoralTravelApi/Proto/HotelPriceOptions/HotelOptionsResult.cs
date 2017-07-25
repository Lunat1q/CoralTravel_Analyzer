namespace CoralTravelAnalyzer.CoralTravelApi.Proto.HotelPriceOptions
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
