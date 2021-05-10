using AMK.Global;
using AMK.Recorder;
using EventHook;
using System;
using WindowsInput.Native;

namespace AMK.UI
{
    public class AMKCommander
    {
        public VirtualKeyCode RecordingVKeyCode = VirtualKeyCode.F9;

        public VirtualKeyCode PlayingVKeyCode = VirtualKeyCode.F10;

        public MainWindow MainWindow
        {
            get
            {
                return GM.Instance.MainWindow;
            }
        }

        private RecorderItemView RecorderView
        {
            get
            {
                return this.MainWindow.RecorderView;
            }
        }

        private AMKRecorder Recorder
        {
            get
            {
                return this.MainWindow.Recorder;
            }
        }

        public bool ProcessKey(KeyInputEventArgs e)
        {
            if (e.KeyData.VkCode == (int)this.RecordingVKeyCode)
            {
                if (e.KeyData.EventType != KeyEvent.up)
                    OnRecording();
                return true;
            }
            else if (e.KeyData.VkCode == (int)this.PlayingVKeyCode)
            {
                if (e.KeyData.EventType != KeyEvent.up)
                    OnPlaying();
                return true;
            }
            else if (e.KeyData.VkCode == (int)VirtualKeyCode.DELETE)
            {
                if (e.KeyData.EventType != KeyEvent.up)
                    OnPushKey((VirtualKeyCode)e.KeyData.VkCode);
                return true;
            }
            return false;
        }

        private void OnRecording()
        {
            ALog.Debug("State={0}", this.Recorder.State);
            if (AUtil.IsStopPause(this.Recorder.State))
            {
                bool isReset = this.Recorder.State != AMKState.RecordingPause;
                this.Recorder.StartRecording();
            }
            else
            {
                this.Recorder.PauseAll();
            }
        }

        private void OnPlaying()
        {
            ALog.Debug("State={0}", this.Recorder.State);
            if (AUtil.IsStopPause(this.Recorder.State))
            {
                bool isReset = this.Recorder.State != AMKState.PlayingPause;
                this.Recorder.StartPlaying(isReset);
            }
            else
            {
                this.Recorder.PauseAll();
            }
        }

        private void OnPushKey(VirtualKeyCode key)
        {
            ALog.Debug("VirtualKeyCode={0}", key.ToString());
            if (key == VirtualKeyCode.DELETE && AUtil.IsStop(this.Recorder.State))
            {
                DeleteKey();
            }
        }

        private void DeleteKey()
        {
            this.MainWindow.InvokeIfRequired(() =>
            {
                this.RecorderView.DeleteSelectedItems();
            });
        }
    }
}
