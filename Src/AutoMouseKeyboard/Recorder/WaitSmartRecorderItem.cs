using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMK.Global;

namespace AMK.Recorder
{
    public class WaitSmartRecorderItem : AbsRecorderItem
    {
        public override string Description
        {
            get
            {
                return string.Format("X: {0}, Y: {1}", this.Point.X, this.Point.Y);
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
