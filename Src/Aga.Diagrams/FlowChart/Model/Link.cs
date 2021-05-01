using System.ComponentModel;

namespace Aga.Diagrams.FlowChart
{
    public class Link : INotifyPropertyChanged
    {
        [Browsable(false)]
        public FlowNode Source { get; private set; }
        [Browsable(false)]
        public PortKinds SourcePort { get; private set; }
        [Browsable(false)]
        public FlowNode Target { get; private set; }
        [Browsable(false)]
        public PortKinds TargetPort { get; private set; }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        public Link(FlowNode source, PortKinds sourcePort, FlowNode target, PortKinds targetPort)
        {
            Source = source;
            SourcePort = sourcePort;
            Target = target;
            TargetPort = targetPort;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }

    public enum PortKinds { Top, Bottom, Left, Right }
}
