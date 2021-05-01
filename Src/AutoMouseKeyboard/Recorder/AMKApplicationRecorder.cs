using EventHook;

namespace AMK.Recorder
{
    public class AMKApplicationRecorder
    {
        public AMKRecorder AMKRecorder { get; set; } = null;

        public AMKApplicationRecorder(AMKRecorder recorder)
        {
            this.AMKRecorder = recorder;
        }

        public void Add(ApplicationEventArgs e)
        {
            IRecorderItem newRecorder = null;
            newRecorder = new ApplicationRecorderItem()
            {
                ApplicationData = e.ApplicationData,
                Event = e.Event,
            };

            this.AMKRecorder.AddItem(newRecorder);
            this.AMKRecorder.CurrentRecorder = newRecorder;
        }

    }
}
