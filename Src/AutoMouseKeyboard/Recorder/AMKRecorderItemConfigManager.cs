using AMK.Global;
using AMK.UI;
using System;
using System.Windows;

namespace AMK.Recorder
{
    public class AMKRecorderItemConfigManager
    {
        private AMKRecorder AMKRecorder { get; set; } = null;

        public Action<IRecorderItem, IRecorderItem> OnReplaceItem = null;

        public AMKRecorderItemConfigManager(AMKRecorder recorder)
        {
            this.AMKRecorder = recorder;
        }

        public bool ShowModifyConfigWindow(IRecorderItem prevItem)
        {
            if (prevItem == null)
                return false;

            IRecorderItemConfig config = CreateRecorderItemConfig(prevItem);
            config.RecorderItem = prevItem.Copy();
            Window window = config as Window;
            window.Owner = GM.Instance.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (window.ShowDialog() == false)
                return false;

            IRecorderItem modifiedItem = config.RecorderItem.Copy();
            if (OnReplaceItem != null)
                OnReplaceItem(prevItem, modifiedItem);

            return true;
        }

        public IRecorderItem ShowNewConfigWindow(IRecorderItem item)
        {
            IRecorderItemConfig config = CreateRecorderItemConfig(item);
            config.RecorderItem = item.Copy();
            Window window = config as Window;
            window.Owner = GM.Instance.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (window.ShowDialog() == false)
                return null;

            return config.RecorderItem.Copy();
        }

        private IRecorderItemConfig CreateRecorderItemConfig(IRecorderItem item)
        {
            IRecorderItemConfig recorderItemConfig = null;
            switch (item.Recorder)
            {
                case RecorderType.WaitSmart: recorderItemConfig = new WaitingTimeRecorderItemConfig(); break;
                case RecorderType.WaitTime: recorderItemConfig = new WaitingTimeRecorderItemConfig(); break;
                case RecorderType.MouseClick: recorderItemConfig = new MouseClickRecorderItemConfig(); break;
                case RecorderType.MouseUpDown: recorderItemConfig = new MouseUpDownRecorderItemConfig(); break;
                case RecorderType.MouseSmartClick: recorderItemConfig = new MouseUpDownRecorderItemConfig(); break;
                case RecorderType.MouseMove: recorderItemConfig = new MouseUpDownRecorderItemConfig(); break;
                case RecorderType.MouseWheel: recorderItemConfig = new MouseWheelRecorderItemConfig(); break;
                case RecorderType.KeyUpDown: recorderItemConfig = new KeyUpDownRecorderItemConfig(); break;
                case RecorderType.KeyPress: recorderItemConfig = new KeyUpDownRecorderItemConfig(); break;
            }
            return recorderItemConfig;
        }
    }
}
