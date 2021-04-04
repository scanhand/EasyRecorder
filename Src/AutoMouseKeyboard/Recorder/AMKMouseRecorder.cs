using EventHook;
using EventHook.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK.Recorder
{
    public class AMKMouseRecorder
    {
        public AMKRecorder AMKRecorder { get; set; }

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

        private IRecorderItem CurrentMouseRecorder
        {
            get
            {
                return AMKRecorder.CurrentMouseRecorder;
            }
        }

        public AMKMouseRecorder(AMKRecorder recorder)
        {
            this.AMKRecorder = recorder;
        }

        private bool IsMouseWheel()
        {
            if (this.CurrentRecorder?.Recorder == RecorderType.MouseWheel)
                return true;

            return false;
        }

        private bool IsMouseMove()
        {
            if (this.CurrentRecorder?.Recorder == RecorderType.MouseMove)
                return true;

            return false;
        }

        public void Add(MouseEventArgs e)
        {
            IRecorderItem newRecorder = null;

            if (e.Message == MouseMessages.WM_WHEELBUTTONUP ||
                e.Message == MouseMessages.WM_WHEELBUTTONDOWN ||
                e.Message == MouseMessages.WM_MOUSEWHEEL)
            {
                newRecorder = new MouseWheelRecorderItem()
                {
                    Dir = ((int)e.MouseData) > 0 ? Dir.Up : Dir.Down,
                    Point = e.Point,
                    MouseData = e.MouseData,
                };

                if (IsMouseWheel())
                {
                    this.WaitingRecorder.ResetWaitingTime();
                    this.CurrentRecorder.ChildItems.Add(newRecorder);
                    this.AMKRecorder.UpdateItem(this.CurrentRecorder);
                    return;
                }
            }
            else if (e.Message == MouseMessages.WM_LBUTTONUP ||
                    e.Message == MouseMessages.WM_LBUTTONDOWN ||
                    e.Message == MouseMessages.WM_RBUTTONUP ||
                    e.Message == MouseMessages.WM_RBUTTONDOWN)
            {
                newRecorder = new MouseClickRecorderItem()
                {
                    Dir = e.Message == MouseMessages.WM_LBUTTONUP ? Dir.Up : Dir.Down,
                    LR = (e.Message == MouseMessages.WM_LBUTTONUP || e.Message == MouseMessages.WM_LBUTTONDOWN) ? LR.Left : LR.Right,
                    Point = e.Point,
                    MouseData = e.MouseData,
                };
            }
            else if (e.Message == MouseMessages.WM_MOUSEMOVE)
            {
                newRecorder = new MouseMoveRecorderItem()
                {
                    Point = e.Point,
                    MouseData = e.MouseData,
                };

                if (IsMouseMove())
                {
                    this.WaitingRecorder.ResetWaitingTime();
                    this.CurrentMouseRecorder.ChildItems.Add(newRecorder);
                    this.AMKRecorder.UpdateItem(this.CurrentMouseRecorder);
                    return;
                }
            }
            this.AMKRecorder.AddMouseItem(newRecorder);
        }
    }
}
