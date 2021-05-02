using AMK.Recorder;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AMK.UI
{
    /// <summary>
    /// Interaction logic for RecordeItemView.xaml
    /// </summary>
    public partial class RecorderItemView : UserControl
    {
        public AMKRecorder Recorder { get; set; } = null;

        public AMKRecorderItemConfigManager RecorderItemConfigManager
        {
            get
            {
                return this.Recorder.RecorderItemConfigManager;
            }
        }

        public RecorderItemView()
        {
            InitializeComponent();

            this.MouseDoubleClick += RecorderListView_MouseDoubleClick;
        }

        public void DeleteSelectedItems()
        {
            if (this.RecorderListView.SelectedItems == null || this.RecorderListView.SelectedItems.Count <= 0)
                return;

            this.Recorder.Player.ResetLastItem();

            List<IRecorderItem> deleteItems = new List<IRecorderItem>();
            foreach (var i in this.RecorderListView.SelectedItems)
            {
                IRecorderItem item = i as IRecorderItem;
                deleteItems.Add(item);
            }
            this.Recorder.DeleteItem(deleteItems);
        }

        private void MenuItem_PlayItems_Click(object sender, RoutedEventArgs e)
        {
            if (this.RecorderListView.SelectedItems == null || this.RecorderListView.SelectedItems.Count <= 0)
                return;

            this.Recorder.Player.ResetLastItem();
            foreach (var i in this.RecorderListView.SelectedItems)
            {
                IRecorderItem item = i as IRecorderItem;
                item.Play(this.Recorder.Player);

                Thread.Sleep(1);
            }
        }

        private void MenuItem_DeleteItems_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedItems();
        }

        private void MenuItem_ModifyItem_Click(object sender, RoutedEventArgs e)
        {
            this.RecorderItemConfigManager.ShowModifyConfigWindow(this.RecorderListView.SelectedItem as IRecorderItem);
        }

        private void MenuItem_NewWaitingItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new WaitTimeRecorderItem() { Time = GetNewItemTime()+TimeSpan.FromSeconds(AMKRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        private void MenuItem_NewMouseUpDownItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new MouseUpDownRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AMKRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        private void MenuItem_NewKeyUpDownItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new KeyUpDownRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AMKRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        private void MenuItem_NewMouseWheelItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new MouseWheelRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AMKRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        private void RecorderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.RecorderItemConfigManager.ShowModifyConfigWindow(this.RecorderListView.SelectedItem as IRecorderItem);
        }

        private DateTime GetNewItemTime()
        {
            DateTime dateTime = DateTime.Now;
            if (this.RecorderListView.SelectedItem == null)
            {
                if(this.RecorderListView.Items.Count <= 0)
                    return dateTime;

                return (this.RecorderListView.Items[0] as IRecorderItem).Time;
            }

            return (this.RecorderListView.SelectedItem as IRecorderItem).Time;
        }
    }
}
