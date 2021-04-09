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

        public const double MouseSimulatorMiniumSleepTimeSec = 0.02; //20 msec

        public static Point ToMouseSimulatorPoint(Point pt)
        {
            //Temp
            double screenWidth = 1920;
            double screenHeight = 1080;
            return new Point(MouseSimulatorMaxValue * (pt.X / screenWidth), MouseSimulatorMaxValue * (pt.Y / screenHeight));
        }
    }
}
