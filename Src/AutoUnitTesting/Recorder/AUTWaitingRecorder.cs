﻿using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AUT.Recorder
{
    public class AUTWaitingRecorder
    {
        public AUTRecorder AUTRecorder { get; set; } = null;

        private IRecorderItem CurrentRecorder
        {
            get
            {
                return AUTRecorder.CurrentRecorder;
            }
        }

        private IRecorderItem CurrentMouseRecorder
        {
            get
            {
                return AUTRecorder.CurrentMouseRecorder;
            }
        }

        //500 msec
        public double WaitingTimeSec = 0.500;

        private double CurrentWaitingTimeSec = 0;

        private bool IsThreadEnable = false;

        private CancellationTokenSource CancelToken = null;

        public AUTWaitingRecorder(AUTRecorder recorder)
        {
            this.AUTRecorder = recorder;
        }

        public bool Start()
        {
            if (this.IsThreadEnable)
                return false;

            this.CancelToken = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj)=>
            {
                CancellationToken token = (CancellationToken)obj;

                this.IsThreadEnable = true;
                const double waitTime = 0.02f; // 20 mesc
                while (!token.IsCancellationRequested)
                {
                    double startTime = Stopwatch.GetTimestamp();
                    if (this.CurrentWaitingTimeSec >= this.WaitingTimeSec)
                        AddWaitingRecorderItem(this.CurrentWaitingTimeSec);

                    Thread.Sleep((int)(waitTime * 1000));
                    this.CurrentWaitingTimeSec += (((double)Stopwatch.GetTimestamp() - startTime) / (double)Stopwatch.Frequency);
                }

                AddWaitingRecorderItem(this.CurrentWaitingTimeSec);
            }), this.CancelToken.Token);
            ALog.Debug("Start WaitingRecorder ThreadPool");
            return true;
        }

        public void Stop()
        {
            if (!this.IsThreadEnable)
                return;

            this.CancelToken.Cancel();
            this.CancelToken.Dispose();
            this.CancelToken = null;
            this.IsThreadEnable = false;
        }

        private void AddWaitingRecorderItem(double waitingTimeSec)
        {
            IRecorderItem newRecorder = null;

            newRecorder = new WaitTimeRecorderItem()
            {
                WaitingTimeSec = waitingTimeSec,
            };

            if (this.CurrentRecorder?.IsEqualType(newRecorder) == true)
            {
                this.CurrentRecorder.ChildItems.Add(newRecorder);
                this.AUTRecorder.UpdateItem(this.CurrentRecorder);
                return;
            }

            this.AUTRecorder.AddItem(newRecorder);
            ALog.Debug("Add Waiting Event!");
        }

        public void ResetWaitingTime()
        {
            this.CurrentWaitingTimeSec = 0;
        }
    }
}