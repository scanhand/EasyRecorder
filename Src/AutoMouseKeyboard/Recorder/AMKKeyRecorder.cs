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

        private IRecorderItem GetPrviousKeyDownItem(KeyInputEventArgs e)
        {
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
                        return item;
                }
            }
            return null;
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
            List<IRecorderItem> keyDownItems = this.AMKRecorder.Items.FindAll(p => p.Recorder == RecorderType.KeyPress);
            foreach (var item in keyDownItems)
            {
                if (IsSameKeyCodeAndWithInIntervalTime(item, e))
                    return true;

                foreach (var childItem in this.CurrentKeyRecorder.ChildItems)
                {
                    if (IsSameKeyCodeAndWithInIntervalTime(childItem, e))
                        return true;
                }
            }
            return false;
        }

        private void ReplaceKeyDownToKeyPress(IRecorderItem item)
        {
            IKeyRecorderItem keyItem = item as IKeyRecorderItem;
            IRecorderItem newItem = new KeyPressRecorderItem()
            {
                Dir = Dir.Press,
                VkCode = keyItem.VkCode,
                Keyname = keyItem.Keyname,
                UnicodeCharacter = keyItem.UnicodeCharacter,
                ModifierKeys = keyItem.ModifierKeys
            };
            this.AMKRecorder.ReplaceItem(item, newItem);
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

                IRecorderItem prevRecorder = GetPrviousKeyDownItem(e);
                if(prevRecorder != null)
                {
                    //Delete Previous Key up Items
                    ALog.Debug("Delete Items::Recorder={0}, VkCode={1}", prevRecorder.Recorder, AUtil.ToVKeyToString((prevRecorder as IKeyRecorderItem).VkCode));
                    this.AMKRecorder.DeleteItem(prevRecorder);

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

                    //If Current Is KeyPress, this KeyPress add into ChildItem
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

                //If current is KeyPress, Add a item into ChildItem
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

                //If Current is Key Down, It means continuosly 2 item's Type is KeyDown.
                //So that, 1) Replace Previous KeyDown item to KeyPress
                //         2) Add a this time KeyDown item into Child Item
                if(IsCurrentKeyDown() && !IsCtrlAltShift(e.KeyData.VkCode) && !IsCtrlAltShift(Control.ModifierKeys))
                {
                    ReplaceKeyDownToKeyPress(this.CurrentRecorder);

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
                
                //New Key Down
                newRecorder = new KeyUpDownRecorderItem()
                {
                    Dir = Dir.Down,
                    VkCode = e.KeyData.VkCode,
                    Keyname = e.KeyData.Keyname,
                    UnicodeCharacter = e.KeyData.UnicodeCharacter,
                    ModifierKeys = Control.ModifierKeys
                };
            }

            this.AMKRecorder.AddKeyItem(newRecorder);
        }
    }
}
