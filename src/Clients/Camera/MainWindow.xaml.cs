namespace FaceIO.Camera
{
    using AForge.Video;
    using AForge.Video.DirectShow;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media.Imaging;

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Guid _customerUid = new Guid("");
        private Guid _locationUid = new Guid("");

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<FilterInfo> VideoDevices { get; set; }

        public FilterInfo CurrentDevice
        {
            get { return _currentDevice; }
            set { _currentDevice = value; OnPropertyChanged(nameof(CurrentDevice)); }
        }

        private FilterInfo _currentDevice;
        private IVideoSource _videoSource;
        private readonly BackgroundWorker _backgroundWorker;

        private bool _isProcessingImage = true;

        private static readonly CascadeClassifier _cascadeClasifier = new CascadeClassifier(@"Classifiers\haarcascade_frontalface_alt_tree.xml");

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            GetVideoDevices();
            Closing += MainWindow_Closing;
            StartCamera();

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += CompareImage;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                    var grayImage = new Image<Bgr, byte>(bitmap);
                    var detectedFaces = _cascadeClasifier.DetectMultiScale(grayImage, 1.2, 1);

                    foreach (Rectangle rectangle in detectedFaces)
                    {
                        using (var graphics = Graphics.FromImage(bitmap))
                        using (var pen = new Pen(Color.Red, 3))
                        {
                            graphics.DrawRectangle(pen, rectangle);
                        }
                    }

                    if (detectedFaces.Length > 0
                     && !_isProcessingImage)
                    {
                        _backgroundWorker.RunWorkerAsync(bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat));
                    }

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

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        private async void CompareImage(object sender, DoWorkEventArgs e)
        {
            try
            {
                _isProcessingImage = true;

                var bitmap = e.Argument as Bitmap;

                using (var stream = new MemoryStream())
                using (var httpClient = new HttpClient())
                using (var multipartContent = new MultipartFormDataContent())
                {
                    Encoder encoder = Encoder.Quality;
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(encoder, 50L);

                    bitmap.Save(stream, jpgEncoder, encoderParameters);

                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://localhost:49153/api/customers/{_customerUid}/locations/{_locationUid}/verify"))
                    {
                        request.Headers.TryAddWithoutValidation("accept", "*/*");

                        var imageFile = new ByteArrayContent(stream.ToArray());
                        imageFile.Headers.Add("Content-Type", "image/jpeg");
                        multipartContent.Add(imageFile, "image", $"verify{DateTime.UtcNow.Minute}-{DateTime.UtcNow.Millisecond}.jpg");
                        request.Content = multipartContent;

                        var response = await httpClient.SendAsync(request);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _isProcessingImage = false;
            }
        }
    }
}