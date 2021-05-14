using ESR.Global;
using System.Windows;

namespace ESR.Recorder
{
    public class MouseClickRecorderItem : AbsRecorderItem, IMouseRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format("X: {0,4}, Y: {1,4}, {2,-6}, Count: {3,-4}", this.Point.X, this.Point.Y, this.Button.ToString(), this.ChildItems.Count + 1);
            }
        }

        public MouseClickRecorderItem()
        {
            this.Recorder = RecorderType.MouseClick;
        }

        public override bool Play(ESRPlayer player)
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

            foreach (var item in this.ChildItems)
            {
                MouseClickRecorderItem mouseItem = item as MouseClickRecorderItem;
                if (mouseItem == null)
                    continue;

                if (!player.IsThreadEnable)
                    return false;

                //Waiting
                player.WaitingPlaying(item);
                //Action
                if (this.Button == ButtonType.Left)
                    GM.Instance.InputSimulator.Mouse.LeftButtonClick();
                else
                    GM.Instance.InputSimulator.Mouse.RightButtonClick();
            }

            return true;
        }
    }
}
