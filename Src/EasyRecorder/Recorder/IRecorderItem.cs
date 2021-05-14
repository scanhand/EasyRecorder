using ESR.Global;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ESR.Recorder
{
    public interface IRecorderItem
    {
        string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        RecorderItemState State { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        RecorderType Recorder { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        Dir Dir { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        ButtonType Button { get; set; }

        Point Point { get; set; }

        DateTime Time { get; set; }

        [JsonIgnore]
        double TotalTimeDurationSec { get; }

        double ResidualTimeSec { get; set; }

        string Memo { get; set; }

        List<IRecorderItem> ChildItems { get; set; }

        bool IsEqualType(IRecorderItem item);

        bool Play(ESRPlayer player);

        DateTime GetVeryLastTime();
    }
}
