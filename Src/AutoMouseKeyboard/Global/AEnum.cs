﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public enum RecorderType
    {
        None,
        MouseMove,
        MouseDown,
        MouseUp,
        MouseClick,
        MouseWheel,
        MouseSmartClick,
        KeyPress,
        KeyHotkey,
        KeyDown,
        KeyUp,
        WaitTime,
        WaitSmart,
        Application,
    }

    public enum Dir
    {
        Up,
        Down,
    }

    public enum LR
    {
        None,
        Left,
        Right,
    }

    public enum AMKState
    {
        None,
        Recording,
        Pause,
        Stop,
        Playing,
    }

    public enum RecorderItemState
    {
        None,
        Activated,
    }
}
