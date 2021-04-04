using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    public class KeyDownRecorderItem : AbsRecorderItem
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

        public KeyDownRecorderItem()
        {
            this.Recorder = RecorderType.KeyDown;
            this.Dir = Dir.Down;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
