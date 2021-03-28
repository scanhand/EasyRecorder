using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class WaitSmartRecorderItem : AbsRecorderItem
    {
        public override string Description
        {
            get
            {
                return string.Format("X: {0}, Y: {1}", this.Point.x, this.Point.y);
            }
        }

        public WaitSmartRecorderItem()
        {
            this.Recorder = RecorderType.WaitSmart;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
