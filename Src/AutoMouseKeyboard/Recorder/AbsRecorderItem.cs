using EventHook.Hooks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public abstract class AbsRecorderItem : IRecorderItem
    {
        public RecorderType Recorder { get; set; } = RecorderType.None;

        public Dir Dir { get; set; } = Dir.Down;

        public LR LR { get; set; } = LR.None;

        public Point Point { get; set; }

        public string ImageSource
        {
            get
            {
                switch (this.Recorder)
                {
                    case RecorderType.MouseClick:   return "Resources/icons8-mouse_leftclick-64.png";
                    case RecorderType.MouseMove: return "Resources/icons8-cursor-64.png";
                    case RecorderType.KeyPress: return "Resources/icons8-keyboard-64.png";
                    case RecorderType.WaitTime: return "Resources/icons8-timer-64.png";
                }
                return string.Empty;
            }
        }

        public List<IRecorderItem> ChildItems { get; set; } = new List<IRecorderItem>();

        public abstract bool Play();

        public bool IsSameRecorderType(IRecorderItem item)
        {
            return this.Recorder == item.Recorder;
        }

        public bool IsEqualType(IRecorderItem item)
        {
            return this.Recorder == item?.Recorder;
        }
    }
}
