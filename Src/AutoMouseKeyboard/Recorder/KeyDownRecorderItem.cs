using AMK.Global;
using WindowsInput.Native;

namespace AMK.Recorder
{
    public class KeyDownRecorderItem : AbsRecorderItem, IKeyRecorderItem
    {
        public int VkCode { get; set; }
        public string Keyname { get; set; }
        public string UnicodeCharacter { get; set; }

        public override string Description
        {
            get
            {
                return string.Format("[{0}]", this.Keyname);
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
