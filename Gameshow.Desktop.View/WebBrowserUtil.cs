using System.Windows;
using System.Windows.Controls;
using CefSharp.Wpf;

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
        if (o is not ChromiumWebBrowser browser)
        {
            return;
        }
        
        string? uri = e.NewValue as string;
        browser.Address = !string.IsNullOrEmpty(uri) ? uri : null;
    }
}