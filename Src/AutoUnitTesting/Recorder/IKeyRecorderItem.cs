using System.Windows.Forms;

namespace AUT.Recorder
{
    interface IKeyRecorderItem
    {
        int VkCode { get; set; }
        string Keyname { get; set; }
        string UnicodeCharacter { get; set; }
        Keys ModifierKeys { get; set; }
    }
}
