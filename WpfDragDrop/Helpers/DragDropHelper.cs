using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace WpfDragDrop.Helpers
{
    public class DragDropHelper
    {
        public EventHandler DragHandler;

        private ElevatedFileDroper droper;

        public DragDropHelper(Visual visual, FrameworkElement control)
        {
            // 创建拖放处理对象
            droper = new ElevatedFileDroper();
            // 获取宿主窗口的 HwndSource
            var hwndSource = (HwndSource)PresentationSource.FromVisual(visual);
            if (hwndSource != null)
            {
                droper.HwndSource = hwndSource;
                droper.AddHook();  // 添加拖放钩子
            }
            droper.DragDrop += (s, c) =>
            {
                if (control == null)
                {
                    return;
                }
                // 获取控件相对于Window的坐标
                var controlPoint = control.TranslatePoint(new Point(0, 0), Application.Current.MainWindow);
                // 获取当前显示器的 DPI 缩放比例
                PresentationSource source = PresentationSource.FromVisual(control);
                if (source == null)
                {
                    return;
                }
                var dpiScale = source.CompositionTarget.TransformToDevice;
                // 将坐标值进行缩放
                var scaledPoint = new Point(controlPoint.X * dpiScale.M11, controlPoint.Y * dpiScale.M22);
                // 获取控件的实际大小
                var scaledSize = new Size(control.ActualWidth * dpiScale.M11, control.ActualHeight * dpiScale.M22);
                // 检查转换后的点是否在控件的边界内
                if ((droper.DropPoint.X >= scaledPoint.X && droper.DropPoint.X <= scaledPoint.X + scaledSize.Width) &&
                    (droper.DropPoint.Y >= scaledPoint.Y && droper.DropPoint.Y <= scaledPoint.Y + scaledSize.Height))
                {
                    DragHandler?.Invoke(string.Join(Environment.NewLine, droper.DropFilePaths), c);
                }
            };
        }

        /// <summary>
        /// remove droper hook
        /// </summary>
        public void RemoveDroper()
        {
            droper?.RemoveHook();
        }
    }
}
