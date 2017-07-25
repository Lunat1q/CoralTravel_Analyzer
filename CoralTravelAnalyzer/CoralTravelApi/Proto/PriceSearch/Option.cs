namespace CoralTravelAnalyzer.CoralTravelApi.Proto.PriceSearch
{
    public class Option
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int ChildAge1 { get; set; }
        public int ChildAge2 { get; set; }
        public int ChildAge3 { get; set; }
        public double Price { get; set; }
        public int HotelEeId { get; set; }
        public int CountryEeId { get; set; }
        public int AreaFromEeId { get; set; }
        public int RoomEeId { get; set; }
        public int MealEeId { get; set; }
        public int AccEeId { get; set; }
        public int Nights { get; set; }
        public string BeginDate { get; set; }
        public string RoomName { get; set; }
        public string SearchType { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCss { get; set; }
        public string CurrencyName { get; set; }
        public bool StopSale { get; set; }
        public double LocalPrice { get; set; }
        public string LocalCurrencyCss { get; set; }
        public string Hash { get; set; }
        public object PriceImage { get; set; }
        public int ToCountry { get; set; }
        public int AreaId { get; set; }
}
}
