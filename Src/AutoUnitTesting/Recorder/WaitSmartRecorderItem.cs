using AUT.Global;

namespace AUT.Recorder
{
    public class WaitSmartRecorderItem : AbsRecorderItem, IWaitRecorderItem
    {
        public double WaitingTimeSec { get; set; } = 0;

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

        public override bool Play(AUTPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            return true;
        }
    }
}
