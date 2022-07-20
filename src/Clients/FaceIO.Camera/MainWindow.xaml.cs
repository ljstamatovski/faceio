namespace FaceIO.Camera
{
    using AForge.Video;
    using AForge.Video.DirectShow;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<FilterInfo> VideoDevices { get; set; }

        public FilterInfo CurrentDevice
        {
            get { return _currentDevice; }
            set { _currentDevice = value; OnPropertyChanged(nameof(CurrentDevice)); }
        }

        private FilterInfo _currentDevice;

        private IVideoSource _videoSource;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            GetVideoDevices();
            Closing += MainWindow_Closing;
            StartCamera();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                var propertyChangedEvenet = new PropertyChangedEventArgs(propertyName);
                handler(this, propertyChangedEvenet);
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            StopCamera();
        }

        private void ProcessNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bitmapImage;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bitmapImage = bitmap.ToBitmapImage();
                }

                bitmapImage.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(delegate { videoPlayer.Source = bitmapImage; }));
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error on _videoSource_NewFrame:\n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StopCamera();
            }
        }

        private void GetVideoDevices()
        {
            VideoDevices = new ObservableCollection<FilterInfo>();

            foreach (FilterInfo filterInfo in new FilterInfoCollection(FilterCategory.VideoInputDevice))
            {
                VideoDevices.Add(filterInfo);
            }

            if (VideoDevices.Any())
            {
                CurrentDevice = VideoDevices[0];
            }
            else
            {
                MessageBox.Show("No video sources found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartCamera()
        {
            if (CurrentDevice != null)
            {
                _videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
                _videoSource.NewFrame += ProcessNewFrame;
                _videoSource.Start();
            }
        }

        private void StopCamera()
        {
            if (_videoSource?.IsRunning == true)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= ProcessNewFrame;
            }
        }
    }
}