﻿using System.Diagnostics;
using System.Threading;

namespace AMK.Recorder
{
    public class AMKWaitingRecorder
    {
        public AMKRecorder AMKRecorder { get; set; } = null;

        private IRecorderItem CurrentRecorder
        {
            get
            {
                return AMKRecorder.CurrentRecorder;
            }
        }

        private IRecorderItem CurrentMouseRecorder
        {
            get
            {
                return AMKRecorder.CurrentMouseRecorder;
            }
        }

        private Thread ThreadRecording = null;

        private bool IsThreadRecording = false;

        //500 msec
        public double WaitingTimeSec = 0.500f;

        private double CurrentWaitingTimeSec = 0;

        public AMKWaitingRecorder(AMKRecorder recorder)
        {
            this.AMKRecorder = recorder;
        }

        public bool Start()
        {
            if (this.ThreadRecording != null)
                return false;

            this.ThreadRecording = new Thread(() =>
            {
                const double waitTime = 0.02f; // 20 mesc

                while (this.IsThreadRecording)
                {
                    double startTime = Stopwatch.GetTimestamp();
                    if (this.CurrentWaitingTimeSec >= this.WaitingTimeSec)
                        AddWaitingRecorderItem(this.CurrentWaitingTimeSec);

                    Thread.Sleep((int)(waitTime * 1000));
                    this.CurrentWaitingTimeSec += (((double)Stopwatch.GetTimestamp() - startTime) / (double)Stopwatch.Frequency);
                }

                AddWaitingRecorderItem(this.CurrentWaitingTimeSec);
            });

            this.IsThreadRecording = true;
            this.ThreadRecording.Start();

            ALog.Debug("WaitingRecorder.Start()");
            return true;
        }

        public void Stop()
        {
            if (this.ThreadRecording == null || !this.ThreadRecording.IsAlive)
                return;

            int tryCount = 0;
            const int timeInterval = 20;
            const int timeOutCount = 1000 / timeInterval;
            this.IsThreadRecording = false;
            while (true)
            {
                if (!this.ThreadRecording.IsAlive)
                    break;

                if (tryCount++ >= timeOutCount)
                {
                    this.ThreadRecording.Abort();
                    break;
                }

                Thread.Sleep(timeInterval);
            }
            this.ThreadRecording = null;
            ALog.Debug("WaitingRecorder.Stop()");
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
                this.AMKRecorder.UpdateItem(this.CurrentRecorder);
                return;
            }

            this.AMKRecorder.AddItem(newRecorder);
            ALog.Debug("Add Waiting Event!");
        }

        public void ResetWaitingTime()
        {
            this.CurrentWaitingTimeSec = 0;
        }
    }
}
