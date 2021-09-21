using System.Windows;

namespace ESR
{
    /// <summary>
    /// Interaction logic for PreferenceWindow.xaml
    /// </summary>
    public partial class PreferenceWindow : Window
    {
        public PreferenceWindow()
        {
            InitializeComponent();

            this.Loaded += PreferenceWindow_Loaded;
        }

        private void PreferenceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;

            //TOOD: Added preference by RecorderPreference
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            //TOOD: Update preference to RecorderPreference

            this.DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
