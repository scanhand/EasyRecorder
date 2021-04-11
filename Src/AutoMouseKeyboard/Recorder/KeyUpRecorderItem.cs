using AMK.Global;
using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace AMK.Recorder
{
    public class KeyUpRecorderItem : AbsRecorderItem
    {
        public int VkCode;
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

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);
            //Action
            GM.Instance.InputSimulator.Keyboard.KeyUp((VirtualKeyCode)this.VkCode);
            return true;
        }
    }
}
