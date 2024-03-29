﻿using System.ComponentModel;

namespace ESR.UI
{
    public class ESRStatusBarItem : INotifyPropertyChanged
    {
        public string StatusImageSource { get; set; }

        public string StatusText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void UpdateProperties()
        {
            this.NotifyPropertyChanged("StatusImageSource");
            this.NotifyPropertyChanged("StatusText");
        }
    }
}
