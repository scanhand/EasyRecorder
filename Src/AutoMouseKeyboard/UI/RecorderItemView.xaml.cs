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
            this.SizeChanged += RecorderItemView_SizeChanged;
        }

        private void RecorderItemView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeRecorderListViewColumn();
        }

        private void RecorderItemView_StateChanged(object sender, EventArgs e)
        {

        }

        private void ResizeRecorderListViewColumn()
        {
            if (this.RecorderListView == null)
                return;

            const int statusColumnWidth = 30;
            const int columnCount = 4;
            double totalWidth = 0;
            for (int i = 1; i < columnCount; i++)
                totalWidth += ((GridView)this.RecorderListView.View).Columns[i].Width;

            double[] totalWidthFactor = new double[columnCount];
            for (int i = 1; i < columnCount; i++)
                totalWidthFactor[i] = ((GridView)this.RecorderListView.View).Columns[i].Width / totalWidth;

            this.RecorderListView.Width = this.ActualWidth;
            double width = this.ActualWidth - statusColumnWidth - this.BorderThickness.Left - this.BorderThickness.Right - this.Margin.Left - this.Margin.Right - 2;
            ((GridView)this.RecorderListView.View).Columns[0].Width = statusColumnWidth;
            for (int i = 1; i < columnCount; i++)
                ((GridView)this.RecorderListView.View).Columns[i].Width = width * totalWidthFactor[i];
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

        #region Menu

        private void MenuItem_PlayItems_Click(object sender, RoutedEventArgs e)
        {
            if (this.RecorderListView.SelectedItems == null || this.RecorderListView.SelectedItems.Count <= 0)
                return;

            this.Recorder.Player.ResetLastItem();

            List<IRecorderItem> items = new List<IRecorderItem>();
            foreach (var i in this.RecorderListView.SelectedItems)
                items.Add(i as IRecorderItem);

            this.Recorder.StartPlaying(items);
        }

        private void MenuItem_DeleteItems_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedItems();
        }

        private void MenuItem_ModifyItem_Click(object sender, RoutedEventArgs e)
        {
            this.RecorderItemConfigManager.ShowModifyConfigWindow(this.RecorderListView.SelectedItem as IRecorderItem);
        }

        private void MenuItem_ModifyMemo_Click(object sender, RoutedEventArgs e)
        {
            this.RecorderItemConfigManager.ShowModifyMemoWindow(this.RecorderListView.SelectedItem as IRecorderItem);
        }

        private void MenuItem_NewWaitingItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new WaitTimeRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AMKRecorder.MinimumTimeSpan) });
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

        private void MenuItem_NewMouseClickItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new MouseClickRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AMKRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        private void MenuItem_NewKeyPressItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new KeyPressRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AMKRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        #endregion

        private void RecorderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(Preference.Instance.DoubleClickAction == Global.DoubleClickActionType.EditItem)
                this.RecorderItemConfigManager.ShowModifyConfigWindow(this.RecorderListView.SelectedItem as IRecorderItem);
            else if (Preference.Instance.DoubleClickAction == Global.DoubleClickActionType.Memo)
                this.RecorderItemConfigManager.ShowModifyMemoWindow(this.RecorderListView.SelectedItem as IRecorderItem);
        }

        private DateTime GetNewItemTime()
        {
            DateTime dateTime = DateTime.Now;
            if (this.RecorderListView.SelectedItem == null)
            {
                if (this.RecorderListView.Items.Count <= 0)
                    return dateTime;

                return (this.RecorderListView.Items[0] as IRecorderItem).Time;
            }

            return (this.RecorderListView.SelectedItem as IRecorderItem).Time;
        }
    }
}
