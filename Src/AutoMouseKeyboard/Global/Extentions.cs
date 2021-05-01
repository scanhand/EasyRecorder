using System.Threading;
using System.Windows.Threading;

namespace System
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
