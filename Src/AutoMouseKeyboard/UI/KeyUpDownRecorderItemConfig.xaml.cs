﻿using AMK.Recorder;
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

            SetKeyButtonCombobox((VirtualKeyCode)this.RecorderKeyItem.VkCode);
        }

        private void SetKeyButtonCombobox(VirtualKeyCode vkCode)
        {
            this.comboKey.SelectedItem = this.comboKey.Items.OfType<KeyItem>().First(p => p.VKKeyCode == vkCode);
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            IKeyRecorderItem keyItem = this.RecorderItem as IKeyRecorderItem;
            VirtualKeyCode vkCode = (this.comboKey.SelectedItem as KeyItem).VKKeyCode;
            keyItem.VkCode = (int)vkCode;
            keyItem.Keyname = vkCode.ToString();

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
