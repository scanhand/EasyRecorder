using AMK.Global;
using AMK.Recorder;
using AMK.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMK.Recorder
{
    public class AMKRecorderItemConfigManager : SingletonBase<AMKRecorderItemConfigManager>
    {
        public Action<IRecorderItem, IRecorderItem> OnReplaceItem = null;

        public void ShowConfigWindow(IRecorderItem item)
        {
            IRecorderItemConfig config = CreateRecorderItemConfig(item);
            config.RecorderItem = item.Copy();
            Window window = config as Window;
            window.Owner = GM.Instance.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (window.ShowDialog() == false)
                return;

            IRecorderItem updatedItem = config.RecorderItem.Copy();
            OnReplaceItem(item, updatedItem);
        }

        private IRecorderItemConfig CreateRecorderItemConfig(IRecorderItem item)
        {
            IRecorderItemConfig recorderItemConfig = null;
            switch(item.Recorder)
            {
                case RecorderType.WaitTime: recorderItemConfig = new WaitingTimeRecorderItemConfig(); break;
                case RecorderType.MouseClick: recorderItemConfig = new MouseClickRecorderItemConfig(); break;
                case RecorderType.MouseUp: recorderItemConfig = new MouseClickRecorderItemConfig(); break;
                case RecorderType.MouseDown: recorderItemConfig = new MouseClickRecorderItemConfig(); break;
                case RecorderType.KeyDown: recorderItemConfig = new KeyUpDownRecorderItemConfig(); break;
                case RecorderType.KeyUp: recorderItemConfig = new KeyUpDownRecorderItemConfig(); break;
            }
            return recorderItemConfig;
        }
    }
}
