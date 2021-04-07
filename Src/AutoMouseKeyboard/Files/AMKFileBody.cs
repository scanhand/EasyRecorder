using AMK.Recorder;
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
            return JsonConvert.SerializeObject(this);
        }

        public static AMKFileBody FromJsonString(string json)
        {
            AMKFileBody fileBody = null;

            fileBody = JsonConvert.DeserializeObject<AMKFileBody>(json, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            });

            return fileBody;
        }
    }
}
