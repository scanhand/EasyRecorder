﻿using ESR.Global;
using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ESR.Recorder
{
    public class ESRRecorder
    {
        private ESRState _State = ESRState.Stop;
        public ESRState State
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

        public Action<ESRState> OnChangedState = null;

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

        public ESRMouseRecorder MouseRecorder = null;

        public ESRWaitingRecorder WaitingRecorder = null;

        public ESRKeyRecorder KeyRecorder = null;

        public ESRApplicationRecorder ApplicationRecorder = null;

        public ESRRecorderItemConfigManager RecorderItemConfigManager = null;

        public ESRPlayer Player = null;

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

        public ESRRecorder()
        {
            this.MouseRecorder = new ESRMouseRecorder(this);
            this.KeyRecorder = new ESRKeyRecorder(this);
            this.WaitingRecorder = new ESRWaitingRecorder(this);
            this.ApplicationRecorder = new ESRApplicationRecorder(this);
            this.Player = new ESRPlayer(this);
            this.RecorderItemConfigManager = new ESRRecorderItemConfigManager(this);

            this.RecorderItemConfigManager.OnReplaceItem += (oldItem, newItem) =>
            {
                double totalTimeSpanSec = newItem.TotalTimeDurationSec - oldItem.TotalTimeDurationSec;
                ReplaceItem(oldItem, newItem);
                TimeSpan decreaseTime = TimeSpan.FromSeconds(totalTimeSpanSec);
                //Adjust a timestamp in remained items
                AdjustTimeSpanbyItem(newItem, decreaseTime);
            };

            this.RecorderItemConfigManager.OnUpdateItem += (item) =>
            {
                UpdateItem(item);
            };
        }

        public void Initialize()
        {
            this.State = ESRState.Stop;
        }

        public void Reset()
        {
            this.Items.Clear();
            if (OnResetItem != null)
                OnResetItem();
        }

        public void PauseAll()
        {
            if (this.State == ESRState.Playing)
            {
                this.PausePlaying();
            }
            else if (this.State == ESRState.Recording)
            {
                this.PauseRecording();
            }
            else if (this.State == ESRState.DragClick)
            {
                this.StopRecording();
            }
        }

        public void StartRecordingWithConfirm()
        {
            ALog.Debug("");

            bool isReset = true;
            if (this.Items.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to reset all of recorder items?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                    return;

                if (result == MessageBoxResult.Yes)
                    isReset = true;
                else if (result == MessageBoxResult.No)
                    isReset = false;
            }

            if (isReset)
                this.Reset();

            StartRecording();
        }

        public void StartRecording()
        {
            OnStartRecording();
            this.ResetCurrentRecorder();
            this.State = ESRState.Recording;
            this.WaitingRecorder.Start();
        }

        public void StopRecording()
        {
            ALog.Debug("");
            this.State = ESRState.Stop;
            this.WaitingRecorder.Stop();
            OnStopRecording();
        }

        public void StartDragClick()
        {
            ALog.Debug("");
            this.State = ESRState.DragClick;
            this.WaitingRecorder.Stop();
        }

        public void StopDragClick()
        {
            ALog.Debug("");
            this.State = ESRState.Stop;
            OnStopRecording();
        }

        public void PauseRecording()
        {
            ALog.Debug("");
            this.State = ESRState.RecordingPause;
            this.WaitingRecorder.Stop();
            OnStopRecording();
        }

        public void StopAll()
        {
            ALog.Debug("");
            this.StopRecording();
            this.StopPlaying();
        }

        public void ResetItems()
        {
            ALog.Debug("");
            this.StopPlaying();
            this.StopRecording();
            this.Reset();
        }

        public void Add(ApplicationEventArgs e)
        {
            if (this.State != ESRState.Recording)
                return;

            this.ApplicationRecorder.Add(e);
        }

        public void Add(MouseEventArgs e)
        {
            if (this.State != ESRState.Recording)
                return;

            this.MouseRecorder.Add(e);
        }

        public void Add(KeyInputEventArgs e)
        {
            if (this.State != ESRState.Recording)
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
                     this.Items[i].Recorder == RecorderType.KeyPress ||
                      this.Items[i].Recorder == RecorderType.KeyHotkey)
                    return this.Items[i];
            }
            return null;
        }

        public IRecorderItem GetLastMouseItem(bool isIgnoreWaitItem = true)
        {
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                if (isIgnoreWaitItem && this.Items[i].Recorder == RecorderType.WaitTime)
                    continue;

                if (this.Items[i].Recorder == RecorderType.MouseWheel ||
                     this.Items[i].Recorder == RecorderType.MouseClick ||
                      this.Items[i].Recorder == RecorderType.MouseMove ||
                       this.Items[i].Recorder == RecorderType.MouseUpDown)
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
            ResetCurrentRecorderbyLast();

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
            this.CurrentMouseRecorder = GetLastMouseItem();
        }

        public void UpdateItem(IRecorderItem item)
        {
            //Do not call ResetWaitingTime()
            if (OnUpdateItem != null)
                OnUpdateItem(item);
        }

        public void StartPlaying(bool isReset)
        {
            ALog.Debug("IsReset={0}", isReset);
            if (isReset)
                ResetToStart();
            StartPlaying();
        }

        public bool StartPlaying()
        {
            if (this.Items.Count <= 0)
            {
                ALog.Debug("ESRRecorder::StartPlaying::Item's count is 0.");
                return false;
            }

            ALog.Debug("");
            this.State = ESRState.Playing;
            this.Player.Start(this.Items);
            return true;
        }

        public bool StartPlaying(List<IRecorderItem> items)
        {
            if (items.Count <= 0)
            {
                ALog.Debug("ESRRecorder::StartPlaying::Item's count is 0.");
                return false;
            }

            ALog.Debug("");
            this.State = ESRState.Playing;
            this.Player.Start(items);
            return true;
        }

        public void StopPlaying()
        {
            ALog.Debug("");
            this.Player.Stop();
            this.State = ESRState.Stop;
        }

        public void PausePlaying()
        {
            ALog.Debug("");
            this.Player.Stop();
            this.State = ESRState.PlayingPause;
        }

        public void ResetToStart()
        {
            ALog.Debug("");
            if (!AUtil.IsStopPause(this.State))
                return;

            this.Player.ResetToStart();
        }
    }
}
