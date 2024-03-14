using System.Windows;
using System.Windows.Controls;

namespace Gameshow.Desktop.View;

public class WebBrowserUtil
{
    public readonly static DependencyProperty BindableSourceProperty =
        DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserUtil), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

    public static string GetBindableSource(DependencyObject obj)
    {
        return (string)obj.GetValue(BindableSourceProperty);
    }

    public static void SetBindableSource(DependencyObject obj, string value)
    {
        obj.SetValue(BindableSourceProperty, value);
    }

    private static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        if (o is not WebBrowser browser)
        {
            return;
        }
        
        string? uri = e.NewValue as string;
        browser.Source = !string.IsNullOrEmpty(uri) ? new Uri(uri) : null;
    }
}