using ESR.Global;
using ESR.Recorder;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ESR.UI
{
    /// <summary>
    /// Interaction logic for MainToolbar.xaml
    /// </summary>
    public partial class MainToolbar : UserControl
    {
        private ESRRecorder Recorder { get; set; } = null;

        public MainToolbar()
        {
            InitializeComponent();
        }

        public void Initialize(ESRRecorder recorder)
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
            bool isReset = this.Recorder.State != ESRState.PlayingPause;
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

        private void EnableToolbarButton(ESRState state)
        {
            if (state == ESRState.Playing)
            {
                buttonPlay.IsEnabled = false;
                buttonPause.IsEnabled = true;
                buttonStop.IsEnabled = true;
                buttonRecord.IsEnabled = false;
            }
            else if (state == ESRState.Recording)
            {
                buttonPlay.IsEnabled = false;
                buttonPause.IsEnabled = true;
                buttonStop.IsEnabled = true;
                buttonRecord.IsEnabled = false;
            }
            else if (state == ESRState.Stop || state == ESRState.PlayDone)
            {
                buttonPlay.IsEnabled = true;
                buttonPause.IsEnabled = false;
                buttonStop.IsEnabled = false;
                buttonRecord.IsEnabled = true;
            }
            else if (state == ESRState.PlayingPause)
            {
                buttonPlay.IsEnabled = true;
                buttonPause.IsEnabled = false;
                buttonStop.IsEnabled = true;
                buttonRecord.IsEnabled = false;
            }
            else if (state == ESRState.RecordingPause)
            {
                buttonPlay.IsEnabled = false;
                buttonPause.IsEnabled = false;
                buttonStop.IsEnabled = true;
                buttonRecord.IsEnabled = true;
            }
        }
    }
}
