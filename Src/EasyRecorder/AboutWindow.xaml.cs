using ESR.Global;
using System.Windows;

namespace ESR
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public string Version { get; set; } = "Ver. " + AConst.Version;

        public AboutWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void Hyperlink_icons8_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.icons8.com");
        }
    }
}
