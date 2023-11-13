using System.Windows;
using System.Windows.Input;

namespace Реестр_маневренного_фонда
{
    /// <summary>
    /// Логика взаимодействия для HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void bt_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void bt_MaxWindow_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResize;
            }
            else
            {
                ResizeMode = ResizeMode.NoResize;
                WindowState = WindowState.Maximized;
            }
        }

        private void bt_MinWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}