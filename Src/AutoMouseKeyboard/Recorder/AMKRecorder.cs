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

        private IRecorderItem CurrentKeyRecorder = null;

        private IRecorderItem CurrentMouseRecorder = null;

        private bool IsWaitingTimeEvent = false;

        private float CurrentWaitingTimeSec = 0;

        //500 msec
        public float WaitingTimeSec = 0.5f;

        public float KeyPressIntervalTimeSec = 0.2f;

        private Thread ThreadRecording = null;

        private bool IsThreadRecording = false;

        public Action<IRecorderItem> OnAddItem = null;

        public Action<IRecorderItem> OnUpdateItem = null;

        public Action<IRecorderItem, IRecorderItem> OnReplaceItem = null;

        public AMKRecorder()
        {
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
            }
            else if(e.Message == MouseMessages.WM_MOUSEMOVE)
            {
                newRecorder = new MouseMoveRecorderItem()
                {
                    Point = e.Point,
                    MouseData = e.MouseData,
                };

                if (IsMouseMove())
                {
                    ResetWaitingTime();
                    this.CurrentMouseRecorder.ChildItems.Add(newRecorder);
                    UpdateItem(this.CurrentMouseRecorder);
                    return;
                }
            }

            AddMouseItem(newRecorder);
        }

        public void Add(KeyInputEventArgs e)
        {
            IRecorderItem newRecorder = null;
            if (e.KeyData.EventType == KeyEvent.up)
            {
                ALog.Debug("e.KeyData.EventType == KeyEvent.up");
                if (IsKeyPress())
                {
                    ALog.Debug("IsKeyPress::True!");
                    newRecorder = new KeyPressRecorderItem()
                    {
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                    };

                    if (this.CurrentKeyRecorder?.Recorder == RecorderType.KeyPress)
                    {
                        this.CurrentKeyRecorder.ChildItems.Add(newRecorder);
                        UpdateItem(CurrentKeyRecorder);
                        return;
                    }
                    
                    ReplaceKeyItem(this.CurrentRecorder, newRecorder);
                    return;
                }
                else
                {
                    ALog.Debug("IsKeyPress::False!");
                    newRecorder = new KeyUpRecorderItem()
                    {
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                    };
                }
            }
            else
            {
                //it's state on pressing key
                if (GetLastItem()?.Recorder == RecorderType.KeyDown)
                    return;

                newRecorder = new KeyDownRecorderItem()
                {
                    Keyname = e.KeyData.Keyname,
                    UnicodeCharacter = e.KeyData.UnicodeCharacter,
                };
            }

            AddKeyItem(newRecorder);
        }

        private IRecorderItem GetLastItem(bool isIgnoreWaitItem = true)
        {
            for(int i=Items.Count-1; i>=0; i--)
            {
                if (isIgnoreWaitItem && Items[i].Recorder == RecorderType.WaitTime)
                    continue;

                return Items[i];
            }
            return null;
        }

        private bool IsKeyPress()
        {
            if( (this.CurrentKeyRecorder?.Recorder == RecorderType.KeyDown || this.CurrentKeyRecorder?.Recorder == RecorderType.KeyPress) &&
                (DateTime.Now - this.CurrentKeyRecorder?.Time).Value.TotalSeconds < KeyPressIntervalTimeSec)
            {
                return true;
            }

            return false;
        }

        private bool IsMouseMove()
        {
            if(this.CurrentRecorder?.Recorder == RecorderType.MouseMove)
                return true;

            return false;
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
            ALog.Debug("Add Waiting Event!");
        }

        public void ResetWaitingTime()
        {
            this.CurrentWaitingTimeSec = 0;
        }

        public void AddItem(IRecorderItem item)
        {
            ResetWaitingTime();
            this.CurrentRecorder = item;
            this.Items.Add(item);
            if (OnAddItem != null)
                OnAddItem(item);
        }

        public void AddMouseItem(IRecorderItem item)
        {
            this.CurrentMouseRecorder = item;
            AddItem(item);
        }

        public void AddKeyItem(IRecorderItem item)
        {
            this.CurrentKeyRecorder = item;
            AddItem(item);
        }

        public bool ReplaceItem(IRecorderItem oldItem, IRecorderItem newItem)
        {
            int index = this.Items.IndexOf(oldItem);
            if (index < 0 || index >= this.Items.Count)
            {
                ALog.Debug("ReplaceItem::Index is invalide!(Index={0})", index);
                return false;
            }

            this.CurrentRecorder = newItem;
            this.Items[index] = newItem;
            if (OnReplaceItem != null)
                OnReplaceItem(oldItem, newItem);

            return true;
        }

        public bool ReplaceKeyItem(IRecorderItem oldItem, IRecorderItem newItem)
        {
            this.CurrentKeyRecorder = newItem;
            return ReplaceItem(oldItem, newItem);
        }

        public bool ReplaceMouseItem(IRecorderItem oldItem, IRecorderItem newItem)
        {
            this.CurrentMouseRecorder = newItem;
            return ReplaceItem(oldItem, newItem);
        }

        public void UpdateItem(IRecorderItem item)
        {
            if (OnUpdateItem != null)
                OnUpdateItem(item);
        }
    }
}
