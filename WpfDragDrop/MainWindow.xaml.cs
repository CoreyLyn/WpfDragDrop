using System.Windows;
using WpfDragDrop.Helpers;

namespace WpfDragDrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dragDropHelper = new DragDropHelper(this, grid);
            dragDropHelper.DragHandler += (s, args) =>
            {
                MessageBox.Show(s?.ToString());
            };
        }
    }
}