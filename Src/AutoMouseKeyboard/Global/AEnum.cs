using System.ComponentModel;

namespace AUT.Global
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
        Press,
    }

    public enum ButtonType
    {
        None,
        Left,
        Right,
        Wheel,
    }

    public enum AUTState
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

    public enum DoubleClickActionType
    {
        EditItem,
        Memo,
    }
}
