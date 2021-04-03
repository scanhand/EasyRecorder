﻿using EventHook.Hooks;
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
        string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        RecorderType Recorder {get; set;}

        [JsonConverter(typeof(StringEnumConverter))]
        Dir Dir { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        LR LR { get; set; }

        Point Point { get; set; }

        DateTime Time { get; set; }

        List<IRecorderItem> ChildItems { get; set; }

        bool IsEqualType(IRecorderItem item);

        bool Play();
    }
}