using AMK.Global;
using AMK.Recorder;
using MahApps.Metro.Controls;
using System;
using System.Linq;
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

        private IKeyRecorderItem RecorderKeyItem
        {
            get
            {
                return this.RecorderItem as IKeyRecorderItem;
            }
        }

        public KeyUpDownRecorderItemConfig()
        {
            InitializeComponent();

            this.comboUpDownButton.Items.Add(new UpDownItem(Dir.Up));
            this.comboUpDownButton.Items.Add(new UpDownItem(Dir.Down));

            foreach (var key in AUtil.GetVirtualKeyCodes())
                this.comboKey.Items.Add(new KeyItem(key));

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

            SetUpDownButtonCombobox(this.RecorderItem.Dir);
            SetKeyButtonCombobox((VirtualKeyCode)this.RecorderKeyItem.VkCode);
        }

        private void SetUpDownButtonCombobox(Dir dir)
        {
            this.comboUpDownButton.SelectedItem = this.comboUpDownButton.Items.OfType<UpDownItem>().First(f => f.Dir == dir);
        }

        private void SetKeyButtonCombobox(VirtualKeyCode vkCode)
        {
            this.comboKey.SelectedItem = this.comboKey.Items.OfType<KeyItem>().FirstOrDefault(p => p.VKKeyCode == vkCode);
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        { 
            this.RecorderItem.Dir = (this.comboUpDownButton.SelectedItem as UpDownItem).Dir;
            IKeyRecorderItem keyItem = this.RecorderItem as IKeyRecorderItem;
            VirtualKeyCode vkCode = (this.comboKey.SelectedItem as KeyItem).VKKeyCode;
            keyItem.VkCode = (int)vkCode;
            keyItem.Keyname = AUtil.ToVKeyToString(vkCode);

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
