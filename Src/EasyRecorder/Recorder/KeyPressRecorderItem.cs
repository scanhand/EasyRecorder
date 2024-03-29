﻿using ESR.Global;
using System.Text;
using System.Windows.Forms;
using WindowsInput.Native;

namespace ESR.Recorder
{
    public class KeyPressRecorderItem : AbsRecorderItem, IKeyRecorderItem
    {
        public int VkCode { get; set; } = (int)VirtualKeyCode.SPACE;
        public string Keyname { get; set; }
        public string UnicodeCharacter { get; set; }
        public Keys ModifierKeys { get; set; }

        public override string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}", AUtil.ToVKeyToString(this.VkCode, true));
                foreach (var i in this.ChildItems)
                {
                    KeyPressRecorderItem item = i as KeyPressRecorderItem;
                    if (item == null)
                        continue;
                    sb.AppendFormat("{0}", AUtil.ToVKeyToString(item.VkCode, true));
                }
                return sb.ToString();
            }
        }

        public KeyPressRecorderItem()
        {
            this.Recorder = RecorderType.KeyPress;
        }

        public override bool Play(ESRPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);

            //Action
            ActionVkCode(this);
            foreach (var i in this.ChildItems)
            {
                if (!player.IsThreadEnable)
                    return false;

                KeyPressRecorderItem item = i as KeyPressRecorderItem;
                //Waiting
                player.WaitingPlaying(item);
                //Action
                ActionVkCode(item);
            }
            return true;
        }

        private void ActionVkCode(KeyPressRecorderItem item)
        {
            GM.Instance.InputSimulator.Keyboard.KeyPress((VirtualKeyCode)item.VkCode);
        }
    }
}
