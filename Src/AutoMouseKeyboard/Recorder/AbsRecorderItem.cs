using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AMK.Global;

namespace AMK.Recorder
{
    public abstract class AbsRecorderItem : IRecorderItem, INotifyPropertyChanged
    {
        public string Id { get; set; }

        public RecorderItemState State { get; set; }

        public RecorderType Recorder { get; set; } = RecorderType.None;

        public Dir Dir { get; set; } = Dir.Down;

        public LR LR { get; set; } = LR.None;

        public Point Point { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;

        public string Memo { get; set; } = string.Empty;

        [JsonIgnore]
        public double ResidualTimeSec { get; set; } = 0;

        [JsonIgnore]
        public bool IsSelected
        {
            get
            {
                return this.State == RecorderItemState.Activated;
            }
        }

        [JsonIgnore]
        public string ImageSource
        {
            get
            {
                switch (this.Recorder)
                {
                    case RecorderType.KeyDown:          return "/AutoMouseKeyboard;component/Resources/icons8-keydown-64.png";
                    case RecorderType.KeyUp:            return "/AutoMouseKeyboard;component/Resources/icons8-keyup-64.png";
                    case RecorderType.MouseUp:
                    case RecorderType.MouseDown:
                    case RecorderType.MouseClick:
                        {
                            if(this.LR == LR.Left)
                                return "/AutoMouseKeyboard;component/Resources/icons8-mouse-leftclick-64.png";
                            else
                                return "/AutoMouseKeyboard;component/Resources/icons8-mouse-rightclick-64.png";
                        }
                    case RecorderType.MouseSmartClick:  return "/AutoMouseKeyboard;component/Resources/icons8-smartmouseclick-64.png";
                    case RecorderType.MouseMove:        return "/AutoMouseKeyboard;component/Resources/icons8-cursor-64.png";
                    case RecorderType.MouseWheel:       return "/AutoMouseKeyboard;component/Resources/icons8-mouse-wheel-64.png";
                    case RecorderType.KeyPress:         return "/AutoMouseKeyboard;component/Resources/icons8-keyboard-64.png";
                    case RecorderType.WaitTime:         return "/AutoMouseKeyboard;component/Resources/icons8-timer-64.png";
                    case RecorderType.WaitSmart:        return "/AutoMouseKeyboard;component/Resources/icons8-smarttimer-64.png";
                   

                }
                return string.Empty;
            }
        }

        public List<IRecorderItem> ChildItems { get; set; } = new List<IRecorderItem>();

        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public abstract string Description { get; }

        public abstract bool Play(AMKPlayer player);

        public AbsRecorderItem()
        {
            this.Id = GenerateID();
            this.State = RecorderItemState.None;
        }

        public string GenerateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public bool IsSameRecorderType(IRecorderItem item)
        {
            return this.Recorder == item.Recorder;
        }

        public bool IsEqualType(IRecorderItem item)
        {
            return this.Recorder == item?.Recorder;
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void UpdateProperties()
        {
            this.NotifyPropertyChanged("ImageSource");
            this.NotifyPropertyChanged("Recorder");
            this.NotifyPropertyChanged("Description");
            this.NotifyPropertyChanged("IsSelected");
        }

        public DateTime GetVeryLastTime()
        {
            if (this.ChildItems.Count <= 0)
                return this.Time;

            return this.ChildItems.Last().Time;
        }
    }
}
