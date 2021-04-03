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

        public override string Description
        {
            get
            {
                return string.Format("X: {0}, Y: {1}, Count={2}", this.Point.x, this.Point.y, this.ChildItems.Count);
            }
        }

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
