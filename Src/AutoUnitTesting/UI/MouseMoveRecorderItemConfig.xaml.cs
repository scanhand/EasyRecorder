using AUT.Global;
using AUT.Recorder;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput.Native;

namespace AUT.UI
{
    /// <summary>
    /// Interaction logic for MouseMoveRecorderItemConfig.xaml
    /// </summary>
    public partial class MouseMoveRecorderItemConfig : MetroWindow, IRecorderItemConfig
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

        public MouseMoveRecorderItemConfig()
        {
            InitializeComponent();

            this.KeyDown += (e, k) =>
            {
                if (k.Key == Key.Enter)
                    buttonOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };

            this.Loaded += MouseMoveRecorderItemConfig_Loaded;
        }

        private void MouseMoveRecorderItemConfig_Loaded(object sender, RoutedEventArgs e)
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
