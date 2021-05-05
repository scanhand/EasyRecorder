using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace AMK.UI
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
