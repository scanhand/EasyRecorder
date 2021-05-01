using System.Windows;

namespace AMK
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_icons8_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.icons8.com");
        }
    }
}
