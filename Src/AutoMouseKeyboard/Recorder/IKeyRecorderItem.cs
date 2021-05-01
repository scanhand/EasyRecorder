namespace AMK.Recorder
{
    interface IKeyRecorderItem
    {
        int VkCode { get; set; }
        string Keyname { get; set; }
        string UnicodeCharacter { get; set; }
    }
}
