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

        public List<KeyPressData> KeyPressRecorders { get; set; } = new List<KeyPressData>();

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

            List<KeyItem> keyComboItems = new List<KeyItem>();
            foreach (var key in AUtil.GetVirtualKeyCodes())
                keyComboItems.Add(new KeyItem(key));
            this.comboBoxDataGridKey.ItemsSource = keyComboItems;

            this.Loaded += KeyPressRecorderItemConfig_Loaded;
        }

        private void KeyPressRecorderItemConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.RecorderItem.Recorder.ToString() + " Configuration";

            KeyPressRecorderItem rootItem = this.RecorderItem as KeyPressRecorderItem;
            int index = 1;
            this.KeyPressRecorders.Clear();
            this.KeyPressRecorders.Add(new KeyPressData() { Index = index++, Key = new KeyItem((VirtualKeyCode)rootItem.VkCode) });
            foreach(var key in rootItem.ChildItems)
            {
                KeyPressRecorderItem item = key as KeyPressRecorderItem;
                this.KeyPressRecorders.Add(new KeyPressData() { Index = index++, Key = new KeyItem((VirtualKeyCode)item.VkCode) });
            }

            this.DataContext = this;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            KeyPressRecorderItem rootItem = this.RecorderItem as KeyPressRecorderItem;
            List<KeyPressData> gridItems = this.dataGridKeys.ItemsSource as List<KeyPressData>;

            KeyPressData keyData = gridItems[0];
            rootItem.VkCode = (int)keyData.Key.VKKeyCode;
            rootItem.Keyname = AUtil.ConvertVKeyToString(keyData.Key.VKKeyCode);

            rootItem.ChildItems.Clear();
            for (int i=1; i<gridItems.Count; i++)
            {
                keyData = gridItems[i];
                rootItem.ChildItems.Add(new KeyPressRecorderItem() { 
                    Time = rootItem.Time + TimeSpan.FromSeconds(AMKRecorder.MinimumTimeSpan * i),
                    VkCode = (int)keyData.Key.VKKeyCode,
                    Keyname = AUtil.ConvertVKeyToString(keyData.Key.VKKeyCode),
                });
            }

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void buttonPlus_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = 0;
            VirtualKeyCode newVkCode = VirtualKeyCode.SPACE;
            if (this.dataGridKeys.SelectedItem != null && 
                this.dataGridKeys.SelectedItem as KeyPressData != null)
            {
                KeyPressData keyPress = this.dataGridKeys.SelectedItem as KeyPressData;
                newVkCode = keyPress.Key.VKKeyCode;
                newIndex = this.KeyPressRecorders.IndexOf(keyPress);
            }

            this.KeyPressRecorders.Insert(newIndex, new KeyPressData()
            {
                Key = new KeyItem(newVkCode),
            });

            UpdateKeyPressRecorderDataGrid();
        }

        private void buttonMinus_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.dataGridKeys.SelectedItems)
            {
                this.KeyPressRecorders.Remove(item as KeyPressData);
            }

            UpdateKeyPressRecorderDataGrid();
        }

        private void UpdateKeyPressRecorderDataGrid()
        {
            int index = 1;
            foreach (var item in this.KeyPressRecorders)
            {
                (item as KeyPressData).Index = index++;
            }

            this.DataContext = null;
            this.DataContext = this;
        }
    }
}
