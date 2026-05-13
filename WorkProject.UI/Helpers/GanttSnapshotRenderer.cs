using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WorkProject.UI.Helpers
{
    public static class GanttSnapshotRenderer
    {
        public static byte[] RenderToPng(FrameworkElement element)
        {
            if (element == null || element.ActualWidth <= 0 || element.ActualHeight <= 0)
                return null;

            element.Measure(new Size(element.ActualWidth, element.ActualHeight));
            element.Arrange(new Rect(new Size(element.ActualWidth, element.ActualHeight)));

            var bitmap = new RenderTargetBitmap(
                (int)element.ActualWidth,
                (int)element.ActualHeight,
                96, 96, PixelFormats.Pbgra32);

            var visual = new DrawingVisual();
            using (var ctx = visual.RenderOpen())
            {
                var brush = new VisualBrush(element);
                ctx.DrawRectangle(brush, null,
                    new Rect(new Point(0, 0), new Size(element.ActualWidth, element.ActualHeight)));
            }
            bitmap.Render(visual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
