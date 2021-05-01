using AMK.Global;

namespace AMK.Recorder
{
    class WaitTimeRecorderItem : AbsRecorderItem
    {
        public double WaitingTimeSec { get; set; } = 0;

        public override double TotalTimeDurationSec
        {
            get
            {
                double totalWaitingTimeSec = this.WaitingTimeSec;
                IRecorderItem prevItem = this;
                foreach (var item in this.ChildItems)
                {
                    totalWaitingTimeSec += (item.Time - prevItem.Time).TotalSeconds;
                    prevItem = item;
                }
                return totalWaitingTimeSec;
            }
        }

        public override string Description
        {
            get
            {
                return string.Format("Time: {0:F2} sec", this.TotalTimeDurationSec);
            }
        }

        public WaitTimeRecorderItem()
        {
            this.Recorder = RecorderType.WaitTime;
        }

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            foreach (var item in this.ChildItems)
            {
                if (!player.IsThreadEnable)
                    return false;

                //Waiting
                player.WaitingPlaying(item);
            }
            return true;
        }
    }
}
