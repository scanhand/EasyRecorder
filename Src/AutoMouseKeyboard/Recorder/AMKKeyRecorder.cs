using AMK.Global;
using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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

        private bool IsCurrentKeyDown()
        {
            if (this.CurrentRecorder?.Recorder == RecorderType.KeyUpDown && 
                this.CurrentRecorder?.Dir == Dir.Down)
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

        private bool IsCtrlAltShift(Keys key)
        {
            if (key == Keys.Control ||
                key == Keys.Shift ||
                key == Keys.Menu)
                return true;
            return false;
        }

        private IRecorderItem CreateKeyPressRecorderItem(KeyInputEventArgs e)
        {
            return new KeyPressRecorderItem()
            {
                Dir = Dir.Press,
                VkCode = e.KeyData.VkCode,
                Keyname = e.KeyData.Keyname,
                UnicodeCharacter = e.KeyData.UnicodeCharacter,
                ModifierKeys = Control.ModifierKeys
            };
        }

        private List<IRecorderItem> GetDeletePrviousKeyDownItems(KeyInputEventArgs e)
        {
            List<IRecorderItem> deleteItems = new List<IRecorderItem>();
            List<IRecorderItem> keyDownItems = this.AMKRecorder.Items.FindAll(p => p.Recorder == RecorderType.KeyUpDown && p.Dir == Dir.Down);
            foreach (var item in keyDownItems)
            {
                if (IsCtrlAltShift(item))
                    continue;

                IKeyRecorderItem keyItem = item as IKeyRecorderItem;
                if (keyItem.VkCode != e.KeyData.VkCode)
                    continue;

                if ((DateTime.Now - item.GetVeryLastTime()).TotalSeconds < KeyPressIntervalTimeSec)
                {
                    if(!IsCtrlAltShift(keyItem.ModifierKeys))
                        deleteItems.Add(item);
                }
            }
            return deleteItems;
        }

        private List<IRecorderItem> GetPrviousKeyDownItems(KeyInputEventArgs e)
        {
            List<IRecorderItem> deleteItems = new List<IRecorderItem>();
            List<IRecorderItem> keyDownItems = this.AMKRecorder.Items.FindAll(p => p.Recorder == RecorderType.KeyUpDown && p.Dir == Dir.Down);
            foreach (var item in keyDownItems)
            {
                if (IsCtrlAltShift(item))
                    continue;

                IKeyRecorderItem keyItem = item as IKeyRecorderItem;
                if (keyItem.VkCode != e.KeyData.VkCode)
                    continue;

                if ((DateTime.Now - item.GetVeryLastTime()).TotalSeconds < this.KeyPressIntervalTimeSec)
                {
                    if (!IsCtrlAltShift(keyItem.ModifierKeys))
                        deleteItems.Add(item);
                }
            }
            return deleteItems;
        }

        private bool IsSameKeyCodeAndWithInIntervalTime(IRecorderItem item, KeyInputEventArgs e)
        {
            IKeyRecorderItem keyItem = item as IKeyRecorderItem;
            if (keyItem.VkCode == e.KeyData.VkCode &&
                (DateTime.Now - item.Time).TotalSeconds < this.KeyPressIntervalTimeSec)
                return true;
            return false;
        }

        private bool IsIncludedKeyItem(KeyInputEventArgs e)
        {
            if (this.CurrentKeyRecorder?.Recorder != RecorderType.KeyPress)
                return false;

            if (IsSameKeyCodeAndWithInIntervalTime(this.CurrentKeyRecorder, e))
                return true;

            foreach (var item in this.CurrentKeyRecorder.ChildItems)
            {
                if (IsSameKeyCodeAndWithInIntervalTime(item, e))
                    return true;
            }
            return false;
        }

        public void Add(KeyInputEventArgs e)
        {
            IRecorderItem newRecorder = null;
            if (e.KeyData.EventType == KeyEvent.up)
            {
                if (IsIncludedKeyItem(e))
                {
                    ALog.Debug("KeyEvent.Up.IsIncludedKeyItem == true");
                    return;
                }
                
                List<IRecorderItem> prevItems = GetPrviousKeyDownItems(e);
                if(prevItems.Count > 0)
                {
                    //Delete Previous Key up Items
                    foreach (var item in prevItems)
                    {
                        IKeyRecorderItem keyItem = item as IKeyRecorderItem;
                        ALog.Debug("Delete Items::Recorder={0}, VkCode={1}", item.Recorder, keyItem.VkCode);
                        this.AMKRecorder.DeleteItem(item);
                    }

                    //New Key Press
                    this.AMKRecorder.ResetCurrentRecorderbyLast();
                    newRecorder = new KeyPressRecorderItem()
                    {
                        Dir = Dir.Press,
                        VkCode = e.KeyData.VkCode,
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                        ModifierKeys = Control.ModifierKeys
                    };

                    if(this.CurrentRecorder?.Recorder == RecorderType.KeyPress)
                    {
                        ALog.Debug("Add KeyPress into KeyPress.ChildItem");
                        this.AMKRecorder.ResetWaitingTime();
                        this.CurrentRecorder.ChildItems.Add(newRecorder);
                        this.AMKRecorder.UpdateItem(this.CurrentRecorder);
                        return;
                    }
                }
                else
                {
                    ALog.Debug("KeyUp Item");
                    newRecorder = new KeyUpDownRecorderItem()
                    {
                        Dir = Dir.Up,
                        VkCode = e.KeyData.VkCode,
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                        ModifierKeys = Control.ModifierKeys
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

                if (IsCurrentKeyPress() && !IsCtrlAltShift(e.KeyData.VkCode) && !IsCtrlAltShift(Control.ModifierKeys))
                {
                    //New Key Press
                    this.AMKRecorder.ResetCurrentRecorderbyLast();
                    newRecorder = new KeyPressRecorderItem()
                    {
                        Dir = Dir.Press,
                        VkCode = e.KeyData.VkCode,
                        Keyname = e.KeyData.Keyname,
                        UnicodeCharacter = e.KeyData.UnicodeCharacter,
                        ModifierKeys = Control.ModifierKeys
                    };

                    this.AMKRecorder.ResetWaitingTime();
                    this.CurrentRecorder.ChildItems.Add(newRecorder);
                    this.AMKRecorder.UpdateItem(this.CurrentRecorder);
                    return;
                }
                
                newRecorder = new KeyUpDownRecorderItem()
                {
                    Dir = Dir.Down,
                    VkCode = e.KeyData.VkCode,
                    Keyname = e.KeyData.Keyname,
                    UnicodeCharacter = e.KeyData.UnicodeCharacter,
                    ModifierKeys = Control.ModifierKeys
                };
            }

            if(newRecorder != null)
                this.AMKRecorder.AddKeyItem(newRecorder);
        }
    }
}
