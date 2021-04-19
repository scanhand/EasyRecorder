using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            if (this.ThreadRecording == null)
                return;

            int tryCount = 0;
            const int timeInterval = 20;
            const int waitTime = 1000 / timeInterval; 
            this.IsThreadRecording = false;
            while (true)
            {
                if (!this.ThreadRecording.IsAlive)
                    break;

                if (tryCount++ >= waitTime)
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
                var item = this.CurrentRecorder as WaitTimeRecorderItem;
                item.WaitingTimeSec = waitingTimeSec;
                item.Time = newRecorder.Time;
                this.AMKRecorder.UpdateItem(item);
                this.CurrentRecorder.ChildItems.Add(newRecorder);
                ALog.Debug("Accumulated time {0} in currentRecorder", item.WaitingTimeSec);
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
