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

        public VirtualKeyCode PlayingVKeyCode = VirtualKeyCode.F5;

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
            if (AUtil.IsStop(this.Recorder.State))
                this.MainWindow.StartRecording(); 
            else
                this.MainWindow.Stop();
        }

        private void OnPlaying()
        {
            if (AUtil.IsStop(this.Recorder.State))
                this.MainWindow.StartPlaying(); 
            else
                this.MainWindow.Stop();
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
