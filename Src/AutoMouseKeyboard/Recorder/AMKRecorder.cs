using EventHook;
using EventHook.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMK
{
    public class AMKRecorder
    {
        private List<IRecorderItem> Items = new List<IRecorderItem>();

        private IRecorderItem CurrentRecorder = null;

        private bool IsWaitingTimeEvent = false;

        private float CurrentWaitingTimeSec = 0;

        public float WaitingTimeSec = 0.5f;

        private Thread ThreadRecording = null;

        private bool IsThreadRecording = false;

        public Action<IRecorderItem> OnAddItem = null;

        public Action<IRecorderItem> OnUpdateItem = null;

        public AMKRecorder()
        {
        }

        public void AddItem(IRecorderItem item)
        {
            this.Items.Add(item);
            if (OnAddItem != null)
                OnAddItem(item);
        }

        public void UpdateItem(IRecorderItem item)
        {
            if (OnUpdateItem != null)
                OnUpdateItem(item);
        }

        public void Start()
        {
            if (this.ThreadRecording != null)
                return;

            this.ThreadRecording = new Thread(() =>
            {
                const float waitTime = 0.1f; // 100 mesc
                while(this.IsThreadRecording)
                {
                    if(this.CurrentWaitingTimeSec >= this.WaitingTimeSec)
                        AddWaitingRecorderItem(this.CurrentWaitingTimeSec);

                    Thread.Sleep((int)(waitTime*1000));
                    this.CurrentWaitingTimeSec += waitTime;
                }
            });

            this.IsThreadRecording = true;
            this.ThreadRecording.Start();

            ALog.Debug("Recorder.Start()");
        }

        public void Stop()
        {
            if (this.ThreadRecording == null)
                return;

            this.IsThreadRecording = false;
            while(true)
            {
                if (!this.ThreadRecording.IsAlive)
                    break;
            }
            this.ThreadRecording = null;
            ALog.Debug("Recorder.Stop()");
        }

        public void Add(ApplicationEventArgs e)
        {
            ALog.Debug("AMKRecorder::Items.Count={0}", this.Items.Count);

            this.CurrentWaitingTimeSec = 0;
            IRecorderItem newRecorder = null;

            newRecorder = new ApplicationRecorderItem()
            {
                ApplicationData = e.ApplicationData,
                Event = e.Event,
            };

            AddItem(newRecorder);
            this.CurrentRecorder = newRecorder;
        }

        public void Add(MouseEventArgs e)
        {
            ALog.Debug("AMKRecorder::Items.Count={0}", this.Items.Count);

            this.CurrentWaitingTimeSec = 0;
            IRecorderItem newRecorder = null;

            if (e.Message == MouseMessages.WM_WHEELBUTTONUP ||
                e.Message == MouseMessages.WM_WHEELBUTTONDOWN ||
                e.Message == MouseMessages.WM_MOUSEWHEEL)
            {
                newRecorder = new MouseWheelRecorderItem()
                {
                    Dir = e.Message == MouseMessages.WM_WHEELBUTTONUP ? Dir.Up : Dir.Down,
                    Point = e.Point,
                    MouseData = e.MouseData,
                };

                AddItem(newRecorder);
            }
            else if(e.Message == MouseMessages.WM_LBUTTONUP ||
                    e.Message == MouseMessages.WM_LBUTTONDOWN ||
                    e.Message == MouseMessages.WM_RBUTTONUP ||
                    e.Message == MouseMessages.WM_RBUTTONDOWN )
            {
                newRecorder = new MouseClickRecorderItem()
                {
                    Dir = e.Message == MouseMessages.WM_LBUTTONUP ? Dir.Up : Dir.Down,
                    LR = (e.Message == MouseMessages.WM_LBUTTONUP || e.Message == MouseMessages.WM_LBUTTONDOWN) ? LR.Left : LR.Right,
                    Point = e.Point,
                    MouseData = e.MouseData,
                };

                AddItem(newRecorder);
            }
            else if(e.Message == MouseMessages.WM_MOUSEMOVE)
            {
                newRecorder = new MouseMoveRecorderItem()
                {
                    Point = e.Point,
                    MouseData = e.MouseData,
                };

                if (CurrentRecorder?.IsEqualType(newRecorder) == true)
                    CurrentRecorder.ChildItems.Add(newRecorder);
                else
                    AddItem(newRecorder);
            }

            this.CurrentRecorder = newRecorder;

        }

        public void Add(KeyInputEventArgs e)
        {
            ALog.Debug("AMKRecorder::Items.Count={0}", this.Items.Count);

            this.CurrentWaitingTimeSec = 0;
            IRecorderItem newRecorder = null;
            if (e.KeyData.EventType == KeyEvent.up)
            {
                newRecorder = new KeyUpRecorderItem()
                {
                    Keyname = e.KeyData.Keyname,
                    UnicodeCharacter = e.KeyData.UnicodeCharacter,
                };
            }
            else
            {
                newRecorder = new KeyDownRecorderItem()
                {
                    Keyname = e.KeyData.Keyname,
                    UnicodeCharacter = e.KeyData.UnicodeCharacter,
                };
            }

            AddItem(newRecorder);
            this.CurrentRecorder = newRecorder;
        }

        private void AddWaitingRecorderItem(float waitingTimeSec)
        {
            IRecorderItem newRecorder = null;

            newRecorder = new WaitTimeRecorderItem()
            {
                WaitingTimeSec = waitingTimeSec,
            };

            if (this.CurrentRecorder?.IsEqualType(newRecorder) == true)
            {
                var item = this.CurrentRecorder as WaitTimeRecorderItem;
                item.WaitingTimeSec = waitingTimeSec;
                UpdateItem(item);
                ALog.Debug("Accumulated time {0} in currentRecorder", item.WaitingTimeSec);
                return;
            }

            AddItem(newRecorder);
            this.CurrentRecorder = newRecorder;
            ALog.Debug("Add Waiting Event!");
        }
    }
}
