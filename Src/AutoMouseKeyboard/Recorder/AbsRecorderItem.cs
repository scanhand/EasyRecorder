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

        public abstract bool Play();
    }
}
