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

        public override string Description
        {
            get
            {
                return string.Format("X: {0}, Y: {1}", this.Point.x, this.Point.y);
            }
        }

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
