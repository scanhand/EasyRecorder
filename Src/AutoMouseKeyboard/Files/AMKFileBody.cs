﻿using AMK.Recorder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Files
{
    public class AMKFileBody
    {
        public List<IRecorderItem> Items = new List<IRecorderItem>();

        public string ToJsonString()
        {
            string json = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
            });
            return json;
        }

        public static AMKFileBody FromJsonString(string json)
        {
            AMKFileBody fileBody = null;

            fileBody = JsonConvert.DeserializeObject<AMKFileBody>(json, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
            });

            return fileBody;
        }
    }
}
