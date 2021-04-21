﻿using AMK.Global;
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
                return string.Format("{0,6}, X: {1,4}, Y: {2,4}, Count: {3,3}", this.Dir.ToString(), this.Point.X, this.Point.Y, this.ChildItems.Count);
            }
        }

        public MouseWheelRecorderItem()
        {
            this.Recorder = RecorderType.MouseWheel;
        }

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            GM.Instance.InputSimulator.Mouse.VerticalScroll(this.MouseData);
            foreach (var item in this.ChildItems)
            {
                MouseWheelRecorderItem mouseItem = item as MouseWheelRecorderItem;
                if (mouseItem == null)
                    continue;

                if (!player.IsThreadEnable)
                    return false;

                //Waiting
                player.WaitingPlaying(item);
                //Action
                GM.Instance.InputSimulator.Mouse.VerticalScroll(mouseItem.MouseData);
            }
            return true;
        }
    }
}
