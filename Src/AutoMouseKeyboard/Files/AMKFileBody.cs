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
        private List<IRecorderItem> Items = new List<IRecorderItem>();

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
