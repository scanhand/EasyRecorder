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
                return string.Format($"\t\tX: {this.Point.x}\tY: {this.Point.y}\tCount={this.ChildItems.Count}");
            }
        }

        public MouseMoveRecorderItem()
        {
            this.Recorder = RecorderType.MouseMove;
        }

        public override bool Play()
        {
            var sim = new InputSimulator();
            sim.Mouse.MoveMouseTo(this.Point.x, this.Point.y);

            foreach(var item in this.ChildItems)
            {
                sim.Mouse.MoveMouseTo(item.Point.x, item.Point.y);
                //Thread.Sleep(1);
            }
            return true;
        }
    }
}
