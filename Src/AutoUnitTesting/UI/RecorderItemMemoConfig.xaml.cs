using AUT.Recorder;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AUT.UI
{
    /// <summary>
    /// Interaction logic for RecorderItemMemoConfig.xaml
    /// </summary>
    public partial class RecorderItemMemoConfig : MetroWindow, IRecorderItemConfig
    {
        public IRecorderItem RecorderItem { get; set; }

        public RecorderItemMemoConfig()
        {
            InitializeComponent();

            this.KeyDown += (e, k) =>
            {
                if (k.Key == Key.Enter)
                    buttonOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };

            this.Loaded += RecorderItemMemoConfig_Loaded;
        }

        private void RecorderItemMemoConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.RecorderItem.Recorder.ToString() + " Configuration";

            this.textBoxMemo.Text = this.RecorderItem.Memo;
            this.textBoxMemo.Focus();
            this.textBoxMemo.SelectAll();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.RecorderItem.Memo = this.textBoxMemo.Text;

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
