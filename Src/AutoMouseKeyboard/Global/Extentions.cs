﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Threading;

namespace AMK
{
    public static class WPFThreadingExtensions
    {
        /// <summary>
        /// Simple helper extension method to marshall to correct
        /// thread if its required
        /// </summary>
        /// <param name="""control""">The source control</param>
        /// <param name="""methodcall""">The method to call</param>
        /// <param name="""priorityForCall""">The thread priority</param>
        public static void InvokeIfRequired(
            this DispatcherObject control,
            Action methodcall)
        {
            //see if we need to Invoke call to Dispatcher thread
            if (control.Dispatcher.Thread != Thread.CurrentThread)
                control.Dispatcher.Invoke(methodcall, DispatcherPriority.Render);
            else
                methodcall();
        }
    }
}