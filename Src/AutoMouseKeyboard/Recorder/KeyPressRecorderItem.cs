using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class KeyPressRecorderItem : AbsRecorderItem
    {
        public KeyPressRecorderItem()
        {
            this.Recorder = RecorderType.KeyPress;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
