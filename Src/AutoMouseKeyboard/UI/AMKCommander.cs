using AMK.Global;
using AMK.Recorder;
using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace AMK.UI
{
    public class AMKCommander
    {
        public VirtualKeyCode RecordingVKeyCode = VirtualKeyCode.F4;

        public VirtualKeyCode PlayingVKeyCode = VirtualKeyCode.F3;

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
            if(e.KeyData.VkCode == (int)this.RecordingVKeyCode)
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
            if (AUtil.IsStopPause(this.Recorder.State))
            {
                bool isReset = this.Recorder.State != AMKState.RecordingPause;
                this.MainWindow.StartRecording(isReset);
            }
            else
            {
                this.MainWindow.Stop();
                this.Recorder.State = AMKState.RecordingPause;
            }
        }

        private void OnPlaying()
        {
            if (AUtil.IsStopPause(this.Recorder.State))
            {
                bool isReset = this.Recorder.State != AMKState.PlayingPause;
                this.MainWindow.StartPlaying(isReset);
            }
            else
            {
                this.MainWindow.Stop();
                this.Recorder.State = AMKState.PlayingPause;
            }
        }

        private void OnPushKey(VirtualKeyCode key)
        {
            if(key == VirtualKeyCode.DELETE && AUtil.IsStop(this.Recorder.State))
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
