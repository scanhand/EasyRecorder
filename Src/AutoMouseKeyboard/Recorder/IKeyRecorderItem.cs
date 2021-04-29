using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    interface IKeyRecorderItem
    {
        int VkCode { get; set; }
        string Keyname { get; set; }
        string UnicodeCharacter { get; set; }
    }
}
