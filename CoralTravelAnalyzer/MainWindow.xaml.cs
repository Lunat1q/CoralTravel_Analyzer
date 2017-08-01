using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using CoralTravelAnalyzer.Classes;
using CoralTravelAnalyzer.CoralTravelApi;
using CoralTravelAnalyzer.Ext;
using CoralTravelAnalyzer.FileDestinations.Office;
using Microsoft.Win32;
using TiqUtils.Serialize;

namespace CoralTravelAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<HotelEntry> SearchResult { get; }
        private ObservableCollection<HotelOptionEntry> OptionsResult { get; set; }

        private readonly HotelSearch _hotelSearchApi = new HotelSearch();
        private readonly PriceOptionsSearch _priceOptionsApi = new PriceOptionsSearch();
        private readonly ListFilter _listFilter = new ListFilter();

        //filter vars
        private string _selectedMealType = string.Empty;
        private string _selectedRoomType = string.Empty;
        private double _selectedMaxPrice;
        private bool _onlyWithFlight;

        public MainWindow()
        {
            InitializeComponent();
            SearchResult = new ObservableCollection<HotelEntry>();
            OptionsResult = new ObservableCollection<HotelOptionEntry>();

            DebugData();

            HotelBox.ItemsSource = SearchResult;
            ResultList.ItemsSource = OptionsResult;
            _listFilter.Filter = FilterPriceResults;
            ResultList.Items.Filter = _listFilter.Filter;
            InDate.SelectedDate = DateTime.Now;
            InitWaitImage();
        }

        private async void DebugData()
        {
#if DEBUG
            OptionsResult = Json.DeserializeDataFromFile<ObservableCollection<HotelOptionEntry>>("data.json");
            WaitImage.ShowAndRotate();
            WaitImage.SetInfoText("Test");
            for (var i = 0; i < 10; i++)
            {
                await Task.Delay(2000);
                WaitImage.SetInfoText("Test " + i);
            }
            WaitImage.Hide();
#endif
        }

        private void InitWaitImage()
        {
            WaitImage.Source = Properties.Resources.island2.LoadBitmap();
            WaitImage.ImageWidth = WaitImage.ImageHeight = 300;
        }

        private bool FilterPriceResults(object o)
        {
            var option = o as HotelOptionEntry;
            if (option == null) return true;
            
            var match = string.IsNullOrWhiteSpace(_selectedRoomType) || option.RoomType.IndexOf(_selectedRoomType, StringComparison.OrdinalIgnoreCase) > -1;

            match &= string.IsNullOrWhiteSpace(_selectedMealType) || option.MealType.IndexOf(_selectedMealType, StringComparison.OrdinalIgnoreCase) > -1;

            match &= _selectedMaxPrice <= 0 || option.TotalPrice <= _selectedMaxPrice;

            match &= !_onlyWithFlight || !option.NoFlight;

            return match;
        }

        private async void UpdateHotelSearchResult(string input, bool forceInstant = false)
        {
            if (string.IsNullOrWhiteSpace(input)) return;
            _hotelSearchApi.SetRequestParameters(input);
            var result = await _hotelSearchApi.GetDataAsync(forceInstant);
            if (result == null) return;
            SearchResult.Clear();
            foreach (var hotel in result.Result.List.Where(x => x.HotelId > 0).Take(10))
            {
                SearchResult.Add(HotelEntry.InitFromSearch(hotel));
            }
        }

        private void LockUi()
        {
            HotelBox.IsEnabled =
                InDate.IsEnabled =
                    DaysBox.IsEnabled =
                        ShiftBox.IsEnabled =
                            Start.IsEnabled = false;

            Start.Content = "Stop";

            WaitImage.ShowAndRotate();
        }

        private void UnlockUi()
        {
            HotelBox.IsEnabled =
                InDate.IsEnabled =
                    DaysBox.IsEnabled =
                        ShiftBox.IsEnabled =
                            Start.IsEnabled = true;

            Start.Content = "Start";

            WaitImage.Hide();
        }

        private async void RequstPrice(HotelEntry hotelEntry, string areaId, DateTime startDate, int shift, string nightsFrom, string nightsTo,
            string adults)
        {
            OptionsResult.Clear();
            ResetFilterBoxes();
            LockUi();
            for (var i = 0; i < shift; i++)
            {
                var curDay = startDate.AddDays(i);
                WaitImage.SetInfoText($"Requesting tour options for {curDay:dd.MM.yyyy}...");
                _priceOptionsApi.SetRequestParameters(hotelEntry.HotelEeId.ToString(), hotelEntry.CountryEeId.ToString(),
                    areaId, curDay.ToString("yyyy-MM-dd"), adults, "0", nightsFrom, nightsTo, nightsFrom, "500000", "false", "true", "20", "1",
                        "1", "Tour");
                var result = await _priceOptionsApi.GetDataAsync(false, 20);
                var noFlight = false;
                if (result == null || result.Result.Empty)
                {
                    WaitImage.SetInfoText($"No tour options found for {curDay:dd.MM.yyyy}! Requesting hotel options...");
                    _priceOptionsApi.SetRequestParameters(hotelEntry.HotelEeId.ToString(), hotelEntry.CountryEeId.ToString(),
                        areaId, curDay.ToString("yyyy-MM-dd"), adults, "0", nightsFrom, nightsTo, nightsFrom, "500000", "false", "true", "20", "1",
                        "1", "Hotel");
                    result = await _priceOptionsApi.GetDataAsync(false, 20);
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
            UnlockUi();
        }

        private void ResetFilterBoxes()
        {
            MealTypeFilterBox.SelectedIndex = -1;
            RoomTypeFilterBox.SelectedIndex = -1;
            RoomTypeFilterBox.Items.Clear();
            MealTypeFilterBox.Items.Clear();
            MaxTourPrice.Text = "";
            _selectedMaxPrice = 0;
            OnlyWithFlight.IsChecked = _onlyWithFlight = false;
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
            if (shift < 0) shift = 0;
            shift++;
            var selectedDate = DateTime.Now;
            if (InDate.SelectedDate != null)
            {
                selectedDate = (DateTime) InDate.SelectedDate;
            }
            RequstPrice(hotelEntry, "2671", selectedDate, shift, daysRange.Item1, daysRange.Item2, "2");
        }

        private void MealTypeFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb == null) return;
            var text = cb.SelectedItem as string;
            _selectedMealType = text;
            ReapplyFilter();
        }

        private void RoomTypeFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb == null) return;
            var text = cb.SelectedItem as string;
            _selectedRoomType = text;
            ReapplyFilter();
        }

        private void ReapplyFilter()
        {
            ResultList.Items.Filter = _listFilter.Filter;
        }

        private void HotelBox_DropDownOpened(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb == null) return;
            UpdateHotelSearchResult(cb.Text, true);
        }

        private void MaxTrourPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) return;
            if (!double.TryParse(tb.Text, out _selectedMaxPrice))
                _selectedMaxPrice = 0;
            ReapplyFilter();
        }

        private void OnlyWithFlight_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null) return;
            if (cb.IsChecked != null)
                _onlyWithFlight = (bool)cb.IsChecked;
            ReapplyFilter();
        }

        private void ExcelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OptionsResult.Count == 0) return;
            var hotelEntry = HotelBox.SelectedItem as HotelEntry;
            if (hotelEntry == null) return;

            var sheetName = $"{hotelEntry.HotelName}_{DateTime.Now:dd-MM-yyyy_HH_mm}";
            var dia = new SaveFileDialog
            {
                RestoreDirectory = true,
                Filter = @"Excel Worksheets (*.xlsx)|*.xlsx|xls file (*.xls)|*.xls|All files (*.*)|*.*",
                FileName = sheetName
            };

            if (dia.ShowDialog() != true) return;

            OptionsResult.OrderBy(x=>x.PricePerDay).GenerateExcelDocument(sheetName, dia.FileName);
        }
    }
}
