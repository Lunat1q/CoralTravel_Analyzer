using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace CoralTravelAnalyzer.Controls
{
    /// <summary>
    /// Interaction logic for ImageSpinControl.xaml
    /// </summary>
    public partial class ImageSpinControl : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _imageWidth;
        private int _imageHeight;
        private bool _isRotating;
        private string _infoText;
        private ImageSource _imageSource;
        private Thickness _infoTextMargin;
        private Visibility _infoTextVisible;


        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                OnPropertyChanged();
                RecalculateTextMargin();
            }
        }

        public int ImageHeight {
            get => _imageHeight;
            set
            {
                _imageHeight = value;
                OnPropertyChanged();
            }
        }

        private bool IsRotating
        {
            set
            {
                _isRotating = value;
                VisualStateManager.GoToState(this, _isRotating ? "RotationState" : "StopState", true);
            }
        }

        public ImageSource Source
        {
            get => _imageSource;
            set { _imageSource = value; OnPropertyChanged(); }
        }

        public string InfoText
        {
            get => _infoText;
            set
            {
                _infoText = value;
                OnPropertyChanged();
                CheckIsVisible();
            }
        }

        public Thickness InfoTextMargin
        {
            get => _infoTextMargin;
            set
            {
                _infoTextMargin = value;
                OnPropertyChanged();
            }
        }

        public Visibility InfoTextVisible
        {
            get => _infoTextVisible;
            set
            {
                _infoTextVisible = value;
                OnPropertyChanged();
            }
        }

        private void CheckIsVisible()
        {
            InfoTextVisible = string.IsNullOrWhiteSpace(InfoText) ? Visibility.Collapsed : Visibility.Visible;
        }

        public async void SetInfoText(string text)
        {
            await Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                InfoText = text;
            }));
        }

        private void RecalculateTextMargin()
        {
            InfoTextMargin = new Thickness(0,0,0,ImageHeight + 75);
        }

        public ImageSpinControl()
        {
            InitializeComponent();
            ImageWidth = 100;
            ImageHeight = 100;
            InfoText = string.Empty;
            LayoutRoot.DataContext = this;
        }

        public void ShowAndRotate()
        {
            Visibility = Visibility.Visible;
            IsRotating = true;
        }

        public void Hide()
        {
            IsRotating = false;
            Visibility = Visibility.Collapsed;
        }
    }
}
