using AMK.Global;
using WindowsInput.Native;

namespace AMK.Recorder
{
    public class KeyUpRecorderItem : AbsRecorderItem, IKeyRecorderItem
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
