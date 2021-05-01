using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace AMK
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : MetroWindow
    {
        public bool IsDestoryWindow { get; set; } = false;

        public LogWindow()
        {
            InitializeComponent();

            this.Loaded += LogWindow_Loaded;
            this.Closing += LogWindow_Closing;
        }

        private void LogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ALog.OnDebug += (msg) =>
            {
                this.InvokeIfRequired(() =>
                {
                    this.listLog.Items.Add(msg);
                    this.listLog.SelectedIndex = this.listLog.Items.Count - 1;
                    this.listLog.ScrollIntoView(this.listLog.SelectedItem);
                });
            };
        }

        private void LogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsDestoryWindow)
                return;

            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

    }
}
