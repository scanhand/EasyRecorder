using ESR.Global;
using EventHook;
using EventHook.Hooks;
using System;
using System.Diagnostics;

namespace ESR.Recorder
{
    public class ESRMouseRecorder
    {
        private ESRRecorder ESRRecorder { get; set; } = null;

        private ESRWaitingRecorder WaitingRecorder
        {
            get
            {
                return ESRRecorder.WaitingRecorder;
            }
        }

        private IRecorderItem CurrentRecorder
        {
            get
            {
                return ESRRecorder.CurrentRecorder;
            }
        }

        private IRecorderItem CurrentMouseRecorder
        {
            get
            {
                return ESRRecorder.CurrentMouseRecorder;
            }
        }

        private float MouseClickIntervalTimeSec = 0.5f;

        public ESRMouseRecorder(ESRRecorder recorder)
        {
            this.ESRRecorder = recorder;
        }

        private bool IsCurrentMouseWheelDir(IRecorderItem newRecorderItem)
        {
            if (this.CurrentRecorder?.Recorder == RecorderType.MouseWheel &&
                this.CurrentRecorder.Dir == newRecorderItem.Dir)
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

        private bool IsLastMouseDown()
        {
            if (this.ESRRecorder.GetLastItem()?.Recorder == RecorderType.MouseUpDown &&
                     this.ESRRecorder.GetLastItem()?.Dir == Dir.Down)
                return true;
            return false;
        }

        private ButtonType ToButtonType(MouseMessages message)
        {
            if (message == MouseMessages.WM_RBUTTONDOWN || message == MouseMessages.WM_RBUTTONUP)
            {
                return ButtonType.Right;
            }
            else if (message == MouseMessages.WM_WHEELBUTTONDOWN || message == MouseMessages.WM_WHEELBUTTONUP)
            {
                return ButtonType.Wheel;
            }
            else
            {
                return ButtonType.Left;
            }
        }

        private bool IsMouseButtonPress()
        {
            if ((this.CurrentMouseRecorder?.Recorder == RecorderType.MouseUpDown || this.CurrentMouseRecorder?.Recorder == RecorderType.MouseClick) &&
                (DateTime.Now - this.CurrentMouseRecorder?.GetVeryLastTime()).Value.TotalSeconds < MouseClickIntervalTimeSec)
            {
                return true;
            }

            return false;
        }

        private bool IsCurrentMouseClick()
        {
            if (this.CurrentRecorder?.Recorder == RecorderType.MouseClick)
                return true;

            return false;
        }

        private bool IsCurrentSingleMouseMove(IRecorderItem newItem)
        {
            if (newItem.Recorder == RecorderType.MouseMove)
                return false;

            if (this.CurrentRecorder?.Recorder == RecorderType.MouseMove &&
                this.CurrentRecorder.ChildItems.Count <= 0)
            {
                return true;
            }
            return false;
        }

        public void Add(MouseEventArgs e)
        {
            IRecorderItem newRecorder = null;
            if (e.Message == MouseMessages.WM_MOUSEWHEEL)
            {
                newRecorder = new MouseWheelRecorderItem()
                {
                    Dir = ((int)e.MouseData) > 0 ? Dir.Up : Dir.Down,
                    Point = new System.Windows.Point(e.Point.x, e.Point.y),
                    MouseData = (int)e.MouseData,
                };

                if (IsCurrentMouseWheelDir(newRecorder))
                {
                    this.WaitingRecorder.ResetWaitingTime();
                    this.CurrentRecorder.ChildItems.Add(newRecorder);
                    this.ESRRecorder.UpdateItem(this.CurrentRecorder);
                    return;
                }
            }
            else if (e.Message == MouseMessages.WM_LBUTTONDOWN ||
                     e.Message == MouseMessages.WM_RBUTTONDOWN ||
                     e.Message == MouseMessages.WM_WHEELBUTTONDOWN)
            {
                if (IsLastMouseDown())
                    return;

                if (IsMouseButtonPress())
                {
                    newRecorder = new MouseClickRecorderItem()
                    {
                        Button = ToButtonType(e.Message),
                        Point = new System.Windows.Point(e.Point.x, e.Point.y),
                        MouseData = (int)e.MouseData,
                    };

                    this.ESRRecorder.ResetWaitingTime();
                    this.CurrentMouseRecorder.ChildItems.Add(newRecorder);
                    this.ESRRecorder.UpdateItem(this.CurrentMouseRecorder);
                    return;
                }

                newRecorder = new MouseUpDownRecorderItem()
                {
                    Dir = Dir.Down,
                    Button = ToButtonType(e.Message),
                    Point = new System.Windows.Point(e.Point.x, e.Point.y),
                    MouseData = (int)e.MouseData,
                };
            }
            else if (e.Message == MouseMessages.WM_LBUTTONUP ||
                     e.Message == MouseMessages.WM_RBUTTONUP ||
                     e.Message == MouseMessages.WM_WHEELBUTTONUP)
            {
                if (IsMouseButtonPress())
                {
                    ALog.Debug("MouseEvent.Up, IsMouseButtonPress: True");
                    if (IsCurrentMouseClick())
                        return;

                    //Remove MouseDown item
                    this.ESRRecorder.DeleteItem(this.CurrentMouseRecorder);
                    this.ESRRecorder.ResetCurrentRecorderbyLast();

                    newRecorder = new MouseClickRecorderItem()
                    {
                        Button = ToButtonType(e.Message),
                        Point = new System.Windows.Point(e.Point.x, e.Point.y),
                        MouseData = (int)e.MouseData,
                    };
                }
                else
                {
                    newRecorder = new MouseUpDownRecorderItem()
                    {
                        Dir = Dir.Up,
                        Button = ToButtonType(e.Message),
                        Point = new System.Windows.Point(e.Point.x, e.Point.y),
                        MouseData = (int)e.MouseData,
                    };
                }
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
                    ALog.Debug("ESRMouseRecorder::Add(MouseEvent)::IsIgnoreMouseMove");
                    return;
                }

                if (IsCurrentMouseMove())
                {
                    this.WaitingRecorder.ResetWaitingTime();
                    this.CurrentMouseRecorder.ChildItems.Add(newRecorder);
                    this.ESRRecorder.UpdateItem(this.CurrentMouseRecorder);
                    return;
                }
            }

            //Need to delete Unnecessary a mouse move item
            if (IsCurrentSingleMouseMove(newRecorder))
                this.ESRRecorder.DeleteItem(this.CurrentRecorder);

            this.ESRRecorder.AddMouseItem(newRecorder);
        }
    }
}
