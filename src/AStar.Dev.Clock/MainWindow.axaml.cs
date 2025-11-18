using AStar.Dev.Clock.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
        AvaloniaXamlLoader.Load(this);
    }

    private void OnLight(object? sender, RoutedEventArgs e)
        => (Application.Current as App)?.SetTheme(ThemeVariant.Light);

    private void OnDark(object? sender, RoutedEventArgs e)
        => (Application.Current as App)?.SetTheme(ThemeVariant.Dark);

    private void OnAuto(object? sender, RoutedEventArgs e)
        => (Application.Current as App)?.SetTheme(null);

    private void OnArabicNumerals(object? sender, RoutedEventArgs e)
    {
        var clock = this.FindControl<AnalogClockControl>("Clock");
        if (clock != null) clock.NumeralStyle = NumeralStyle.Arabic;
    }

    private void OnRomanNumerals(object? sender, RoutedEventArgs e)
    {
        var clock = this.FindControl<AnalogClockControl>("Clock");
        if (clock != null) clock.NumeralStyle = NumeralStyle.Roman;
    }

    private void OnNoNumerals(object? sender, RoutedEventArgs e)
    {
        var clock = this.FindControl<AnalogClockControl>("Clock");
        if (clock != null) clock.NumeralStyle = NumeralStyle.None;
    }

    private void OnClose(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
