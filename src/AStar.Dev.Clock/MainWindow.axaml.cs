using AStar.Dev.Clock.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;

namespace AStar.Dev.Clock;

public partial class MainWindow : Window
{
    private AnalogClockControl? _clock;
    private CancellationTokenSource? _resizeCts;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _clock = FindClock();
    }

    protected override void OnResized(WindowResizedEventArgs e)
    {
        base.OnResized(e);

        // Cancel any pending update and schedule a new one
        _resizeCts?.Cancel();
        _resizeCts = new CancellationTokenSource();
        var token = _resizeCts.Token;

        _ = DebouncedUpdateAsync(token);
    }

    private async Task DebouncedUpdateAsync(CancellationToken ct)
    {
        try
        {
            await Task.Delay(200, ct); // 200ms debounce
            if (ct.IsCancellationRequested) return;

            await Dispatcher.UIThread.InvokeAsync(UpdateTitle());
        }
        catch (TaskCanceledException)
        {
            /* ignore */
        }
    }

    private Action UpdateTitle() => () =>
    {
        Title = ClientSize.Width switch
        {
            < 200 => "Analog Clock",
            >= 200 and < 400 => "AStar — Analog Clock",
            >= 400 and < 800 => "AStar Dev — Analog Clock",
            _ => "AStar Development — Analog Clock"
        };
    };

    private void OnLightModeSelected(object? sender, RoutedEventArgs e) => (Application.Current as App)?.SetTheme(ThemeVariant.Light);

    private void OnDarkModeSelected(object? sender, RoutedEventArgs e) => (Application.Current as App)?.SetTheme(ThemeVariant.Dark);

    private void OnAutoModeSelected(object? sender, RoutedEventArgs e) => (Application.Current as App)?.SetTheme(null);

    private void OnArabicNumeralsSelected(object? sender, RoutedEventArgs e) => _clock?.NumeralStyle = NumeralStyle.Arabic;

    private void OnRomanNumeralsSelected(object? sender, RoutedEventArgs e) => _clock?.NumeralStyle = NumeralStyle.Roman;

    private void OnNoNumeralsSelected(object? sender, RoutedEventArgs e) => _clock?.NumeralStyle = NumeralStyle.None;

    private void OnClose(object? sender, RoutedEventArgs e) => Close();

    private AnalogClockControl? FindClock() => this.FindControl<AnalogClockControl>("Clock");
}