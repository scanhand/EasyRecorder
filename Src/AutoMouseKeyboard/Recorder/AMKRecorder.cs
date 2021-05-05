using AMK.Global;
using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AMK.Recorder
{
    public class AMKRecorder
    {
        private AMKState _State = AMKState.Stop;
        public AMKState State
        {
            get
            {
                return _State;
            }

            set
            {
                _State = value;
                if (OnChangedState != null)
                    OnChangedState(_State);
            }

        }

        public Action<AMKState> OnChangedState = null;

        public List<IRecorderItem> Items = new List<IRecorderItem>();

        private IRecorderItem _CurrentRecoder = null;

        public IRecorderItem CurrentRecorder
        {
            get
            {
                return _CurrentRecoder;
            }

            set
            {
                IRecorderItem lastRecorderItem = _CurrentRecoder;
                _CurrentRecoder = value;

                if (lastRecorderItem != null)
                {
                    lastRecorderItem.State = RecorderItemState.None;
                    OnUpdateItem(lastRecorderItem);
                }

                if (_CurrentRecoder != null)
                {
                    _CurrentRecoder.State = RecorderItemState.Activated;
                    OnUpdateItem(_CurrentRecoder);
                }
            }
        }

        public IRecorderItem CurrentKeyRecorder = null;

        public IRecorderItem CurrentMouseRecorder = null;

        public AMKMouseRecorder MouseRecorder = null;

        public AMKWaitingRecorder WaitingRecorder = null;

        public AMKKeyRecorder KeyRecorder = null;

        public AMKApplicationRecorder ApplicationRecorder = null;

        public AMKRecorderItemConfigManager RecorderItemConfigManager = null;

        public AMKPlayer Player = null;

        public Action<IRecorderItem> OnAddItem = null;

        public Action<IRecorderItem, IRecorderItem> OnInsertItem = null;

        public Action<IRecorderItem> OnUpdateItem = null;

        public Action<IRecorderItem, IRecorderItem> OnReplaceItem = null;

        public Action<IRecorderItem> OnDeleteItem = null;

        public Action OnResetItem = null;

        public Action OnStartPlaying
        {
            get
            {
                return this.Player.OnStartPlaying;
            }

            set
            {
                this.Player.OnStartPlaying = value;
            }
        }

        public Action<bool> OnStopPlaying
        {
            get
            {
                return this.Player.OnStopPlaying;
            }

            set
            {
                this.Player.OnStopPlaying = value;
            }
        }

        public Action OnStartRecording = null;

        public Action OnStopRecording = null;

        public const double MinimumTimeSpan = 0.1;

        public AMKRecorder()
        {
            this.MouseRecorder = new AMKMouseRecorder(this);
            this.KeyRecorder = new AMKKeyRecorder(this);
            this.WaitingRecorder = new AMKWaitingRecorder(this);
            this.ApplicationRecorder = new AMKApplicationRecorder(this);
            this.Player = new AMKPlayer(this);
            this.RecorderItemConfigManager = new AMKRecorderItemConfigManager(this);

            this.RecorderItemConfigManager.OnReplaceItem += (oldItem, newItem) =>
            {
                double totalTimeSpanSec = newItem.TotalTimeDurationSec - oldItem.TotalTimeDurationSec;
                ReplaceItem(oldItem, newItem);
                TimeSpan decreaseTime = TimeSpan.FromSeconds(totalTimeSpanSec);
                //Adjust a timestamp in remained items
                AdjustTimeSpanbyItem(newItem, decreaseTime);
            };
        }

        public void Reset()
        {
            this.Items.Clear();
            if (OnResetItem != null)
                OnResetItem();
        }

        public void StartRecording(bool isReset)
        {
            ALog.Debug("StartRecording");
            if (isReset)
                this.Reset();
            StartRecording();
        }

        private void StartRecording()
        {
            OnStartRecording();
            this.ResetCurrentRecorder();
            this.State = AMKState.Recording;
            this.WaitingRecorder.Start();
        }

        public void StopRecording()
        {
            ALog.Debug("StopRecording");
            this.State = AMKState.Stop;
            this.WaitingRecorder.Stop();
            OnStopRecording();
        }

        public void StopAll()
        {
            ALog.Debug("StopAll");
            this.StopRecording();
            this.StopPlaying();
        }

        public void ResetItems()
        {
            ALog.Debug("ResetItems");
            this.StopPlaying();
            this.StopRecording();
            this.Reset();
        }

        public void Add(ApplicationEventArgs e)
        {
            if (this.State != AMKState.Recording)
                return;

            this.ApplicationRecorder.Add(e);
        }

        public void Add(MouseEventArgs e)
        {
            if (this.State != AMKState.Recording)
                return;

            this.MouseRecorder.Add(e);
        }

        public void Add(KeyInputEventArgs e)
        {
            if (this.State != AMKState.Recording)
                return;

            this.KeyRecorder.Add(e);
        }

        public IRecorderItem GetLastItem(bool isIgnoreWaitItem = true)
        {
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                if (isIgnoreWaitItem && this.Items[i].Recorder == RecorderType.WaitTime)
                    continue;

                return this.Items[i];
            }
            return null;
        }

        public IRecorderItem GetLastKeyItem(bool isIgnoreWaitItem = true)
        {
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                if (isIgnoreWaitItem && this.Items[i].Recorder == RecorderType.WaitTime)
                    continue;

                if (this.Items[i].Recorder == RecorderType.KeyUpDown ||
                     this.Items[i].Recorder == RecorderType.KeyPress)
                    return this.Items[i];
            }
            return null;
        }

        public void AddItem(IRecorderItem item)
        {
            if (item == null)
                return;

            ResetWaitingTime();
            this.CurrentRecorder = item;
            this.Items.Add(item);
            if (OnAddItem != null)
                OnAddItem(item);
        }

        public void AddMouseItem(IRecorderItem item)
        {
            if (item == null)
                return;

            this.CurrentMouseRecorder = item;
            AddItem(item);
        }

        public void AddKeyItem(IRecorderItem item)
        {
            if (item == null)
                return;

            this.CurrentKeyRecorder = item;
            AddItem(item);
        }

        public void ResetCurrentRecorder()
        {
            this.CurrentRecorder = null;
            this.CurrentKeyRecorder = null;
            this.CurrentMouseRecorder = null;
        }

        public void ResetWaitingTime()
        {
            this.WaitingRecorder.ResetWaitingTime();
        }

        public bool ReplaceItem(IRecorderItem oldItem, IRecorderItem newItem)
        {
            int index = this.Items.IndexOf(oldItem);
            if (index < 0 || index >= this.Items.Count)
            {
                ALog.Debug("ReplaceItem::Index is invalide!(Index={0})", index);
                return false;
            }

            ResetWaitingTime();
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

        public bool DeleteItem(IRecorderItem item)
        {
            if (item == null)
                return false;

            //check is this last item
            if (!IsFirstItem(item))
            {
                int removedItemIndex = this.Items.IndexOf(item);
                IRecorderItem prevItem = this.Items[removedItemIndex - 1];
                TimeSpan decreaseTime = GetTimeSpan(item as AbsRecorderItem, prevItem as AbsRecorderItem);

                //Adjust a timestamp in remained items
                AdjustTimeSpanbyItem(item, decreaseTime);
            }

            this.Items.Remove(item);
            if (OnDeleteItem != null)
                OnDeleteItem(item);

            return true;
        }

        private void AdjustTimeSpanbyItem(IRecorderItem item, TimeSpan decreaseTime)
        {
            if (IsLastItem(item) || decreaseTime.TotalSeconds == 0)
                return;

            int startIndex = this.Items.IndexOf(item);
            //Adjust a timestamp in remained items
            for (int i = startIndex + 1; i < this.Items.Count; i++)
            {
                AbsRecorderItem recorderItem = this.Items[i] as AbsRecorderItem;
                recorderItem.AdjustTimeSpan(decreaseTime);
            }
        }

        public bool InsertItem(IRecorderItem prevItem, IRecorderItem newItem)
        {
            int startIndex = -1;
            if (prevItem != null)
                startIndex = this.Items.IndexOf(prevItem);

            this.Items.Insert(startIndex + 1, newItem);
            if (OnInsertItem != null)
                OnInsertItem(prevItem, newItem);

            TimeSpan increaseTimeSec = TimeSpan.FromSeconds(newItem.TotalTimeDurationSec);
            for (int i = startIndex + 1; i < this.Items.Count; i++)
            {
                AbsRecorderItem recorderItem = this.Items[i] as AbsRecorderItem;
                recorderItem.AdjustTimeSpan(increaseTimeSec);
            }
            return true;
        }

        private bool IsLastItem(IRecorderItem item)
        {
            return this.Items.Last().Equals(item);
        }

        private bool IsFirstItem(IRecorderItem item)
        {
            return this.Items.First().Equals(item);
        }

        private TimeSpan GetTimeSpan(AbsRecorderItem item, AbsRecorderItem prevItem)
        {
            return prevItem.GetVeryLastTime() - item.GetVeryLastTime();
        }

        public bool DeleteItem(List<IRecorderItem> items)
        {
            if (items == null)
                return false;

            for (int i = 0; i < items.Count; i++)
                this.DeleteItem(items[i]);
            return true;
        }

        public void ResetCurrentRecorderbyLast()
        {
            this.CurrentRecorder = GetLastItem();
            this.CurrentKeyRecorder = GetLastKeyItem();
        }

        public void UpdateItem(IRecorderItem item)
        {
            //Do not call ResetWaitingTime()
            if (OnUpdateItem != null)
                OnUpdateItem(item);
        }

        public bool StartPlaying()
        {
            if (this.Items.Count <= 0)
            {
                ALog.Debug("AMKRecorder::StartPlaying::Item's count is 0.");
                return false;
            }

            ALog.Debug("AMKRecorder::Start Playing");
            this.State = AMKState.Playing;
            this.Player.Start(this.Items);
            return true;
        }

        public bool StartPlaying(List<IRecorderItem> items)
        {
            if (items.Count <= 0)
            {
                ALog.Debug("AMKRecorder::StartPlaying::Item's count is 0.");
                return false;
            }

            ALog.Debug("AMKRecorder::Start Playing");
            this.State = AMKState.Playing;
            this.Player.Start(items);
            return true;
        }

        public void StopPlaying()
        {
            ALog.Debug("AMKRecorder::Stop Playing");
            this.Player.Stop();
            this.State = AMKState.Stop;
        }

        public void ResetToStart()
        {
            ALog.Debug("AMKRecorder::ResetToStart");
            if (!AUtil.IsStopPause(this.State))
                return;

            this.Player.ResetToStart();
        }
    }
}
