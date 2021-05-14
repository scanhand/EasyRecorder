using EventHook;

namespace ESR.Recorder
{
    public class ESRApplicationRecorder
    {
        private ESRRecorder ESRRecorder { get; set; } = null;

        public ESRApplicationRecorder(ESRRecorder recorder)
        {
            this.ESRRecorder = recorder;
        }

        public void Add(ApplicationEventArgs e)
        {
            IRecorderItem newRecorder = null;
            newRecorder = new ApplicationRecorderItem()
            {
                ApplicationData = e.ApplicationData,
                Event = e.Event,
            };

            this.ESRRecorder.AddItem(newRecorder);
            this.ESRRecorder.CurrentRecorder = newRecorder;
        }

    }
}
