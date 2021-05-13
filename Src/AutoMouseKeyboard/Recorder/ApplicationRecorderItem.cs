using AUT.Global;
using EventHook;

namespace AUT.Recorder
{
    public class ApplicationRecorderItem : AbsRecorderItem
    {
        public WindowData ApplicationData { get; set; } = new WindowData();
        public ApplicationEvents Event { get; set; }

        public override string Description
        {
            get
            {
                return string.Format($"{this.Event.ToString()}\t{this.ApplicationData.AppTitle}\t{this.ApplicationData.AppPath}\t{this.ApplicationData.AppName}");
            }
        }

        public ApplicationRecorderItem()
        {
            this.Recorder = RecorderType.Application;
        }

        public override bool Play(AUTPlayer player)
        {
            return true;
        }
    }
}
