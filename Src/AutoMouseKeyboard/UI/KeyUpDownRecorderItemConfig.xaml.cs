using AMK.Recorder;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput.Native;

namespace AMK.UI
{
    /// <summary>
    /// Interaction logic for KeyUpDownRecorderItemConfig.xaml
    /// </summary>
    public partial class KeyUpDownRecorderItemConfig : MetroWindow, IRecorderItemConfig
    {
        #region inner

        class KeyItem
        {
            public VirtualKeyCode VKKeyCode { get; set; }
            public KeyItem(VirtualKeyCode code)
            {
                this.VKKeyCode = code;
            }

            public override string ToString()
            {
                return this.VKKeyCode.ToString();
            }
        }

        #endregion

        public IRecorderItem RecorderItem { get; set; }

        public KeyUpDownRecorderItemConfig()
        {
            InitializeComponent();

            foreach (var key in Enum.GetValues(typeof(VirtualKeyCode)))
                this.comboKey.Items.Add(new KeyItem((VirtualKeyCode)key));

            this.KeyDown += (e, k) =>
            {
                if (k.Key == Key.Enter)
                    buttonOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };

            this.Loaded += KeyUpDownRecorderItemConfig_Loaded;
        }

        private void KeyUpDownRecorderItemConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.RecorderItem.Recorder.ToString() + " Configuration";
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
