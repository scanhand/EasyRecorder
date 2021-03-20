using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class MouseWheelRecorderItem : AbsRecorderItem
    {
        public uint MouseData { get; set; } = 0;

        public MouseWheelRecorderItem()
        {
            this.Recorder = RecorderType.MouseWheel;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
