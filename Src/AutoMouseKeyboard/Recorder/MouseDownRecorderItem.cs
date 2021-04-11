using AMK.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMK.Recorder
{
    public class MouseDownRecorderItem : AbsRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format($"{this.LR.ToString()},X: {this.Point.X},Y: {this.Point.Y}");
            }
        }

        public MouseDownRecorderItem()
        {
            this.Recorder = RecorderType.MouseDown;
        }

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            if (this.LR == LR.Left)
                GM.Instance.InputSimulator.Mouse.LeftButtonDown();
            else
                GM.Instance.InputSimulator.Mouse.RightButtonDown();
            return true;
        }
    }
}
