using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class KeyUpRecorderItem : AbsRecorderItem
    {
        public KeyUpRecorderItem()
        {
            this.Recorder = RecorderType.KeyUp;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
