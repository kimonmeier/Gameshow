using System.Windows.Input;

namespace Gameshow.Desktop.View.Windows;

public partial class BaseGameshowWindow
{
    public BaseGameshowWindow(GameshowViewModel gameshowViewModel)
    {
        InitializeComponent();
        DataContext = gameshowViewModel;
    }
    
    
    [Obsolete("Just for the Designer")]
    public BaseGameshowWindow()
    {
        InitializeComponent();
    }

    private void BaseGameshowWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.KeyDown += OnKeyEventHandler;
    }

    private void OnKeyEventHandler(object _, KeyEventArgs args)
    {
        if (args.Key != Key.Space)
        {
            return;
        }

        ((GameshowViewModel)DataContext).BuzzerPressedCommand.Execute(DataContext);
    }
}