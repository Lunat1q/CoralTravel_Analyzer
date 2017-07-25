using CoralTravelAnalyzer.CoralTravelApi.Proto.HotelSearch;
using TiQWpfUtils.AbstractClasses;

namespace CoralTravelAnalyzer.Classes
{
    public class HotelEntry : Notified
    {
        private string _countryName;
        private string _hotelName;
        private int _hotelId;
        private int _countryId;

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

        public int HotelId
        {
            get => _hotelId;
            set
            {
                _hotelId = value;
                OnPropertyChanged();
            }
        }

        public int CountryId
        {
            get => _countryId;
            set
            {
                _countryId = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return HotelName;
        }

        public static HotelEntry InitFromSearch(HotelSearchEntry searchEntry)
        {
            var entry = new HotelEntry
            {
                HotelId = searchEntry.HotelId,
                HotelName = searchEntry.Text,
                CountryId = searchEntry.CountryId,
                CountryName = searchEntry.Description
            };

            return entry;
        }
    }
}
