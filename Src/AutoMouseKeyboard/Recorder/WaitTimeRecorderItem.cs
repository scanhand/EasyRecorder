using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMK.Global;

namespace AMK.Recorder
{
    class WaitTimeRecorderItem : AbsRecorderItem
    {
        public float WaitingTimeSec {get; set;} = 0;

        public override string Description
        {
            get
            {
                return string.Format("Time: {0:F1} sec", this.WaitingTimeSec);
            }
        }

        public WaitTimeRecorderItem()
        {
            this.Recorder = RecorderType.WaitTime;
        }

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            return true;
        }
    }
}
