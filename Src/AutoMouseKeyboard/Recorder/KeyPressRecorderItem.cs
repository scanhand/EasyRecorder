﻿using AMK.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace AMK.Recorder
{
    public class KeyPressRecorderItem : AbsRecorderItem
    {
        public int VkCode;
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

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            GM.Instance.InputSimulator.Keyboard.KeyPress((VirtualKeyCode)this.VkCode);
            foreach (var i in this.ChildItems)
            {
                if (!player.IsThreadEnable)
                    return false;

                KeyPressRecorderItem item = i as KeyPressRecorderItem;
                //Waiting
                player.WaitingPlaying(item);
                //Action
                GM.Instance.InputSimulator.Keyboard.KeyPress((VirtualKeyCode)item.VkCode);
            }
            return true;
        }
    }
}
