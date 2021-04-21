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
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("X: {0,4}, Y: {1,4}", this.Point.X, this.Point.Y);
                if(this.ChildItems.Count > 0)
                {
                    sb.Append(" > ");
                    IRecorderItem last = this.ChildItems.Last();
                    sb.AppendFormat("X: {0,4}, Y: {1,4}", last.Point.X, last.Point.Y); ;
                }
                return sb.ToString();
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
                if (!player.IsThreadEnable)
                    return false;

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
