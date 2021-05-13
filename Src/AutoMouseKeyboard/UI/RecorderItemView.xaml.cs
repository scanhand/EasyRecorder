using AUT.Global;
using AUT.Recorder;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AUT.UI
{
    /// <summary>
    /// Interaction logic for RecordeItemView.xaml
    /// </summary>
    public partial class RecorderItemView : UserControl
    {
        private AUTRecorder Recorder { get; set; } = null;

        public AUTRecorderItemConfigManager RecorderItemConfigManager
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

        public void Initialize(AUTRecorder recorder)
        {
            ALog.Debug("");
            this.Recorder = recorder;
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
            //Prevent Exception
            if (width < 100)
                return;

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

        private void MenuItem_PlaySelectedItems_Click(object sender, RoutedEventArgs e)
        {
            if (this.RecorderListView.SelectedItems == null || this.RecorderListView.SelectedItems.Count <= 0)
                return;
            
            this.Recorder.Player.ResetLastItem();

            List<IRecorderItem> items = new List<IRecorderItem>();
            foreach (var i in this.RecorderListView.SelectedItems)
                items.Add(i as IRecorderItem);

            this.Recorder.StartPlaying(items);
        }

        private void MenuItem_PlayFromSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            if (this.RecorderListView.SelectedItem == null)
                return;

            this.Recorder.Player.ResetLastItem();

            int startIndex = this.RecorderListView.Items.IndexOf(this.RecorderListView.SelectedItem);
            List<IRecorderItem> items = new List<IRecorderItem>();
            for(int i=startIndex; i< this.RecorderListView.Items.Count; i++)
                items.Add(this.RecorderListView.Items[i] as IRecorderItem);

            this.Recorder.StartPlaying(items);
        }

        private void MenuItem_PlayUntilSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            if (this.RecorderListView.SelectedItem == null)
                return;

            this.Recorder.Player.ResetLastItem();

            int endIndex = this.RecorderListView.Items.IndexOf(this.RecorderListView.SelectedItem);
            List<IRecorderItem> items = new List<IRecorderItem>();
            for (int i = 0; i <= endIndex; i++)
                items.Add(this.RecorderListView.Items[i] as IRecorderItem);

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
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new WaitTimeRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AUTRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        private void MenuItem_NewMouseUpDownItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new MouseUpDownRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AUTRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        private void MenuItem_NewKeyUpDownItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new KeyUpDownRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AUTRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        private void MenuItem_NewMouseWheelItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new MouseWheelRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AUTRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        private void MenuItem_NewMouseClickItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new MouseClickRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AUTRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        private void MenuItem_NewKeyPressItem_Click(object sender, RoutedEventArgs e)
        {
            IRecorderItem newItem = this.RecorderItemConfigManager.ShowNewConfigWindow(new KeyPressRecorderItem() { Time = GetNewItemTime() + TimeSpan.FromSeconds(AUTRecorder.MinimumTimeSpan) });
            if (newItem == null)
                return;

            this.Recorder.InsertItem(this.RecorderListView.SelectedItem as IRecorderItem, newItem);
        }

        #endregion

        private void RecorderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Preference.Instance.DoubleClickAction == Global.DoubleClickActionType.EditItem)
                this.menuModifyItem.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
            else if (Preference.Instance.DoubleClickAction == Global.DoubleClickActionType.Memo)
                this.menuMemo.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
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
