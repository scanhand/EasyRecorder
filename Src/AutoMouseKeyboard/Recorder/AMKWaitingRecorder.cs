using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    public class AMKWaitingRecorder
    {
        public AMKRecorder AMKRecorder { get; set; }

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
        public float WaitingTimeSec = 0.5f;

        private float CurrentWaitingTimeSec = 0;

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
                const float waitTime = 0.1f; // 100 mesc
                while (this.IsThreadRecording)
                {
                    if (this.CurrentWaitingTimeSec >= this.WaitingTimeSec)
                        AddWaitingRecorderItem(this.CurrentWaitingTimeSec);

                    Thread.Sleep((int)(waitTime * 1000));
                    this.CurrentWaitingTimeSec += waitTime;
                }
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

            this.IsThreadRecording = false;
            while (true)
            {
                if (!this.ThreadRecording.IsAlive)
                    break;
            }
            this.ThreadRecording = null;
            ALog.Debug("WaitingRecorder.Stop()");
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
                this.AMKRecorder.UpdateItem(item);
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
