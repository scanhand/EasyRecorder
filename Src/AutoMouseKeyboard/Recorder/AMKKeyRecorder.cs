using AMK.Global;
using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowsInput.Native;

namespace AMK.Recorder
{
    public class AMKKeyRecorder
    {
        private AMKRecorder AMKRecorder { get; set; } = null;

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

        private readonly List<VirtualKeyCode> NeedtoUpDownList = null;

        public AMKKeyRecorder(AMKRecorder recorder)
        {
            this.AMKRecorder = recorder;

            this.NeedtoUpDownList = new List<VirtualKeyCode>()
            {
                VirtualKeyCode.CONTROL,
                VirtualKeyCode.LCONTROL,
                VirtualKeyCode.RCONTROL,
                VirtualKeyCode.SHIFT,
                VirtualKeyCode.LSHIFT,
                VirtualKeyCode.RSHIFT,
                VirtualKeyCode.MENU,
                VirtualKeyCode.LMENU,
                VirtualKeyCode.RMENU,
            };
        }

        private bool IsKeyPress()
        {
            if ((this.CurrentKeyRecorder?.Recorder == RecorderType.KeyUpDown || this.CurrentKeyRecorder?.Recorder == RecorderType.KeyPress) &&
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

        private bool IsLastSameKeyDown(int vkCode)
        {
            if ((this.AMKRecorder.GetLastItem()?.Recorder == RecorderType.KeyUpDown) &&
                this.AMKRecorder.GetLastItem()?.Dir == Dir.Down &&
                (this.AMKRecorder.GetLastItem() as IKeyRecorderItem)?.VkCode == vkCode)
            {
                return true;
            }

            return false;
        }

        private bool IsCtrlAltShift(int vkCode)
        {
            return this.NeedtoUpDownList.Exists(p => p == (VirtualKeyCode)vkCode);
        }

        private bool IsCtrlAltShift(IRecorderItem item)
        {
            IKeyRecorderItem keyItem = item as IKeyRecorderItem;
            return IsCtrlAltShift(keyItem.VkCode);
        }

        public void Add(KeyInputEventArgs e)
        {
            IRecorderItem newRecorder = null;
            if (e.KeyData.EventType == KeyEvent.up)
            {
                if (IsKeyPress() && !IsCtrlAltShift(e.KeyData.VkCode))
                {
                    ALog.Debug("KeyEvent.Up, IsKeyPress: True");
                    if (IsCurrentKeyPress())
                    {
                        newRecorder = new KeyPressRecorderItem()
                        {
                            Dir = Dir.Press,
                            VkCode = e.KeyData.VkCode,
                            Keyname = e.KeyData.Keyname,
                            UnicodeCharacter = e.KeyData.UnicodeCharacter,
                        };

                        this.AMKRecorder.ResetWaitingTime();
                        this.CurrentKeyRecorder.ChildItems.Add(newRecorder);
                        this.AMKRecorder.UpdateItem(this.CurrentKeyRecorder);
                        return;
                    }

                    //Remove items
                    List<IRecorderItem> deleteItems = new List<IRecorderItem>();
                    List<IRecorderItem> keyDownItems = this.AMKRecorder.Items.FindAll(p => p.Recorder == RecorderType.KeyUpDown && p.Dir == Dir.Down);
                    foreach (var item in keyDownItems)
                    {
                        if (IsCtrlAltShift(item))
                            continue;

                        if((DateTime.Now - item.GetVeryLastTime()).TotalSeconds < KeyPressIntervalTimeSec)
                            deleteItems.Add(item);
                    }

                    foreach(var item in deleteItems)
                    {
                        this.AMKRecorder.DeleteItem(item);
                    }
                    this.AMKRecorder.ResetCurrentRecorderbyLast();

                    //New Key Press
                    newRecorder = new KeyPressRecorderItem()
                    {
                        Dir = Dir.Press,
                        VkCode = e.KeyData.VkCode,
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                    };
                }
                else
                {
                    ALog.Debug("KeyEvent.Up, IsKeyPress: False");
                    newRecorder = new KeyUpDownRecorderItem()
                    {
                        Dir = Dir.Up,
                        VkCode = e.KeyData.VkCode,
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                    };
                }
            }
            else
            {
                //it's state on pressing key
                if (IsLastSameKeyDown(e.KeyData.VkCode))
                {
                    ALog.Debug("KeyEvent.Down, IsLastKeyDown: True");
                    return;
                }

                if (IsCurrentKeyPress() && !IsCtrlAltShift(e.KeyData.VkCode))
                {
                    ALog.Debug("KeyEvent.Down, IsCurrentKeyPress: True, IsCtrlAltShift: False");
                    return;
                }

                newRecorder = new KeyUpDownRecorderItem()
                {
                    Dir = Dir.Down,
                    VkCode = e.KeyData.VkCode,
                    Keyname = e.KeyData.Keyname,
                    UnicodeCharacter = e.KeyData.UnicodeCharacter,
                };
            }

            this.AMKRecorder.AddKeyItem(newRecorder);
        }
    }
}
