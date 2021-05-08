using AMK.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace AMK.Recorder
{
    public class AMKPlayer
    {
        public bool IsThreadEnable = false;

        public bool IsInfinitePlaying { get; set; } = true;

        private AMKRecorder AMKRecorder { get; set; } = null;

        private Thread ThreadPlayer = null;

        private IRecorderItem LastItem { get; set; } = null;

        private IRecorderItem CurrentRecorder
        {
            get
            {
                return this.AMKRecorder.CurrentRecorder;
            }

            set
            {
                this.AMKRecorder.CurrentRecorder = value;
            }
        }

        public Action OnStartPlaying = null;

        public Action<bool> OnStopPlaying = null;

        public AMKPlayer(AMKRecorder recorder)
        {
            this.AMKRecorder = recorder;
        }

        public bool Start(List<IRecorderItem> items)
        {
            if(items.Count <= 0)
            {
                ALog.Debug("Playing item is 0.");
                return false;
            }

            Stop();

            this.ThreadPlayer = new Thread(() =>
            {
                bool isLastStep = false;
                if (OnStartPlaying != null)
                    OnStartPlaying();

                this.CurrentRecorder = items.First();
                while (IsThreadEnable)
                {
                    if (!this.CurrentRecorder.Play(this))
                        break;

                    if (IsLastStep(items))
                    {
                        isLastStep = true;
                        break;
                    }

                    if (!NextStep(items))
                        break;
                }

                if (OnStopPlaying != null)
                    OnStopPlaying(isLastStep);
            });
            this.ThreadPlayer.Priority = ThreadPriority.Highest;

            this.IsThreadEnable = true;
            ResetLastItem();
            this.ThreadPlayer.Start();

            return true;
        }

        public void ResetToStart()
        {
            this.CurrentRecorder = this.AMKRecorder.Items.First();
        }

        private bool IsLastStep()
        {
            return this.CurrentRecorder.Equals(this.AMKRecorder.Items.Last());
        }

        private bool IsLastStep(List<IRecorderItem> items)
        {
            return this.CurrentRecorder.Equals(items.Last());
        }

        private bool NextStep()
        {
            int index = this.AMKRecorder.Items.IndexOf(this.CurrentRecorder);
            if (index == -1)
            {
                ALog.Debug("AMKPlayer::index not found.");
                return false;
            }

            if (index >= this.AMKRecorder.Items.Count - 1)
            {
                ALog.Debug("AMKPlayer::Current index is last index");
                return false;
            }

            index++;
            ALog.Debug($"AMKPlayer::Current index is {index}");
            this.CurrentRecorder = this.AMKRecorder.Items[index];
            return true;
        }

        private bool NextStep(List<IRecorderItem> items)
        {
            int index = items.IndexOf(this.CurrentRecorder);
            if (index == -1)
            {
                ALog.Debug("AMKPlayer::index not found.");
                return false;
            }

            if (index >= items.Count - 1)
            {
                ALog.Debug("AMKPlayer::Current index is last index");
                return false;
            }

            index++;
            ALog.Debug($"AMKPlayer::Current index is {index}");
            this.CurrentRecorder = items[index];
            return true;
        }

        public bool Stop()
        {
            if (this.ThreadPlayer == null || !this.ThreadPlayer.IsAlive)
                return true;

            this.IsThreadEnable = false;
            int tryCount = 0;
            const int timeInterval = 20;
            const int timeOutCount = 1000 / timeInterval; // 1000mesc / 20msec
            while (true)
            {
                if (!this.ThreadPlayer.IsAlive)
                    break;

                if (tryCount++ > timeOutCount)
                {
                    this.ThreadPlayer.Abort();
                    break;
                }
                Thread.Sleep(timeInterval);
            }
            this.ThreadPlayer = null;
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
