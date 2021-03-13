using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public class AMKRecorder
    {
        List<IRecorderItem> Items = new List<IRecorderItem>();

        public void Add(ApplicationEventArgs e)
        {
            
        }

        public void Add(MouseEventArgs e)
        {
            /*
        WM_MOUSEMOVE = 512,
        WM_LBUTTONDOWN = 513,
        WM_LBUTTONUP = 514,
        WM_RBUTTONDOWN = 516,
        WM_RBUTTONUP = 517,
        WM_WHEELBUTTONDOWN = 519,
        WM_WHEELBUTTONUP = 520,
        WM_MOUSEWHEEL = 522,
        WM_XBUTTONDOWN = 523,
        WM_XBUTTONUP = 524
            e.Message  
             */

        }

        public void Add(KeyInputEventArgs e)
        {

        }
    }
}
