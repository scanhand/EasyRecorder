using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace AMK.UI
{
    public class AMKCommander
    {
        public VirtualKeyCode RecordingVKeyCode = VirtualKeyCode.F4;

        public VirtualKeyCode PlayingVKeyCode = VirtualKeyCode.F5;

        public Action OnRecording = null;

        public Action OnPlaying = null;

        public AMKCommander()
        {

        }

        public bool ProcessKey(KeyInputEventArgs e)
        {
            if(e.KeyData.VkCode == (int)this.RecordingVKeyCode)
            {
                if (e.KeyData.EventType != KeyEvent.up)
                    OnRecording();
                return true;
            }
            else if (e.KeyData.VkCode == (int)this.PlayingVKeyCode)
            {
                if (e.KeyData.EventType != KeyEvent.up)
                    OnPlaying();
                return true;
            }
            return false;
        }
    }
}
