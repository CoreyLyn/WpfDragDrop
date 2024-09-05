using System.Windows.Interop;

namespace WpfDragDrop.Helpers
{
    public partial class ElevatedFileDroper
    {
        public event EventHandler DragDrop;
        public string[] DropFilePaths { get; private set; }
        public POINT DropPoint { get; private set; }
        public HwndSource HwndSource { get; set; }

        public void AddHook()
        {
            this.RemoveHook();
            this.HwndSource.AddHook(WndProc);
            IntPtr handle = this.HwndSource.Handle;
            if (IsUserAnAdmin()) RevokeDragDrop(handle);
            DragAcceptFiles(handle, true);
            ChangeMessageFilter(handle);
        }

        public void RemoveHook()
        {
            this.HwndSource.RemoveHook(WndProc);
            DragAcceptFiles(this.HwndSource.Handle, false);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (TryGetDropInfo(msg, wParam, out string[] filePaths, out POINT point))
            {
                DropPoint = point;
                DropFilePaths = filePaths;
                DragDrop?.Invoke(this, null);
                handled = true;
            }
            return IntPtr.Zero;
        }
    }
}
