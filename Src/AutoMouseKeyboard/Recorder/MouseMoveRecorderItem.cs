using EventHook.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            var sim = new InputSimulator();
            sim.Mouse.MoveMouseTo(this.Point.X, this.Point.Y);

            foreach(var item in this.ChildItems)
            {
                sim.Mouse.MoveMouseTo(item.Point.X, item.Point.Y);
                //Thread.Sleep(1);
            }
            return true;
        }
    }
}
