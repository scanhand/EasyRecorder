using AMK.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMK.Recorder
{
    public class MouseWheelRecorderItem : AbsRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format($"\t{this.Dir.ToString()}\tX: {this.Point.X}\tY: {this.Point.Y}\tCount: {this.ChildItems.Count}");
            }
        }

        public MouseWheelRecorderItem()
        {
            this.Recorder = RecorderType.MouseWheel;
        }

        public override bool Play()
        {
            GM.Instance.InputSimulator.Mouse.VerticalScroll(this.MouseData);

            DateTime lastTime = this.Time;
            foreach (var item in this.ChildItems)
            {
                MouseWheelRecorderItem mouseItem = item as MouseWheelRecorderItem;
                if (mouseItem == null)
                    continue;

                if ((item.Time - lastTime).TotalSeconds > AUtil.MouseSimulatorMiniumSleepTimeSec)
                {
                    GM.Instance.InputSimulator.Mouse.Sleep(item.Time - lastTime);
                    lastTime = item.Time;
                }

                GM.Instance.InputSimulator.Mouse.VerticalScroll(mouseItem.MouseData);
            }
            return true;
        }
    }
}
