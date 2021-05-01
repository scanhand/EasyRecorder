using AMK.Recorder;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                if (k.Key == Key.Enter)
                    buttonOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };

            this.Loaded += WaitingTimeRecorderItemConfig_Loaded;
        }

        private void WaitingTimeRecorderItemConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.RecorderItem.Recorder.ToString() + " Configuration";

            this.textBoxWaitingTime.Text = string.Format("{0:F2}", this.RecorderItem.TotalTimeDurationSec);
            this.textBoxWaitingTime.Focus();
            this.textBoxWaitingTime.SelectAll();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            double totalWaitnigTime = 0;
            try
            {
                totalWaitnigTime = double.Parse(this.textBoxWaitingTime.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (totalWaitnigTime < 0)
            {
                MessageBox.Show("The Waiting time must be more than 0 seconds.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IWaitRecorderItem waitItem = this.RecorderItem as IWaitRecorderItem;
            waitItem.WaitingTimeSec = 0;
            this.RecorderItem.ChildItems.Clear();
            this.RecorderItem.ChildItems.Add(new WaitTimeRecorderItem()
            {
                Time = this.RecorderItem.Time + TimeSpan.FromSeconds(totalWaitnigTime),
            });

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
