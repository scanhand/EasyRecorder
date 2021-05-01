using System.ComponentModel;

namespace AMK.Global
{
    public enum RecorderType
    {
        None,
        [Description("Mouse Move")]
        MouseMove,
        [Description("Mouse Down")]
        MouseDown,
        [Description("Mouse Up")]
        MouseUp,
        [Description("Mouse Click")]
        MouseClick,
        [Description("Mouse Wheel")]
        MouseWheel,
        [Description("Mouse Smart Click")]
        MouseSmartClick,
        [Description("Key Press")]
        KeyPress,
        [Description("Hot Key Press")]
        KeyHotkey,
        [Description("Key Down")]
        KeyDown,
        [Description("Key Up")]
        KeyUp,
        [Description("Waiting Time")]
        WaitTime,
        [Description("Waiting Smart")]
        WaitSmart,
        [Description("Application")]
        Application,
    }

    public enum Dir
    {
        Up,
        Down,
    }

    public enum ButtonType
    {
        None,
        Left,
        Right,
        Wheel,
    }

    public enum AMKState
    {
        Recording,
        RecordingPause,
        Stop,
        Playing,
        PlayingPause,
        PlayDone,
    }

    public enum RecorderItemState
    {
        None,
        Activated,
    }
}
