using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    public class MouseSmartClickRecorderItem : AbsRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format("X: {0}, Y: {1}", this.Point.X, this.Point.Y);
            }
        }

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
