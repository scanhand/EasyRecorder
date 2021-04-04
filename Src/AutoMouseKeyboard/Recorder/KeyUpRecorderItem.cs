using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    public class KeyUpRecorderItem : AbsRecorderItem
    {
        public string Keyname;
        public string UnicodeCharacter;

        public override string Description
        {
            get
            {
                return this.Keyname;
            }
        }

        public KeyUpRecorderItem()
        {
            this.Recorder = RecorderType.KeyUp;
            this.Dir = Dir.Up;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
