using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ESR.UI
{
    public class KeyPressData : INotifyPropertyChanged
    {
        private int _Index = 0;
        public int Index
        {
            get { return _Index; }
            set
            {
                _Index = value;
                RaiseProperChanged();
            }
        }

        private KeyItem _Key;
        public KeyItem Key
        {
            get { return _Key; }
            set
            {
                _Key = value;
                RaiseProperChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaiseProperChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
