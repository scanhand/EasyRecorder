using AUT.Global;
using AUT.UI;
using System;
using System.Windows;

namespace AUT.Recorder
{
    public class AUTRecorderItemConfigManager
    {
        private AUTRecorder AUTRecorder { get; set; } = null;

        public Action<IRecorderItem, IRecorderItem> OnReplaceItem = null;

        public Action<IRecorderItem> OnUpdateItem = null;

        public AUTRecorderItemConfigManager(AUTRecorder recorder)
        {
            this.AUTRecorder = recorder;
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

        public void ShowModifyMemoWindow(IRecorderItem prevItem)
        {
            if (prevItem == null)
                return;

            IRecorderItemConfig config = new RecorderItemMemoConfig();
            config.RecorderItem = prevItem.Copy();
            Window window = config as Window;
            window.Owner = GM.Instance.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (window.ShowDialog() == false)
                return;

            IRecorderItem modifiedItem = config.RecorderItem.Copy();
            if (OnReplaceItem != null)
                OnReplaceItem(prevItem, modifiedItem);
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
                case RecorderType.MouseMove: recorderItemConfig = new MouseMoveRecorderItemConfig(); break;
                case RecorderType.MouseWheel: recorderItemConfig = new MouseWheelRecorderItemConfig(); break;
                case RecorderType.KeyUpDown: recorderItemConfig = new KeyUpDownRecorderItemConfig(); break;
                case RecorderType.KeyPress: recorderItemConfig = new KeyPressRecorderItemConfig(); break;
            }
            return recorderItemConfig;
        }
    }
}
