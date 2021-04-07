using AMK.Recorder;
using EventHook;
using EventHook.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    public class AMKRecorder
    {
        public List<IRecorderItem> Items = new List<IRecorderItem>();

        public IRecorderItem CurrentRecorder = null;

        public IRecorderItem CurrentKeyRecorder = null;

        public IRecorderItem CurrentMouseRecorder = null;

        public AMKMouseRecorder MouseRecorder = null;

        public AMKWaitingRecorder WaitingRecorder = null;

        public AMKKeyRecorder KeyRecorder = null;

        public AMKApplicationRecorder ApplicationRecorder = null;

        public Action<IRecorderItem> OnAddItem = null;

        public Action<IRecorderItem> OnUpdateItem = null;

        public Action<IRecorderItem, IRecorderItem> OnReplaceItem = null;

        public AMKRecorder()
        {
            this.MouseRecorder = new AMKMouseRecorder(this);
            this.KeyRecorder = new AMKKeyRecorder(this);
            this.WaitingRecorder = new AMKWaitingRecorder(this);
            this.ApplicationRecorder = new AMKApplicationRecorder(this);
        }

        public void Start()
        {
            this.WaitingRecorder.Start();
        }

        public void Stop()
        {
            this.WaitingRecorder.Stop();
        }

        public void Add(ApplicationEventArgs e)
        {
            this.ApplicationRecorder.Add(e);
        }

        public void Add(MouseEventArgs e)
        {
            this.MouseRecorder.Add(e);
        }

        public void Add(KeyInputEventArgs e)
        {
            this.KeyRecorder.Add(e);
        }

        public IRecorderItem GetLastItem(bool isIgnoreWaitItem = true)
        {
            for(int i=Items.Count-1; i>=0; i--)
            {
                if (isIgnoreWaitItem && Items[i].Recorder == RecorderType.WaitTime)
                    continue;

                return Items[i];
            }
            return null;
        }

        public void AddItem(IRecorderItem item)
        {
            this.WaitingRecorder.ResetWaitingTime();
            this.CurrentRecorder = item;
            this.Items.Add(item);
            if (OnAddItem != null)
                OnAddItem(item);
        }

        public void AddMouseItem(IRecorderItem item)
        {
            this.CurrentMouseRecorder = item;
            AddItem(item);
        }

        public void AddKeyItem(IRecorderItem item)
        {
            this.CurrentKeyRecorder = item;
            AddItem(item);
        }

        public bool ReplaceItem(IRecorderItem oldItem, IRecorderItem newItem)
        {
            int index = this.Items.IndexOf(oldItem);
            if (index < 0 || index >= this.Items.Count)
            {
                ALog.Debug("ReplaceItem::Index is invalide!(Index={0})", index);
                return false;
            }

            this.CurrentRecorder = newItem;
            this.Items[index] = newItem;
            if (OnReplaceItem != null)
                OnReplaceItem(oldItem, newItem);

            return true;
        }

        public bool ReplaceKeyItem(IRecorderItem oldItem, IRecorderItem newItem)
        {
            this.CurrentKeyRecorder = newItem;
            return ReplaceItem(oldItem, newItem);
        }

        public bool ReplaceMouseItem(IRecorderItem oldItem, IRecorderItem newItem)
        {
            this.CurrentMouseRecorder = newItem;
            return ReplaceItem(oldItem, newItem);
        }

        public void UpdateItem(IRecorderItem item)
        {
            if (OnUpdateItem != null)
                OnUpdateItem(item);
        }
    }
}
