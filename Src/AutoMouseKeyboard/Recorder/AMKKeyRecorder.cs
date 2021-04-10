using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    public class AMKKeyRecorder
    {
        public AMKRecorder AMKRecorder { get; set; } = null;

        private float KeyPressIntervalTimeSec = 0.5f;

        private AMKWaitingRecorder WaitingRecorder
        {
            get
            {
                return AMKRecorder.WaitingRecorder;
            }
        }

        private IRecorderItem CurrentRecorder
        {
            get
            {
                return AMKRecorder.CurrentRecorder;
            }
        }

        private IRecorderItem CurrentKeyRecorder
        {
            get
            {
                return AMKRecorder.CurrentKeyRecorder;
            }
        }

        public AMKKeyRecorder(AMKRecorder recorder)
        {
            this.AMKRecorder = recorder;
        }

        private bool IsKeyPress()
        {
            if ((this.CurrentKeyRecorder?.Recorder == RecorderType.KeyDown || this.CurrentKeyRecorder?.Recorder == RecorderType.KeyPress) &&
                (DateTime.Now - this.CurrentKeyRecorder?.GetVeryLastTime()).Value.TotalSeconds < KeyPressIntervalTimeSec)
            {
                return true;
            }

            return false;
        }

        private bool IsCurrentKeyPress()
        {
            if (this.CurrentRecorder?.Recorder == RecorderType.KeyPress)
                return true;

            return false;
        }

        public void Add(KeyInputEventArgs e)
        {
            IRecorderItem newRecorder = null;
            if (e.KeyData.EventType == KeyEvent.up)
            {
                if (IsKeyPress())
                {
                    ALog.Debug("KeyEvent.Up, IsKeyPress: True");
                    if (IsCurrentKeyPress())
                        return;

                    //Remove KeyDown item
                    this.AMKRecorder.DeleteItem(this.CurrentKeyRecorder);
                    this.AMKRecorder.ResetCurrentRecorder();

                    newRecorder = new KeyPressRecorderItem()
                    {
                        VkCode = e.KeyData.VkCode,
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                    };
                }
                else
                {
                    ALog.Debug("KeyEvent.Up, IsKeyPress: False");
                    newRecorder = new KeyUpRecorderItem()
                    {
                        VkCode = e.KeyData.VkCode,
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                    };
                }
            }
            else
            {
                //it's state on pressing key
                if (this.AMKRecorder.GetLastItem()?.Recorder == RecorderType.KeyDown)
                    return;

                if(IsCurrentKeyPress())
                {
                    newRecorder = new KeyPressRecorderItem()
                    {
                        VkCode = e.KeyData.VkCode,
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                    };

                    this.AMKRecorder.ResetWaitingTime();
                    this.CurrentKeyRecorder.ChildItems.Add(newRecorder);
                    this.AMKRecorder.UpdateItem(CurrentKeyRecorder);
                    return;
                }

                newRecorder = new KeyDownRecorderItem()
                {
                    VkCode = e.KeyData.VkCode,
                    Keyname = e.KeyData.Keyname,
                    UnicodeCharacter = e.KeyData.UnicodeCharacter,
                };
            }

            this.AMKRecorder.AddKeyItem(newRecorder);
        }
    }
}
