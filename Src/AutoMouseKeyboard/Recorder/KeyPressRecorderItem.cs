using AMK.Global;
using System.Text;
using WindowsInput.Native;

namespace AMK.Recorder
{
    public class KeyPressRecorderItem : AbsRecorderItem, IKeyRecorderItem
    {
        public int VkCode { get; set; }
        public string Keyname { get; set; }
        public string UnicodeCharacter { get; set; }

        public override string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Keyname))
                    sb.AppendFormat("[{0}]", this.Keyname);
                foreach (var i in this.ChildItems)
                {
                    KeyPressRecorderItem item = i as KeyPressRecorderItem;
                    if (item == null)
                        continue;
                    if (!string.IsNullOrEmpty(this.Keyname))
                        sb.AppendFormat("[{0}]", item.Keyname);
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
