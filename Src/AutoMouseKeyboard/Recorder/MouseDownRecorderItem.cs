﻿using AMK.Global;
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
                return string.Format("X: {0,4}, Y: {1,4}, {2,-5}", this.Point.X, this.Point.Y, this.Button.ToString());
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
            Point pt = AUtil.ToMouseSimulatorPoint(this.Point);
            GM.Instance.InputSimulator.Mouse.MoveMouseTo(pt.X, pt.Y);
            if (this.Button == ButtonType.Left)
                GM.Instance.InputSimulator.Mouse.LeftButtonDown();
            else
                GM.Instance.InputSimulator.Mouse.RightButtonDown();
            return true;
        }
    }
}
