using AMK.Global;
using System.Windows;

namespace AMK.Recorder
{
    public class MouseWheelRecorderItem : AbsRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public const int UpMouseData = -7864320;

        public const int DownMouseData = 7864320;

        public override string Description
        {
            get
            {
                return string.Format("{0,-6}, Count: {1,3}", this.Dir.ToString(), this.ChildItems.Count + 1);
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
