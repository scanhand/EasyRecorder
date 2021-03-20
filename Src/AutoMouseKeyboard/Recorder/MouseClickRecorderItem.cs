using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class MouseClickRecorderItem : AbsRecorderItem
    {
        public uint MouseData { get; set; } = 0;

        public MouseClickRecorderItem()
        {
            this.Recorder = RecorderType.MouseClick;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
