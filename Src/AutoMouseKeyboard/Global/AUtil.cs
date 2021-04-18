using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMK.Global
{
    public static class AUtil
    {
        public const double MouseSimulatorMaxValue = 65535.0;

        public const double SimulatorMiniumSleepTimeSec = 0.02; //20 msec

        public static Point ToMouseSimulatorPoint(Point pt)
        {
            //Temp
            double screenWidth = 1920;
            double screenHeight = 1080;
            return new Point(MouseSimulatorMaxValue * (pt.X / screenWidth), MouseSimulatorMaxValue * (pt.Y / screenHeight));
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

            double posX = (screenWidth/2) - (window.Width/2);
            double posY = (screenHeight/2) - (window.Height/2);

            window.Left = posX;
            window.Top = posY;
        }

        public static bool IsStop(AMKState state)
        {
            return (state == AMKState.Stop || state == AMKState.PlayDone || state == AMKState.None);
        }

    }
}
