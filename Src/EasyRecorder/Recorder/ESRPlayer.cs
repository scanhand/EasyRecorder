﻿using ESR.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ESR.Recorder
{
    public class ESRPlayer
    {
        public bool IsThreadEnable = false;

        public bool IsInfinitePlaying { get; set; } = true;

        private ESRRecorder ESRRecorder { get; set; } = null;

        private IRecorderItem LastItem { get; set; } = null;

        private IRecorderItem CurrentRecorder
        {
            get
            {
                return this.ESRRecorder.CurrentRecorder;
            }

            set
            {
                this.ESRRecorder.CurrentRecorder = value;
            }
        }

        public Action OnStartPlaying = null;

        public Action<bool> OnStopPlaying = null;

        private CancellationTokenSource CancelToken = null;

        public ESRPlayer(ESRRecorder recorder)
        {
            this.ESRRecorder = recorder;
        }

        public bool Start(List<IRecorderItem> items)
        {
            if (items.Count <= 0)
            {
                ALog.Debug("Playing item is 0.");
                return false;
            }

            Stop();

            ResetLastItem();

            this.CancelToken = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                CancellationToken token = (CancellationToken)obj;

                this.IsThreadEnable = true;
                bool isLastStep = false;
                if (OnStartPlaying != null)
                    OnStartPlaying();

                this.CurrentRecorder = items.First();
                while (!token.IsCancellationRequested)
                {
                    if (!this.CurrentRecorder.Play(this))
                        break;

                    if (IsLastStep(items))
                    {
                        if(IsContinuePlaying())
                        {
                            this.CurrentRecorder = items.First();
                            continue;
                        }
                        isLastStep = true;
                        break;
                    }

                    if (!NextStep(items))
                        break;
                }

                if (OnStopPlaying != null)
                    OnStopPlaying(isLastStep);

            }), this.CancelToken.Token);
            ALog.Debug("Start Playing ThreadPool");
            return true;
        }

        private bool IsContinuePlaying()
        {
            ALog.Debug("");
            if (Preference.Instance.RepeatType == RepeatType.Infinite)
                return true;

            if(Preference.Instance.RepeatType == RepeatType.Count)
            {
                bool isContinue = true;
                Preference.Instance.MainWindow.InvokeIfRequired(() =>
                {
                    if (Preference.Instance.RepeatCountControl.Value <= 0)
                        isContinue = false;

                    Preference.Instance.RepeatCountControl.Value--;
                });
                return isContinue;
            }

            return false;
        }

        public void Stop()
        {
            if (!this.IsThreadEnable)
                return;

            this.CancelToken.Cancel();
            this.CancelToken.Dispose();
            this.CancelToken = null;
            this.IsThreadEnable = false;
            return;
        }

        public void ResetToStart()
        {
            this.CurrentRecorder = this.ESRRecorder.Items.First();
        }

        private bool IsLastStep()
        {
            return this.CurrentRecorder.Equals(this.ESRRecorder.Items.Last());
        }

        private bool IsLastStep(List<IRecorderItem> items)
        {
            return this.CurrentRecorder.Equals(items.Last());
        }

        private bool NextStep()
        {
            int index = this.ESRRecorder.Items.IndexOf(this.CurrentRecorder);
            if (index == -1)
            {
                ALog.Debug("ESRPlayer::index not found.");
                return false;
            }

            if (index >= this.ESRRecorder.Items.Count - 1)
            {
                ALog.Debug("ESRPlayer::Current index is last index");
                return false;
            }

            index++;
            ALog.Debug($"ESRPlayer::Current index is {index}");
            this.CurrentRecorder = this.ESRRecorder.Items[index];
            return true;
        }

        private bool NextStep(List<IRecorderItem> items)
        {
            int index = items.IndexOf(this.CurrentRecorder);
            if (index == -1)
            {
                ALog.Debug("ESRPlayer::index not found.");
                return false;
            }

            if (index >= items.Count - 1)
            {
                ALog.Debug("ESRPlayer::Current index is last index");
                return false;
            }

            index++;
            ALog.Debug($"ESRPlayer::Current index is {index}");
            this.CurrentRecorder = items[index];
            return true;
        }

        public void ResetLastItem()
        {
            this.LastItem = null;
        }

        public double WaitingPlaying(IRecorderItem item)
        {
            if (this.LastItem == null)
                this.LastItem = item;

            double timeSec = (item.Time - this.LastItem.Time).TotalSeconds + this.LastItem.ResidualTimeSec;
            if (timeSec < 0)
                timeSec = 0;

            double startTime = Stopwatch.GetTimestamp();
            while (true)
            {
                if (timeSec <= (((double)Stopwatch.GetTimestamp() - startTime) / (double)Stopwatch.Frequency))
                    break;

                if (timeSec > AUtil.SimulatorMiniumSleepTimeSec)
                    Thread.Sleep(1);
            }
            this.LastItem = item;
            this.LastItem.ResidualTimeSec = timeSec - (((double)Stopwatch.GetTimestamp() - startTime) / (double)Stopwatch.Frequency);
            return timeSec;
        }
    }
}
