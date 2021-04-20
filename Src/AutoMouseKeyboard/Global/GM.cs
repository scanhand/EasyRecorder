using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace AMK.Global
{
    public class GM : SingletonBase<GM>
    {
        public InputSimulator InputSimulator = new InputSimulator();

        public MainWindow MainWindow { get; set; } = null;

        public GM()
        {

        }
    }
}
