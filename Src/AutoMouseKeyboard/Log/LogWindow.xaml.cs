using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AMK
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();

            Loaded += LogWindow_Loaded;
        }

        private void LogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ALog.OnDebug += (msg) =>
            {
                listLog.Items.Add(msg);
            };
        }
    }
}
