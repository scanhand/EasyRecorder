using AMK.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMK.Recorder
{
    public class MouseClickRecorderItem : AbsRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format($"{this.LR.ToString()}\t{this.Dir.ToString()}\tX: {this.Point.X}\tY: {this.Point.Y}");
            }
        }

        public MouseClickRecorderItem()
        {
            this.Recorder = RecorderType.MouseClick;
        }

        public override bool Play()
        {
            Point pt = AUtil.ToMouseSimulatorPoint(this.Point);
            GM.Instance.InputSimulator.Mouse.MoveMouseTo(pt.X, pt.Y);

            if(this.LR == LR.Left)
                GM.Instance.InputSimulator.Mouse.LeftButtonClick();
            else
                GM.Instance.InputSimulator.Mouse.RightButtonClick();

            return true;
        }
    }
}
