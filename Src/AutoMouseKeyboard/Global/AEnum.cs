using System.ComponentModel;

namespace AMK.Global
{
    public enum RecorderType
    {
        None,
        [Description("Mouse Move")]
        MouseMove,
        [Description("Mouse UpDown")]
        MouseUpDown,
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
        [Description("Key UpDown")]
        KeyUpDown,
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
