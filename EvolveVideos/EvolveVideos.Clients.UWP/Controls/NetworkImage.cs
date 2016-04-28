using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Cimbalino.Toolkit.Extensions;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace EvolveVideos.Clients.UWP.Controls
{
    [TemplatePart(Name = ImageElementName, Type = typeof(Image))]
    [TemplatePart(Name = FallbackImageElementName, Type = typeof(Image))]
    [TemplatePart(Name = ProgressRingElementName, Type = typeof(ProgressRing))]
    public sealed class NetworkImage : Control
    {
        public const string ImageElementName = "PART_Image";
        public const string FallbackImageElementName = "PART_FallbackImage";
        public const string ProgressRingElementName = "PART_ProgressRing";

        public static readonly DependencyProperty FallbackSourceProperty = DependencyProperty.Register(
            "FallbackSource", typeof(Uri), typeof(NetworkImage),
            new PropertyMetadata(default(Uri)));

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch", typeof(Stretch), typeof(NetworkImage), new PropertyMetadata(Stretch.Fill));

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(Uri), typeof(NetworkImage), new PropertyMetadata(default(Uri), SourceChanged));

        private Image _fallbackImageElement;
        private Image _imageElement;
        private ProgressRing _loadingElement;

        public NetworkImage()
        {
            DefaultStyleKey = typeof(NetworkImage);
        }

        public Uri FallbackSource
        {
            get { return (Uri) GetValue(FallbackSourceProperty); }
            set { SetValue(FallbackSourceProperty, value); }
        }

        public Uri Source
        {
            get { return (Uri) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public Stretch Stretch
        {
            get { return (Stretch) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

       
        private static async void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NetworkImage;
            if (control != null)
            {
                var newSource = e.NewValue as Uri;
                if (newSource != null)
                {
                    control._loadingElement.Visibility = Visibility.Visible;

                    var file = await control.GetImageFromCache(newSource);

                    if (file != null)
                    {
                        // Open a stream for the selected file.
                        // The 'using' block ensures the stream is disposed
                        // after the image is loaded.
                        using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            // Set the image source to the selected bitmap.
                            var bitmapImage = new BitmapImage();

                            bitmapImage.SetSource(fileStream);
                            control._imageElement.Source = bitmapImage;
                            control.ShowImage();
                            return;
                        }
                    }
                    control.ShowFallbackImage();
                }
            }
        }

        private async Task<IStorageFile> GetImageFromCache(Uri newSource)
        {
            try
            {
                var tempFolder = ApplicationData.Current.TemporaryFolder;
                var imagesFolder = await tempFolder.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists);

                var filePath = newSource.LocalPath.Replace("/", "_");

                var fileExists = await imagesFolder.TryGetItemAsync(filePath) != null;
                if (fileExists)
                {
                    var file = await imagesFolder.GetFileAsync(filePath);
                    if (file != null)
                    {
                        return file;
                    }
                }

                using (var client = new HttpClient())
                {
                    var imageData = await client.GetStreamAsync(newSource);
                    var dataArray = await imageData.ToArrayAsync();

                    var localFile = await imagesFolder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists);
                    await FileIO.WriteBytesAsync(localFile, dataArray);
                    return localFile;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _fallbackImageElement = GetTemplateChild(FallbackImageElementName) as Image;
            _imageElement = GetTemplateChild(ImageElementName) as Image;
            _loadingElement = GetTemplateChild(ProgressRingElementName) as ProgressRing;

            if (_imageElement != null)
            {
                _imageElement.ImageOpened += _imageElement_ImageOpened;
                _imageElement.ImageFailed += _imageElement_ImageFailed;
            }
        }

        private void _imageElement_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ShowFallbackImage();
        }

        private void ShowFallbackImage()
        {
            if (_fallbackImageElement != null)
            {
                _fallbackImageElement.Visibility = Visibility.Visible;
            }

            if (_loadingElement != null)
            {
                _loadingElement.Visibility = Visibility.Collapsed;
            }
            if (_imageElement != null)
            {
                _imageElement.Visibility = Visibility.Collapsed;
            }
        }

        private void _imageElement_ImageOpened(object sender, RoutedEventArgs e)
        {
            ShowImage();
        }

        private void ShowImage()
        {
            if (_fallbackImageElement != null)
            {
                _fallbackImageElement.Visibility = Visibility.Collapsed;
            }

            if (_imageElement != null)
            {
                _imageElement.Visibility = Visibility.Visible;
            }

            if (_loadingElement != null)
            {
                _loadingElement.Visibility = Visibility.Collapsed;
            }
        }
    }
}