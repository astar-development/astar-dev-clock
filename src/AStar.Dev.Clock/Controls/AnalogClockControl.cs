using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;

namespace AStar.Dev.Clock.Controls;

public class AnalogClockControl : Control
{
    private readonly DispatcherTimer _timer;

    // Expose a Foreground property so consumers can style the clock hands/ticks
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        AvaloniaProperty.Register<AnalogClockControl, IBrush?>(nameof(Foreground), Brushes.Black);

    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public AnalogClockControl()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) => InvalidateVisual();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (!_timer.IsEnabled) _timer.Start();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (_timer.IsEnabled) _timer.Stop();
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var bounds = Bounds;
        var center = bounds.Center;
        var radius = Math.Min(bounds.Width, bounds.Height) * 0.45;

        // Theme-aware brushes
        var isDark = ActualThemeVariant == ThemeVariant.Dark
                     || Application.Current?.ActualThemeVariant == ThemeVariant.Dark;
        var foreground = Foreground ?? (isDark ? Brushes.White : Brushes.Black);

        var faceBrush = isDark ? new SolidColorBrush(Color.FromUInt32(0xFF22252A)) : new SolidColorBrush(Color.FromUInt32(0xFFFFFFFF));
        var facePen = new Pen(isDark ? new SolidColorBrush(Color.FromUInt32(0xFF444A52)) : new SolidColorBrush(Color.FromUInt32(0xFFCCCCCC)), 2);
        var tickPen = new Pen(foreground, 2);
        var minorTickPen = new Pen(foreground, 1);

        // Draw clock face
        context.DrawEllipse(faceBrush, facePen, center, radius, radius);

        // Draw ticks
        for (var i = 0; i < 60; i++)
        {
            var angle = Math.PI * 2 * (i / 60.0) - Math.PI / 2; // start at 12 o'clock
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);

            var outer = new Point(center.X + cos * radius * 0.95, center.Y + sin * radius * 0.95);
            var innerLen = (i % 5 == 0) ? 0.78 : 0.88; // hour ticks longer
            var inner = new Point(center.X + cos * radius * innerLen, center.Y + sin * radius * innerLen);
            context.DrawLine(i % 5 == 0 ? tickPen : minorTickPen, inner, outer);
        }

        // Current time
        var now = DateTime.Now;
        var sec = now.Second + now.Millisecond / 1000.0;
        var min = now.Minute + sec / 60.0;
        var hour = now.Hour % 12 + min / 60.0;

        // Hands
        DrawHand(context, center, radius * 0.55, hour / 12.0, thickness: 5, brush: foreground);
        DrawHand(context, center, radius * 0.75, min / 60.0, thickness: 3, brush: foreground);
        DrawHand(context, center, radius * 0.85, sec / 60.0, thickness: 1.5, brush: isDark ? Brushes.OrangeRed : Brushes.Crimson);

        // Center cap
        context.DrawGeometry(isDark ? Brushes.OrangeRed : Brushes.Crimson, null,
            new EllipseGeometry(new Rect(center.X - 3, center.Y - 3, 6, 6)));
    }

    private static void DrawHand(DrawingContext ctx, Point center, double length, double unit, double thickness, IBrush brush)
    {
        var angle = unit * Math.PI * 2 - Math.PI / 2;
        var end = new Point(center.X + Math.Cos(angle) * length, center.Y + Math.Sin(angle) * length);
        var pen = new Pen(brush, thickness, lineCap: PenLineCap.Round);
        ctx.DrawLine(pen, center, end);
    }
}
