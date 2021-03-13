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

        bool Play();
    }
}
