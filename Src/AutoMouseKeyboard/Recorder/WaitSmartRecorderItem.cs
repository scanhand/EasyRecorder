using AMK.Global;

namespace AMK.Recorder
{
    public class WaitSmartRecorderItem : AbsRecorderItem
    {
        public override string Description
        {
            get
            {
                return string.Format("X: {0,4}, Y: {1,4}", this.Point.X, this.Point.Y);
            }
        }

        public WaitSmartRecorderItem()
        {
            this.Recorder = RecorderType.WaitSmart;
        }

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            return true;
        }
    }
}
