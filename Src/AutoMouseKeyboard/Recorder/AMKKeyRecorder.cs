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

        private float KeyPressIntervalTimeSec = 0.2f;

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
                (DateTime.Now - this.CurrentKeyRecorder?.Time).Value.TotalSeconds < KeyPressIntervalTimeSec)
            {
                return true;
            }

            return false;
        }

        public void Add(KeyInputEventArgs e)
        {
            IRecorderItem newRecorder = null;
            if (e.KeyData.EventType == KeyEvent.up)
            {
                ALog.Debug("e.KeyData.EventType == KeyEvent.up");
                if (IsKeyPress())
                {
                    ALog.Debug("IsKeyPress::True!");
                    newRecorder = new KeyPressRecorderItem()
                    {
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                    };

                    if (this.CurrentKeyRecorder?.Recorder == RecorderType.KeyPress)
                    {
                        this.CurrentKeyRecorder.ChildItems.Add(newRecorder);
                        this.AMKRecorder.UpdateItem(CurrentKeyRecorder);
                        return;
                    }

                    this.AMKRecorder.ReplaceKeyItem(this.CurrentRecorder, newRecorder);
                    return;
                }
                else
                {
                    ALog.Debug("IsKeyPress::False!");
                    newRecorder = new KeyUpRecorderItem()
                    {
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

                newRecorder = new KeyDownRecorderItem()
                {
                    Keyname = e.KeyData.Keyname,
                    UnicodeCharacter = e.KeyData.UnicodeCharacter,
                };
            }

            this.AMKRecorder.AddKeyItem(newRecorder);
        }
    }
}
