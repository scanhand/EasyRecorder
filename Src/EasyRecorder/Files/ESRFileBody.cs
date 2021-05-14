using ESR.Recorder;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESR.Files
{
    public class ESRFileBody
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

        public static ESRFileBody FromJsonString(string json)
        {
            ESRFileBody fileBody = null;

            fileBody = JsonConvert.DeserializeObject<ESRFileBody>(json, new JsonSerializerSettings()
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
