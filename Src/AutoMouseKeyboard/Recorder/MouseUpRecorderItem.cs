using AMK.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMK.Recorder
{
    public class MouseUpRecorderItem : AbsRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format("X: {0,4}, Y: {1,4}, {2,-5}", this.Point.X, this.Point.Y, this.LR.ToString());
            }
        }

        public MouseUpRecorderItem()
        {
            this.Recorder = RecorderType.MouseUp;
        }

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            if (this.LR == LR.Left)
                GM.Instance.InputSimulator.Mouse.LeftButtonUp();
            else
                GM.Instance.InputSimulator.Mouse.RightButtonUp();
            return true;
        }
    }
}
