using AMK.Global;
using AMK.Recorder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMK.UI
{
    /// <summary>
    /// Interaction logic for MainToolbar.xaml
    /// </summary>
    public partial class MainToolbar : UserControl
    {
        private AMKRecorder Recorder { get; set; } = null;

        public MainToolbar()
        {
            InitializeComponent();
        }

        public void Initialize(AMKRecorder recorder)
        {
            ALog.Debug("");
            this.Recorder = recorder;

            this.Recorder.OnChangedState += (state) =>
            {
                this.InvokeIfRequired(() =>
                {
                    EnableToolbarButton(state);
                });
            };
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            bool isReset = this.Recorder.State != AMKState.PlayingPause;
            this.Recorder.StartPlaying(isReset);
        }

        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.Recorder.PauseAll();
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.Recorder.StopAll();
        }

        private void buttonRecord_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.Recorder.StartRecordingWithConfirm();
        }

        private void EnableToolbarButton(AMKState state)
        {
            if (state == AMKState.Playing)
            {
                buttonPlay.IsEnabled = false;
                buttonPause.IsEnabled = true;
                buttonStop.IsEnabled = true;
                buttonRecord.IsEnabled = false;
            }
            else if (state == AMKState.Recording)
            {
                buttonPlay.IsEnabled = false;
                buttonPause.IsEnabled = true;
                buttonStop.IsEnabled = true;
                buttonRecord.IsEnabled = false;
            }
            else if(state == AMKState.Stop || state == AMKState.PlayDone)
            {
                buttonPlay.IsEnabled = true;
                buttonPause.IsEnabled = false;
                buttonStop.IsEnabled = false;
                buttonRecord.IsEnabled = true;
            }
            else if (state == AMKState.PlayingPause)
            {
                buttonPlay.IsEnabled = true;
                buttonPause.IsEnabled = false;
                buttonStop.IsEnabled = true;
                buttonRecord.IsEnabled = false;
            }
            else if (state == AMKState.RecordingPause)
            {
                buttonPlay.IsEnabled = false;
                buttonPause.IsEnabled = false;
                buttonStop.IsEnabled = true;
                buttonRecord.IsEnabled = true;
            }
        }
    }
}
