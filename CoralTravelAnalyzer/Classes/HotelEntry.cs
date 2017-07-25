using CoralTravelAnalyzer.CoralTravelApi.Proto.HotelSearch;
using TiQWpfUtils.AbstractClasses;

namespace CoralTravelAnalyzer.Classes
{
    public class HotelEntry : Notified
    {
        private string _countryName;
        private string _hotelName;

        public string CountryName
        {
            get => _countryName;
            set
            {
                _countryName = value;
                OnPropertyChanged();
            }
        }

        public string HotelName
        {
            get => _hotelName;
            set
            {
                _hotelName = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return HotelName;
        }

        public int CountryEeId => _baseEntry.CountryEeId;
        public int HotelEeId => _baseEntry.HotelEeId;

        private HotelSearchEntry _baseEntry;

        public static HotelEntry InitFromSearch(HotelSearchEntry searchEntry)
        {
            var entry = new HotelEntry
            {
                HotelName = searchEntry.Text,
                CountryName = searchEntry.Description,
                _baseEntry = searchEntry
            };
            return entry;
        }
    }
}
