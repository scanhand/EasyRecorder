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
        public uint MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format($"\t\tX: {this.Point.X}\tY: {this.Point.Y}\tCount={this.ChildItems.Count}");
            }
        }

        public MouseMoveRecorderItem()
        {
            this.Recorder = RecorderType.MouseMove;
        }

        public override bool Play()
        {
            Point pt = AUtil.ToMouseSimulatorPoint(this.Point);
            GM.Instance.InputSimulator.Mouse.MoveMouseTo(pt.X, pt.Y);

            DateTime lastTime = this.Time;
            foreach (var item in this.ChildItems)
            {
                if ((item.Time - lastTime).TotalSeconds > AUtil.MouseSimulatorMiniumSleepTimeSec)
                {
                    GM.Instance.InputSimulator.Mouse.Sleep(item.Time - lastTime);
                    lastTime = item.Time;
                }

                pt = AUtil.ToMouseSimulatorPoint(item.Point);
                GM.Instance.InputSimulator.Mouse.MoveMouseTo(pt.X, pt.Y);
            }
            return true;
        }
    }
}
