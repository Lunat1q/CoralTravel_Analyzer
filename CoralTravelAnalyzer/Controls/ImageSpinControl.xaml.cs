using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CoralTravelAnalyzer.Controls
{
    /// <summary>
    /// Interaction logic for ImageSpinControl.xaml
    /// </summary>
    public partial class ImageSpinControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _imageWidth;
        private int _imageHeight;
        private bool _isRotating;
        private ImageSource _imageSource;


        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                OnPropertyChanged();
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

        public bool IsRotating
        {
            get => _isRotating;
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

        public ImageSpinControl()
        {
            InitializeComponent();
            ImageWidth = 100;
            ImageHeight = 100;
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
