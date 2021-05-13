using AUT.Global;
using System.Windows;
using System.Windows.Forms;
using WindowsInput.Native;

namespace AUT.Recorder
{
    public class KeyUpDownRecorderItem : AbsRecorderItem, IKeyRecorderItem
    {
        public int VkCode { get; set; } = (int)VirtualKeyCode.SPACE;
        public string Keyname { get; set; }
        public string UnicodeCharacter { get; set; }
        public Keys ModifierKeys { get; set; }

        public override string Description
        {
            get
            {
                return string.Format("{0}", AUtil.ToVKeyToString(this.VkCode));
            }
        }

        public override string RecorderDesc
        {
            get
            {
                string desc = string.Empty;
                if (this.Dir == Dir.Up)
                    desc = "Key Up";
                else
                    desc = "Key Down";
                return desc;
            }
        }

        public KeyUpDownRecorderItem()
        {
            this.Recorder = RecorderType.KeyUpDown;
            this.Dir = Dir.Up;
        }

        public override bool Play(AUTPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);

            //Action
            if (this.Dir == Dir.Up)
                GM.Instance.InputSimulator.Keyboard.KeyUp((VirtualKeyCode)this.VkCode);
            else if (this.Dir == Dir.Down)
                GM.Instance.InputSimulator.Keyboard.KeyDown((VirtualKeyCode)this.VkCode);
            return true;
        }
    }
}
