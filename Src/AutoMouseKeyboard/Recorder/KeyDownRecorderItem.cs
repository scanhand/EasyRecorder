using AMK.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace AMK.Recorder
{
    public class KeyDownRecorderItem : AbsRecorderItem
    {
        public int VkCode;
        public string Keyname;
        public string UnicodeCharacter;

        public override string Description
        {
            get
            {
                return string.Format($"{this.Keyname}");
            }
        }

        public KeyDownRecorderItem()
        {
            this.Recorder = RecorderType.KeyDown;
            this.Dir = Dir.Down;
        }

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            GM.Instance.InputSimulator.Keyboard.KeyDown((VirtualKeyCode)this.VkCode);
            return true;
        }
    }
}
