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

        public override string Description
        {
            get
            {
                return string.Format($"{this.Dir.ToString()} X: {this.Point.x}, Y: {this.Point.y} Count: {this.ChildItems.Count}");
            }
        }

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
