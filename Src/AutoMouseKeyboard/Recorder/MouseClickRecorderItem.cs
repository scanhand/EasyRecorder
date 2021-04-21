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
                return string.Format("{0,6}, X: {1,4}, Y: {2,4}", this.LR.ToString(), this.Point.X, this.Point.Y);
            }
        }

        public MouseClickRecorderItem()
        {
            this.Recorder = RecorderType.MouseClick;
        }

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
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
