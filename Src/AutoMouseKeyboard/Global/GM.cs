﻿using WindowsInput;

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
