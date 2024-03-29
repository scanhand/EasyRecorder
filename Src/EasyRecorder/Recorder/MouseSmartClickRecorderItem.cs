﻿using ESR.Global;

namespace ESR.Recorder
{
    public class MouseSmartClickRecorderItem : AbsRecorderItem, IMouseRecorderItem
    {
        public int MouseData { get; set; } = 0;

        public override string Description
        {
            get
            {
                return string.Format("X: {0,4}, Y: {1,4}", this.Point.X, this.Point.Y);
            }
        }

        public MouseSmartClickRecorderItem()
        {
            this.Recorder = RecorderType.MouseSmartClick;
        }

        public override bool Play(ESRPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            return true;
        }
    }
}
