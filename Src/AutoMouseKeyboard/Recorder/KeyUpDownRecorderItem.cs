using AMK.Global;
using System.Windows;
using WindowsInput.Native;

namespace AMK.Recorder
{
    public class KeyUpDownRecorderItem : AbsRecorderItem, IKeyRecorderItem
    {
        public int VkCode { get; set; } = (int)VirtualKeyCode.SPACE;
        public string Keyname { get; set; }
        public string UnicodeCharacter { get; set; }

        public override string Description
        {
            get
            {
                return string.Format("[{0}]", this.Keyname);
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

        public override bool Play(AMKPlayer player)
        {
            //Waiting
            player.WaitingPlaying(this);

            //Action
            Point pt = AUtil.ToMouseSimulatorPoint(this.Point);
            GM.Instance.InputSimulator.Mouse.MoveMouseTo(pt.X, pt.Y);

            if (this.Dir == Dir.Up)
                GM.Instance.InputSimulator.Keyboard.KeyUp((VirtualKeyCode)this.VkCode);
            else if (this.Dir == Dir.Down)
                GM.Instance.InputSimulator.Keyboard.KeyDown((VirtualKeyCode)this.VkCode);
            return true;
        }
    }
}
