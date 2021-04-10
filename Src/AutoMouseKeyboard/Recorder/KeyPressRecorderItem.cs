﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    public class KeyPressRecorderItem : AbsRecorderItem
    {
        public string Keyname;
        public string UnicodeCharacter;

        public override string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(this.Keyname);
                foreach (var i in this.ChildItems)
                {
                    KeyPressRecorderItem item = i as KeyPressRecorderItem;
                    if (item == null)
                        continue;
                    sb.Append(item.Keyname);
                }
                return sb.ToString();
            }
        }

        public KeyPressRecorderItem()
        {
            this.Recorder = RecorderType.KeyPress;
        }

        public override bool Play()
        {
            return true;
        }
    }
}
