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
    public interface IRecorderItem
    {
        [JsonConverter(typeof(StringEnumConverter))]
        RecorderType Recorder {get; set;}

        [JsonConverter(typeof(StringEnumConverter))]
        Dir Dir { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        LR LR { get; set; }

        Point Point { get; set; }

        List<IRecorderItem> ChildItems { get; set; }

        bool IsEqualType(IRecorderItem item);

        bool Play();
    }
}
