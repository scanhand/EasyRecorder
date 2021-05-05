using AMK.Global;
using AMK.Recorder;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput.Native;

namespace AMK.UI
{
    /// <summary>
    /// Interaction logic for KeyPressRecorderItemConfig.xaml
    /// </summary>
    public partial class KeyPressRecorderItemConfig : MetroWindow, IRecorderItemConfig
    {
        #region inner

        #endregion

        public IRecorderItem RecorderItem { get; set; }

        private IKeyRecorderItem RecorderKeyItem
        {
            get
            {
                return this.RecorderItem as IKeyRecorderItem;
            }
        }

        public KeyPressRecorderItemConfig()
        {
            InitializeComponent();

            this.KeyDown += (e, k) =>
            {
                if (k.Key == System.Windows.Input.Key.Enter)
                    buttonOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };

            this.Loaded += KeyPressRecorderItemConfig_Loaded;
        }

        private void KeyPressRecorderItemConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.RecorderItem.Recorder.ToString() + " Configuration";

            List<KeyItem> keyComboItems = new List<KeyItem>();
            foreach (var key in AUtil.GetVirtualKeyCodes())
                keyComboItems.Add(new KeyItem(key));
            comboBoxDataGridKey.ItemsSource = keyComboItems;

            dataKeys.ItemsSource = new List<KeyPressData>()
            {
                new KeyPressData() { Index=0, Key=new KeyItem(VirtualKeyCode.SPACE),},
                new KeyPressData() { Index=1, Key=new KeyItem(VirtualKeyCode.DOWN),},
                new KeyPressData() { Index=2, Key=new KeyItem(VirtualKeyCode.VK_0),},
            };
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
