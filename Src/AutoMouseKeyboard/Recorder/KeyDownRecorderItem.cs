using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class KeyDownRecorderItem : AbsRecorderItem
    {
        public KeyDownRecorderItem()
        {
            this.Recorder = RecorderType.KeyDown;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
