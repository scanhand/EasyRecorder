using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    class WaitTimeRecorderItem : AbsRecorderItem
    {
        public float WaitingTimeSec {get; set;} = 0;

        public override string Description
        {
            get
            {
                return string.Format("Time: {0:F0} msec", WaitingTimeSec * 1000.0f);
            }
        }

        public WaitTimeRecorderItem()
        {
            this.Recorder = RecorderType.WaitTime;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
