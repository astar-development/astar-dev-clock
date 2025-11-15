using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace AStar.Dev.Clock;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoaderPortable.Ensure();
        Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
    }

    private void OnLight(object? sender, RoutedEventArgs e)
        => (Application.Current as App)?.SetTheme(ThemeVariant.Light);

    private void OnDark(object? sender, RoutedEventArgs e)
        => (Application.Current as App)?.SetTheme(ThemeVariant.Dark);

    private void OnAuto(object? sender, RoutedEventArgs e)
        => (Application.Current as App)?.SetTheme(null);
}

// Small helper to avoid trimming issues in single-file publish scenarios
internal static class AvaloniaXamlLoaderPortable
{
    private static bool _loaded;
    public static void Ensure()
    {
        if (_loaded) return;
        _loaded = true;
    }
}
