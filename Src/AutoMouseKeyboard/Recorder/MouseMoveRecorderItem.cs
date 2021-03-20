using EventHook.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class MouseMoveRecorderItem : AbsRecorderItem
    {
        public uint MouseData { get; set; } = 0;

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
