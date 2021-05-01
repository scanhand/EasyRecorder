using AMK.Global;
using System.Windows;
using System.Windows.Input;

namespace AMK.Recorder
{
    public class MouseUpDownRecorderItem : AbsRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format("X: {0,4}, Y: {1,4}, {2,-5}, {3,-5}", this.Point.X, this.Point.Y, this.Button.ToString(), this.Dir.ToString());
            }
        }

        public MouseUpDownRecorderItem()
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

            if (this.Dir == Dir.Down)
            {
                if (this.Button == ButtonType.Left)
                    GM.Instance.InputSimulator.Mouse.LeftButtonDown();
                else if (this.Button == ButtonType.Right)
                    GM.Instance.InputSimulator.Mouse.RightButtonDown();
                else if (this.Button == ButtonType.Wheel)
                    GM.Instance.InputSimulator.Mouse.MiddleButtonDown();
            }
            else
            {
                if (this.Button == ButtonType.Left)
                    GM.Instance.InputSimulator.Mouse.LeftButtonUp();
                else if (this.Button == ButtonType.Right)
                    GM.Instance.InputSimulator.Mouse.RightButtonUp();
                else if (this.Button == ButtonType.Wheel)
                {
                    GM.Instance.InputSimulator.Mouse.MiddleButtonUp();
                }
            }
            return true;
        }
    }
}
