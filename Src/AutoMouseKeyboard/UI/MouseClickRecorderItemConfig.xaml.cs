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
    /// Interaction logic for MouseClickRecorderItemConfig.xaml
    /// </summary>
    public partial class MouseClickRecorderItemConfig : MetroWindow, IRecorderItemConfig
    {
        public IRecorderItem RecorderItem { get; set; } 

        public MouseClickRecorderItemConfig()
        {
            InitializeComponent();

            this.KeyDown += (e, k) =>
            {
                if(k.Key == Key.Enter)
                    buttonOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };

            this.Loaded += MouseClickRecorderItemConfig_Loaded;
        }

        private void MouseClickRecorderItemConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.RecorderItem.Recorder.ToString() + " Configuration";

            this.textBoxX.Text = string.Format("{0}", (int)this.RecorderItem.Point.X);
            this.textBoxY.Text = string.Format("{0}", (int)this.RecorderItem.Point.Y);
            this.textBoxX.Focus();
            this.textBoxX.SelectAll();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            int inputX = 0;
            int inputY = 0;
            try
            {
                inputX = int.Parse(this.textBoxX.Text);
                inputY = int.Parse(this.textBoxY.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.RecorderItem.Point = new Point(inputX, inputY);

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
