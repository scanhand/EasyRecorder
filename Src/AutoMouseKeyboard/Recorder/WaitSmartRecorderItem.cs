using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class WaitSmartRecorderItem : AbsRecorderItem
    {
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
