using AMK.Global;
using AMK.Recorder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMK.UI
{
    public static class AMKRecorderItemConfigManager
    {
        public static Action<IRecorderItem, IRecorderItem> OnUpdateItem = null;

        public static void ShowConfigWindow(IRecorderItem item)
        {
            IRecorderItemConfig config = CreateRecorderItemConfig(item);
            config.RecorderItem = item;
            Window window = config as Window;
            window.Owner = GM.Instance.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (window.ShowDialog() == false)
                return;

            IRecorderItem updatedItem = config.RecorderItem.Copy();
            OnUpdateItem(item, updatedItem);
        }

        private static IRecorderItemConfig CreateRecorderItemConfig(IRecorderItem item)
        {
            IRecorderItemConfig recorderItemConfig = new WaitingTimeRecorderItemConfig();
            return recorderItemConfig;
        }
    }
}
