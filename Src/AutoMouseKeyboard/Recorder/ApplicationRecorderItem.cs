using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class ApplicationRecorderItem : AbsRecorderItem
    {
        public WindowData ApplicationData { get; set; }
        public ApplicationEvents Event { get; set; }

        public override string Description 
        {
            get
            {
                return this.ApplicationData.AppName; 
            }
        }

        public ApplicationRecorderItem()
        {
            this.Recorder = RecorderType.Application;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
