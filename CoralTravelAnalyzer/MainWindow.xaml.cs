using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        private readonly HotelSearch _hoteSearchApi = new HotelSearch();

        public MainWindow()
        {
            InitializeComponent();
            SearchResult = new ObservableCollection<HotelEntry>();
            HotelBox.ItemsSource = SearchResult;
        }

        private async void UpdateHotelSearchResult(string input)
        {
            var hotelName = input;
            _hoteSearchApi.SetRequestParameters(hotelName);
            var result = await _hoteSearchApi.GetDataAsString();
            if (result == null) return;
            SearchResult.Clear();
            foreach (var hotel in result.Result.List.Where(x => x.HotelId > 0))
            {
                SearchResult.Add(HotelEntry.InitFromSearch(hotel));
            }
        }

        private void HotelBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox == null) return;
            var input = textBox.Text + e.Text;
            UpdateHotelSearchResult(input);
        }
    }
}
