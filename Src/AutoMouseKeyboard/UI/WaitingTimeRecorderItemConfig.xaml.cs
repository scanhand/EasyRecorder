using AMK.Recorder;
using MahApps.Metro.Controls;
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

namespace AMK.UI
{
    /// <summary>
    /// Interaction logic for WaitingTimeRecorderItemConfig.xaml
    /// </summary>
    public partial class WaitingTimeRecorderItemConfig : MetroWindow, IRecorderItemConfig
    {
        public IRecorderItem RecorderItem { get; set; } 

        public WaitingTimeRecorderItemConfig()
        {
            InitializeComponent();

            this.KeyDown += (e, k) =>
            {
                if(k.Key == Key.Enter)
                    buttonOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };

            this.Loaded += WaitingTimeRecorderItemConfig_Loaded;
        }

        private void WaitingTimeRecorderItemConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBoxWaitingTime.Focus();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
