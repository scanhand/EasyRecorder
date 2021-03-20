using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    class WaitTimeRecorderItem : AbsRecorderItem
    {
        public float WaitingTimeSec {get; set;} = 0;

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
