using ESR.Global;
using System.ComponentModel;

namespace ESR.UI
{
    public class ToastMessageItem : INotifyPropertyChanged
    {
        public ESRState State { get; set; } = ESRState.Stop;

        public string ToastMessage { get; set; } = string.Empty;

        public string ToastImageSource { get; set; } = string.Empty;

        public string ToastBGColor { get; set; } = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void UpdateProperties()
        {
            this.NotifyPropertyChanged("ToastImageSource");
            this.NotifyPropertyChanged("ToastMessage");
            this.NotifyPropertyChanged("ToastBGColor");
        }
    }
}
