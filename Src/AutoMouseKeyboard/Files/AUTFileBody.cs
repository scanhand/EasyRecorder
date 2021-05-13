using AUT.Recorder;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AUT.Files
{
    public class AUTFileBody
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

        public static AUTFileBody FromJsonString(string json)
        {
            AUTFileBody fileBody = null;

            fileBody = JsonConvert.DeserializeObject<AUTFileBody>(json, new JsonSerializerSettings()
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
