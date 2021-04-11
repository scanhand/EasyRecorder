﻿using AMK.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    public class AMKPlayer
    {
        public AMKRecorder AMKRecorder { get; set; } = null;

        public bool IsThreadEnable = false;

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

        public bool Start()
        {
            Stop();

            this.ThreadPlayer = new Thread(() =>
            {
                bool isLastStep = false;
                if (OnStartPlaying != null)
                    OnStartPlaying();

                ResetToStart();
                while (IsThreadEnable)
                {
                    if (!this.CurrentRecorder.Play(this))
                        break;

                    if (IsLastStep())
                    {
                        isLastStep = true;
                        break;
                    }

                    if (!NextStep())
                        break;
                }

                if (OnStopPlaying != null)
                    OnStopPlaying(isLastStep);
            });

            this.IsThreadEnable = true;
            ResetLastItem();
            this.ThreadPlayer.Start();
            return true;
        }

        private void ResetToStart()
        {
            this.CurrentRecorder = this.AMKRecorder.Items.First();
        }

        private bool IsLastStep()
        {
            return this.CurrentRecorder.Equals(this.AMKRecorder.Items.Last());
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

        public bool Stop()
        {
            if (ThreadPlayer == null || !ThreadPlayer.IsAlive)
                return true;

            IsThreadEnable = false;
            int tryCount = 0;
            const int timeOutCount = 1000 / 100; // 1000mesc / 100msec
            while(true)
            {
                if (!ThreadPlayer.IsAlive)
                    break;

                if (tryCount++ > timeOutCount)
                {
                    ThreadPlayer.Abort();
                    ThreadPlayer = null;
                    break;
                }
                Thread.Sleep(100);
            }

            return true;
        }

        public void ResetLastItem()
        {
            this.LastItem = null;
        }

        public double WaitingPlaying(IRecorderItem item)
        {
            if(this.LastItem == null)
                this.LastItem = item;

            double timeSec = (item.Time - this.LastItem.Time).TotalSeconds;
            if (timeSec > AUtil.SimulatorMiniumSleepTimeSec)
            {
                Thread.Sleep((int)(timeSec * 1000.0));
                this.LastItem = item;
            }
            return timeSec;
        }
    }
}
