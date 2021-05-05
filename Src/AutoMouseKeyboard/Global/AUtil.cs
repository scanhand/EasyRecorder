using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using WindowsInput.Native;

namespace AMK.Global
{
    public static class AUtil
    {
        public const double MouseSimulatorMaxValue = 65535.0;

        public const double SimulatorMiniumSleepTimeSec = 0.02; //20 msec

        public static Point ToMouseSimulatorPoint(Point pt)
        {
            double screenWidth = 1920;
            double screenHeight = 1080;
            Screen mainScreen = Screen.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
            if (mainScreen != null)
            {
                screenWidth = mainScreen.Bounds.Width;
                screenHeight = mainScreen.Bounds.Height;
            }
            return new Point(AUtil.MouseSimulatorMaxValue * (pt.X / screenWidth), AUtil.MouseSimulatorMaxValue * (pt.Y / screenHeight));
        }

        public static void MoveToRightBottom(Window window)
        {
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
            double screenWidth = System.Windows.SystemParameters.WorkArea.Width;

            double posX = screenWidth - window.Width;
            double posY = screenHeight - window.Height;

            window.Left = posX;
            window.Top = posY;
        }

        public static void MoveToLeftBottom(Window window)
        {
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
            double screenLeft = System.Windows.SystemParameters.WorkArea.Left;

            double posX = screenLeft;
            double posY = screenHeight - window.Height;

            window.Left = posX;
            window.Top = posY;
        }

        public static void MoveToCenter(Window window)
        {
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
            double screenWidth = System.Windows.SystemParameters.WorkArea.Width;

            double posX = (screenWidth / 2) - (window.Width / 2);
            double posY = (screenHeight / 2) - (window.Height / 2);

            window.Left = posX;
            window.Top = posY;
        }

        public static bool IsStop(AMKState state)
        {
            return (state == AMKState.Stop || state == AMKState.PlayDone);
        }

        public static bool IsStopPause(AMKState state)
        {
            return (state == AMKState.Stop || state == AMKState.PlayDone || state == AMKState.PlayingPause || state == AMKState.RecordingPause);
        }

        public static string ToVKeyToString(int vkCode)
        {
            return ToVKeyToString((VirtualKeyCode)vkCode);
        }

        public static string ToVKeyToString(VirtualKeyCode vkCode)
        {
            if(vkCode >= VirtualKeyCode.VK_0 && vkCode <= VirtualKeyCode.VK_9)
            {
                return string.Format("{0}", (char)('0' + (vkCode - VirtualKeyCode.VK_0)));
            } 
            else if (vkCode >= VirtualKeyCode.VK_A && vkCode <= VirtualKeyCode.VK_Z)
            {
                return string.Format("{0}", (char)('A' + (vkCode - VirtualKeyCode.VK_A)));
            }
            else if(vkCode >= VirtualKeyCode.OEM_1 && vkCode <= VirtualKeyCode.OEM_102)
            {
                switch(vkCode)
                {
                    case VirtualKeyCode.OEM_1:   return string.Format(";");
                    case VirtualKeyCode.OEM_PLUS: return string.Format("+");
                    case VirtualKeyCode.OEM_COMMA: return string.Format(",");
                    case VirtualKeyCode.OEM_MINUS: return string.Format("-");
                    case VirtualKeyCode.OEM_PERIOD: return string.Format(".");
                    case VirtualKeyCode.OEM_2: return string.Format("/");
                    case VirtualKeyCode.OEM_3: return string.Format("`");
                    case VirtualKeyCode.OEM_4: return string.Format("[");
                    case VirtualKeyCode.OEM_5: return string.Format("\\");
                    case VirtualKeyCode.OEM_6: return string.Format("]");
                    case VirtualKeyCode.OEM_7: return string.Format("'");
                    case VirtualKeyCode.OEM_8: return string.Format(" ");
                    case VirtualKeyCode.OEM_102: return string.Format("\\");
                }
                return vkCode.ToString();
            }    
            else
            {
                return vkCode.ToString();
            }
        }

        public static List<VirtualKeyCode> GetVirtualKeyCodes()
        {
            List<VirtualKeyCode> keyCodes = new List<VirtualKeyCode>();

            List<VirtualKeyCode> notSupports = new List<VirtualKeyCode>()
            {
                VirtualKeyCode.LBUTTON, VirtualKeyCode.RBUTTON, VirtualKeyCode.MBUTTON,
                VirtualKeyCode.XBUTTON1, VirtualKeyCode.XBUTTON2,
            };

            foreach (var key in Enum.GetValues(typeof(VirtualKeyCode)))
            {
                VirtualKeyCode vkCode = (VirtualKeyCode)key;
                if (notSupports.IndexOf(vkCode) >= 0)
                    continue;

                keyCodes.Add(vkCode);
            }
            return keyCodes;
        }

    }
}
