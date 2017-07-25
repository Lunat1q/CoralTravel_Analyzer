namespace CoralTravelAnalyzer.CoralTravelApi.Proto.PriceCalendarSearch
{
    public class CalendarPriceResult
    {
        public string Id { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public PriceResult Result { get; set; }
        public int ElapsedMilliseconds { get; set; }
}
}
