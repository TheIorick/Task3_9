using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Task3_9.ViewModels;
using Avalonia.Data.Converters;

namespace Task3_9.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Register value converters
            var resources = new ResourceDictionary
            {
                { "InjuryColorConverter", new InjuryColorConverter() },
                { "FinishLineStartConverter", new FinishLineStartConverter() },
                { "FinishLineEndConverter", new FinishLineEndConverter() }
            };
            
            Resources.MergedDictionaries.Add(resources);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
    
    public class InjuryColorConverter : Avalonia.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isInjured = (bool)value;
            return isInjured ? Brushes.Red : Brushes.Blue;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class FinishLineStartConverter : Avalonia.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double trackLength = (double)value;
            return new Point(trackLength, 0);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class FinishLineEndConverter : Avalonia.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double trackLength = (double)value;
            return new Point(trackLength, 400);  // Height of track
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}