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
