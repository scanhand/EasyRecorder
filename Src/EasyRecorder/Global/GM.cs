using WindowsInput;

namespace ESR.Global
{
    public class GM : SingletonBase<GM>
    {
        public bool IsTest = true;

        public InputSimulator InputSimulator = new InputSimulator();

        public MainWindow MainWindow { get; set; } = null;

        public GM()
        {

        }
    }
}
