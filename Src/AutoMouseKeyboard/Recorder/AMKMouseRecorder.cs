﻿using AMK.Global;
using EventHook;
using EventHook.Hooks;
using System.Diagnostics;

namespace AMK.Recorder
{
    public class AMKMouseRecorder
    {
        public AMKRecorder AMKRecorder { get; set; } = null;

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

        private bool IsCurrentMouseWheel()
        {
            if (this.CurrentRecorder?.Recorder == RecorderType.MouseWheel)
                return true;

            return false;
        }

        private bool IsCurrentMouseMove()
        {
            if (this.CurrentRecorder?.Recorder == RecorderType.MouseMove)
                return true;

            return false;
        }

        private bool IsIgnoreMouseMove(IRecorderItem item)
        {
            MouseMoveRecorderItem mouseItem = item as MouseMoveRecorderItem;
            Trace.Assert(mouseItem != null, "IsIgnoreMouseMove::mouseItem is null");
            if (this.CurrentRecorder?.Recorder == RecorderType.MouseWheel ||
                this.CurrentRecorder?.Recorder == RecorderType.MouseMove ||
                 this.CurrentRecorder?.Recorder == RecorderType.MouseClick)
            {
                if (mouseItem.Point == this.CurrentRecorder?.Point)
                    return true;
            }
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
                    Point = new System.Windows.Point(e.Point.x, e.Point.y),
                    MouseData = (int)e.MouseData,
                };

                if (IsCurrentMouseWheel())
                {
                    this.WaitingRecorder.ResetWaitingTime();
                    this.CurrentRecorder.ChildItems.Add(newRecorder);
                    this.AMKRecorder.UpdateItem(this.CurrentRecorder);
                    return;
                }
            }
            else if (e.Message == MouseMessages.WM_LBUTTONDOWN ||
                     e.Message == MouseMessages.WM_RBUTTONDOWN)
            {
                newRecorder = new MouseDownRecorderItem()
                {
                    Dir = Dir.Down,
                    LR = e.Message == MouseMessages.WM_LBUTTONDOWN ? LR.Left : LR.Right,
                    Point = new System.Windows.Point(e.Point.x, e.Point.y),
                    MouseData = (int)e.MouseData,
                };
            }
            else if (e.Message == MouseMessages.WM_LBUTTONUP ||
                     e.Message == MouseMessages.WM_RBUTTONUP)
            {
                newRecorder = new MouseUpRecorderItem()
                {
                    Dir = Dir.Down,
                    LR = e.Message == MouseMessages.WM_LBUTTONUP ? LR.Left : LR.Right,
                    Point = new System.Windows.Point(e.Point.x, e.Point.y),
                    MouseData = (int)e.MouseData,
                };
            }
            else if (e.Message == MouseMessages.WM_MOUSEMOVE)
            {
                newRecorder = new MouseMoveRecorderItem()
                {
                    Point = new System.Windows.Point(e.Point.x, e.Point.y),
                    MouseData = (int)e.MouseData,
                };

                if (IsIgnoreMouseMove(newRecorder))
                {
                    ALog.Debug("AMKMouseRecorder::Add(MouseEvent)::IsIgnoreMouseMove");
                    return;
                }

                if (IsCurrentMouseMove())
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
