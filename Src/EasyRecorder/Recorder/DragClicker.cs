using ESR.Global;
using EventHook.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESR.Recorder
{
    public class DragClicker
    {
        private bool IsIgnoreMouseRecorder = false;

        private int RemainedIgnoreMouseButtonDownCount = 10;

        public void MouseInput(EventHook.MouseEventArgs e)
        {
            if (e.Message == GetMouseMessages())
            {
                if (this.IsIgnoreMouseRecorder)
                {
                    this.RemainedIgnoreMouseButtonDownCount--;
                    if (this.RemainedIgnoreMouseButtonDownCount >= 0)
                    {
                        ALog.Debug("MouseWatcher::IsIgnoreMouseRecorder is true!");
                        return;
                    }
                }

                this.IsIgnoreMouseRecorder = true;
                this.RemainedIgnoreMouseButtonDownCount = Preference.Instance.DragClickNumberOfClicks;
                new Thread(() => {
                    for (int i = 0; i < Preference.Instance.DragClickNumberOfClicks; i++)
                    {
                        GM.Instance.InputSimulator.Mouse.RightButtonClick();
                        Thread.Sleep(Preference.Instance.DragClickClickTimeIntervalMsec);
                    }
                }).Start();
            }
        }

        private MouseMessages GetMouseMessages()
        {
            if (Preference.Instance.DragClickButtonType == ButtonType.Left)
                return MouseMessages.WM_LBUTTONDOWN;
            else
                return MouseMessages.WM_RBUTTONDOWN;
        }
    }
}
