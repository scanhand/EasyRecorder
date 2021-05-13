using EventHook;

namespace AUT.Recorder
{
    public class AUTApplicationRecorder
    {
        private AUTRecorder AUTRecorder { get; set; } = null;

        public AUTApplicationRecorder(AUTRecorder recorder)
        {
            this.AUTRecorder = recorder;
        }

        public void Add(ApplicationEventArgs e)
        {
            IRecorderItem newRecorder = null;
            newRecorder = new ApplicationRecorderItem()
            {
                ApplicationData = e.ApplicationData,
                Event = e.Event,
            };

            this.AUTRecorder.AddItem(newRecorder);
            this.AUTRecorder.CurrentRecorder = newRecorder;
        }

    }
}
