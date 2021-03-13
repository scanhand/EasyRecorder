using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    public class MouseMoveRecorderItem : AbsRecorderItem
    {
        public MouseMoveRecorderItem()
        {
            this.Recorder = RecorderType.MouseMove;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
