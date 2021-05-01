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
    /// Interaction logic for MouseUpDownRecorderItemConfig.xaml
    /// </summary>
    public partial class MouseUpDownRecorderItemConfig : MetroWindow, IRecorderItemConfig
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

        class ButtonItem
        {
            public ButtonType LR { get; set; }
            public ButtonItem(ButtonType lr)
            {
                this.LR = lr;
            }

            public override string ToString()
            {
                return this.LR.ToString();
            }
        }

        #endregion

        public IRecorderItem RecorderItem { get; set; }

        public MouseUpDownRecorderItemConfig()
        {
            InitializeComponent();

            this.comboUpDownButton.Items.Add(new UpDownItem(Dir.Up));
            this.comboUpDownButton.Items.Add(new UpDownItem(Dir.Down));

            this.comboLRButton.Items.Add(new ButtonItem(ButtonType.Left));
            this.comboLRButton.Items.Add(new ButtonItem(ButtonType.Right));
            this.comboLRButton.Items.Add(new ButtonItem(ButtonType.Wheel));

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
            SetMouseButtonCombobox(this.RecorderItem.Button);
            this.textBoxX.Text = string.Format("{0}", (int)this.RecorderItem.Point.X);
            this.textBoxY.Text = string.Format("{0}", (int)this.RecorderItem.Point.Y);
            this.textBoxX.Focus();
            this.textBoxX.SelectAll();
        }

        private void SetUpDownButtonCombobox(Dir dir)
        {
            this.comboUpDownButton.SelectedItem = this.comboUpDownButton.Items.OfType<UpDownItem>().First(f => f.Dir == dir);
        }

        private void SetMouseButtonCombobox(ButtonType lr)
        {
            if (lr == ButtonType.None)
                lr = ButtonType.Left;  //default

            this.comboLRButton.SelectedItem = this.comboLRButton.Items.OfType<ButtonItem>().First(f => f.LR == lr);
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
            this.RecorderItem.Button = (this.comboLRButton.SelectedItem as ButtonItem).LR;
            this.RecorderItem.Dir = (this.comboUpDownButton.SelectedItem as UpDownItem).Dir;
            //TODO: RecorderType

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
