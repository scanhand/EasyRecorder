using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class MouseSmartClickRecorderItem : AbsRecorderItem
    {
        public uint MouseData { get; set; } = 0;

        public MouseSmartClickRecorderItem()
        {
            this.Recorder = RecorderType.MouseSmartClick;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
