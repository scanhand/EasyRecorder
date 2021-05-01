using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    interface IWaitRecorderItem
    {
        double WaitingTimeSec { get; set; }
    }
}
