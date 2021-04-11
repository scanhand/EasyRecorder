using AMK.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using WindowsInput;

namespace AMK.Recorder
{
    public class MouseMoveRecorderItem : AbsRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format($"X: {this.Point.X},Y: {this.Point.Y},Count={this.ChildItems.Count}");
            }
        }

        public MouseMoveRecorderItem()
        {
            this.Recorder = RecorderType.MouseMove;
        }

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            Point pt = AUtil.ToMouseSimulatorPoint(this.Point);
            GM.Instance.InputSimulator.Mouse.MoveMouseTo(pt.X, pt.Y);
            foreach (var item in this.ChildItems)
            {
                //Waiting
                player.WaitingPlaying(item);
                //Action
                pt = AUtil.ToMouseSimulatorPoint(item.Point);
                GM.Instance.InputSimulator.Mouse.MoveMouseTo(pt.X, pt.Y);
            }
            return true;
        }
    }
}
