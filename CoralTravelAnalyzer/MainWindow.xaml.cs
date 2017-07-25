using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using CoralTravelAnalyzer.Classes;
using CoralTravelAnalyzer.CoralTravelApi;
using MahApps.Metro.Controls;

namespace CoralTravelAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ObservableCollection<HotelEntry> SearchResult { get; }
        private ObservableCollection<HotelOptionEntry> OptionsResult { get; }

        private readonly HotelSearch _hotelSearchApi = new HotelSearch();
        private readonly PriceOptionsSearch _priceOptionsApi = new PriceOptionsSearch();
        private readonly ListFilter _listFilter = new ListFilter();

        public MainWindow()
        {
            InitializeComponent();
            SearchResult = new ObservableCollection<HotelEntry>();
            OptionsResult = new ObservableCollection<HotelOptionEntry>();
            HotelBox.ItemsSource = SearchResult;
            ResultList.ItemsSource = OptionsResult;
            _listFilter.Filter = FilterPriceResults;
            ResultList.Items.Filter = _listFilter.Filter;
        }

        private bool FilterPriceResults(object o)
        {
            if (_selectedRoomType == string.Empty && _selectedMealType == string.Empty)
            {
                return true;
            }
            var option = o as HotelOptionEntry;
            if (option == null) return true;
            if (_selectedRoomType != string.Empty && _selectedMealType == string.Empty)
            {
                return option.RoomType.IndexOf(_selectedRoomType, StringComparison.OrdinalIgnoreCase) > -1;
            }
            if (_selectedRoomType == string.Empty && _selectedMealType != string.Empty)
            {
                return option.MealType.IndexOf(_selectedMealType, StringComparison.OrdinalIgnoreCase) > -1;
            }
            if (_selectedRoomType != string.Empty && _selectedMealType != string.Empty)
            {
                return option.MealType.IndexOf(_selectedMealType, StringComparison.OrdinalIgnoreCase) > -1
                       && option.RoomType.IndexOf(_selectedRoomType, StringComparison.OrdinalIgnoreCase) > -1;
            }
            return true;
        }

        private async void UpdateHotelSearchResult(string input)
        {
            var hotelName = input;
            _hotelSearchApi.SetRequestParameters(hotelName);
            var result = await _hotelSearchApi.GetDataAsync();
            if (result == null) return;
            SearchResult.Clear();
            foreach (var hotel in result.Result.List.Where(x => x.HotelId > 0))
            {
                SearchResult.Add(HotelEntry.InitFromSearch(hotel));
            }
        }

        private async void RequstPrice(HotelEntry hotelEntry, string areaId, DateTime startDate, int shift, string nightsFrom, string nightsTo,
            string adults)
        {
            OptionsResult.Clear();
            ResetFilterBoxes();
            for (int i = 0; i < shift; i++)
            {
                _priceOptionsApi.SetRequestParameters(hotelEntry.HotelEeId.ToString(), hotelEntry.CountryEeId.ToString(),
                    areaId, startDate.AddDays(i).ToString("yyyy-MM-dd"), adults, "0", nightsFrom, nightsTo, nightsFrom, "500000", "false", "true", "20", "1",
                        "1", "Tour");
                var result = await _priceOptionsApi.GetDataAsync(i > 0);
                var noFlight = false;
                if (result == null || result.Result.Empty)
                {
                    _priceOptionsApi.SetRequestParameters(hotelEntry.HotelEeId.ToString(), hotelEntry.CountryEeId.ToString(),
                        areaId, startDate.AddDays(i).ToString("yyyy-MM-dd"), adults, "0", nightsFrom, nightsTo, nightsFrom, "500000", "false", "true", "20", "1",
                        "1", "Hotel");
                    result = await _priceOptionsApi.GetDataAsync(i > 0);
                    noFlight = true;
                }
                var parsedResult = HotelOptionEntry.GenerateFromResultOptions(result?.Result, noFlight);

                foreach (var item in parsedResult)
                {
                    OptionsResult.Add(item);
                }
            }
            ResultList.Items.SortDescriptions.Add(new SortDescription("PricePerDay", ListSortDirection.Ascending));
            BuildFilterBoxes();
        }

        private void ResetFilterBoxes()
        {
            MealTypeFilterBox.SelectedIndex = -1;
            RoomTypeFilterBox.SelectedIndex = -1;
            RoomTypeFilterBox.Items.Clear();
            MealTypeFilterBox.Items.Clear();
            _selectedMealType = _selectedRoomType = string.Empty;
        }

        private void BuildFilterBoxes()
        {
            var mealGroups = OptionsResult.GroupBy(x => x.MealType);
            MealTypeFilterBox.Items.Add("");
            foreach (var meal in mealGroups)
            {
                MealTypeFilterBox.Items.Add(meal.Key);
            }

            var roomGroups = OptionsResult.GroupBy(x => x.RoomType);
            RoomTypeFilterBox.Items.Add("");
            foreach (var room in roomGroups)
            {
                RoomTypeFilterBox.Items.Add(room.Key);
            }
        }


        private void HotelBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox == null) return;
            var input = textBox.Text + e.Text;
            UpdateHotelSearchResult(input);
        }

        private Tuple<string, string> GetDaysRange()
        {
            var daysRange = DaysBox.Text;
            var days = daysRange.Split(new[]{'-'}, StringSplitOptions.RemoveEmptyEntries);
            switch (days.Length)
            {
                case 1:
                    return Tuple.Create(days[0], days[0]);
                case 2:
                    return Tuple.Create(days[0], days[1]);
                default:
                    return Tuple.Create("10", "14");
            }
        }

        private void Start_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var daysRange = GetDaysRange();
            var hotelEntry = HotelBox.SelectedItem as HotelEntry;
            if (hotelEntry == null) return;

            int.TryParse(ShiftBox.Text, out int shift);
            if (shift == 0) shift = 1;
            RequstPrice(hotelEntry, "2671", InDate.DisplayDate, shift, daysRange.Item1, daysRange.Item2, "2");
        }

        private string _selectedMealType = string.Empty;
        private string _selectedRoomType = string.Empty;

        private void MealTypeFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null)
            {
                var text = cb.SelectedItem as string;
                _selectedMealType = text;
                ReapplyFilter();
            }
        }

        private void RoomTypeFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null)
            {
                var text = cb.SelectedItem as string;
                _selectedRoomType = text;
                ReapplyFilter();
            }
        }

        private void ReapplyFilter()
        {
            ResultList.Items.Filter = _listFilter.Filter;
        }
    }
}
