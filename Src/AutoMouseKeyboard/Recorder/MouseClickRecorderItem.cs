using AMK.Global;
using System.Windows;

namespace AMK.Recorder
{
    public class MouseClickRecorderItem : AbsRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format("X: {0,4}, Y: {1,4}, {2,-6}", this.Point.X, this.Point.Y, this.Button.ToString());
            }
        }

        public MouseClickRecorderItem()
        {
            this.Recorder = RecorderType.MouseClick;
        }

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);

            //Action
            Point pt = AUtil.ToMouseSimulatorPoint(this.Point);
            GM.Instance.InputSimulator.Mouse.MoveMouseTo(pt.X, pt.Y);
            if (this.Button == ButtonType.Left)
                GM.Instance.InputSimulator.Mouse.LeftButtonClick();
            else
                GM.Instance.InputSimulator.Mouse.RightButtonClick();

            return true;
        }
    }
}
