using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMK.Global
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
