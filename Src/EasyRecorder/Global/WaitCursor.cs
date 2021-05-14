using System;
using System.Windows.Input;

namespace ESR.Global
{
    public class WaitCursor : IDisposable
    {
        public WaitCursor()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
        }

        void IDisposable.Dispose()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
    }
}
