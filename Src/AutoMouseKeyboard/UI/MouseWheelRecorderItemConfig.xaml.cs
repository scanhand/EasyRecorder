using AMK.Global;
using AMK.Recorder;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AMK.UI
{
    /// <summary>
    /// Interaction logic for MouseWheelRecorderItemConfig.xaml
    /// </summary>
    public partial class MouseWheelRecorderItemConfig : MetroWindow, IRecorderItemConfig
    {
        #region inner

        class UpDownItem
        {
            public Dir Dir { get; set; }
            public UpDownItem(Dir dir)
            {
                this.Dir = dir;
            }

            public override string ToString()
            {
                return this.Dir.ToString();
            }
        }

        #endregion

        public IRecorderItem RecorderItem { get; set; }

        public MouseWheelRecorderItemConfig()
        {
            InitializeComponent();

            this.comboUpDownButton.Items.Add(new UpDownItem(Dir.Up));
            this.comboUpDownButton.Items.Add(new UpDownItem(Dir.Down));

            this.KeyDown += (e, k) =>
            {
                if (k.Key == Key.Enter)
                    buttonOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };

            this.Loaded += MouseUpDownRecorderItemConfig_Loaded;
        }

        private void MouseUpDownRecorderItemConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.RecorderItem.Recorder.ToString() + " Configuration";

            SetUpDownButtonCombobox(this.RecorderItem.Dir);

            this.textBoxWheelCount.Text = string.Format("{0}", this.RecorderItem.ChildItems.Count + 1);
            this.textBoxWheelCount.Focus();
            this.textBoxWheelCount.SelectAll();
        }

        private void SetUpDownButtonCombobox(Dir dir)
        {
            this.comboUpDownButton.SelectedItem = this.comboUpDownButton.Items.OfType<UpDownItem>().First(f => f.Dir == dir);
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            int wheelCount = 0;
            try
            {
                wheelCount = int.Parse(this.textBoxWheelCount.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (wheelCount < 1)
            {
                MessageBox.Show("Wheel Count must more than 1.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Dir dir = (this.comboUpDownButton.SelectedItem as UpDownItem).Dir;
            int mouseData = dir == Dir.Down ? MouseWheelRecorderItem.UpMouseData : MouseWheelRecorderItem.DownMouseData;

            this.RecorderItem.Dir = dir;
            this.RecorderItem.ChildItems.Clear();
            for (int i = 1; i < wheelCount; i++)
            {
                this.RecorderItem.ChildItems.Add(new MouseWheelRecorderItem()
                {
                    Dir = dir,
                    MouseData = mouseData,
                    Time = this.RecorderItem.Time + TimeSpan.FromSeconds(AMKRecorder.MinimumTimeSpan * i),
                });
            }

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
