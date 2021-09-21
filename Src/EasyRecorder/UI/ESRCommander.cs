using ESR.Global;
using ESR.Recorder;
using EventHook;
using System;
using WindowsInput.Native;

namespace ESR.UI
{
    public class ESRCommander
    {
        public VirtualKeyCode RecordingVKeyCode = VirtualKeyCode.F9;

        public VirtualKeyCode PlayingVKeyCode = VirtualKeyCode.F10;

        public VirtualKeyCode StopVKeyCode = VirtualKeyCode.ESCAPE;

        public VirtualKeyCode DragClickStartVKKeyCode = VirtualKeyCode.F8;

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

        private ESRRecorder Recorder
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
            else if (e.KeyData.VkCode == (int)this.StopVKeyCode)
            {
                if (e.KeyData.EventType != KeyEvent.up)
                    OnStop();
                return true;
            }
            else if (e.KeyData.VkCode == (int)this.DragClickStartVKKeyCode)
            {
                if (e.KeyData.EventType != KeyEvent.up)
                    OnDragClickStart();
                return true;
            }
            return false;
        }

        private void OnRecording()
        {
            ALog.Debug("State={0}", this.Recorder.State);
            if (AUtil.IsStopPause(this.Recorder.State))
            {
                bool isReset = this.Recorder.State != ESRState.RecordingPause;
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
                bool isReset = this.Recorder.State != ESRState.PlayingPause;
                this.Recorder.StartPlaying(isReset);
            }
            else
            {
                this.Recorder.PauseAll();
            }
        }

        private void OnStop()
        {
            ALog.Debug("State={0}", this.Recorder.State);
            this.Recorder.PauseAll();
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

        private void OnDragClickStart()
        {
            ALog.Debug("OnDragClickStart={0}", this.Recorder.State);
            this.Recorder.StartDragClick();
        }
    }
}
