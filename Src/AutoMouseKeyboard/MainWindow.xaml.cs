using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsInput;
using WindowsInput.Native;
using MahApps.Metro.Controls;
using AMK.Recorder;
using System.Windows.Forms;
using AMK.Files;
using AMK.UI;
using AMK.Global;
using AvalonDock.Themes;

namespace AMK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : MetroWindow
    {
        #region Log

        private LogWindow LogWindow = new LogWindow();

        #endregion

        #region Hook

        private readonly ApplicationWatcher ApplicationWatcher;
        private readonly EventHookFactory EventHookFactory = new EventHookFactory();
        private readonly KeyboardWatcher KeyboardWatcher;
        private readonly MouseWatcher MouseWatcher;
        private BackgroundQueue TaskQueue = new BackgroundQueue();

        #endregion

        #region Commander

        public AMKCommander Commander = new AMKCommander();

        #endregion

        #region Recorder

        public AMKRecorder Recorder = new AMKRecorder();

        private System.Windows.Controls.ListView RecorderListView
        {
            get
            {
                return this.RecorderView.RecoderListView;
            }
        }

        ToastWindow ToastWindow = new ToastWindow();

        #endregion

        public Theme DockTheme { get; set; } = new MetroTheme();

        public MainWindow()
        {
            InitializeComponent();

            //Global Manager
            GM.Instance.MainWindow = this;

            ALog.Initialize();
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
            this.SizeChanged += MainWindow_SizeChanged;
            this.StateChanged += MainWindow_StateChanged;

            //Log
            this.LogWindow.Show();
            this.LogWindow.Visibility = Visibility.Hidden;
            //Test
            this.LogWindow.Visibility = Visibility.Visible;

            //Hooking
            this.KeyboardWatcher = EventHookFactory.GetKeyboardWatcher();
            this.KeyboardWatcher.OnKeyInput += KeyboardWatcher_OnKeyInput;
            this.KeyboardWatcher.Start();

            this.MouseWatcher = EventHookFactory.GetMouseWatcher();
            this.MouseWatcher.OnMouseInput += MouseWatcher_OnMouseInput;
            this.MouseWatcher.Start();

            this.ApplicationWatcher = EventHookFactory.GetApplicationWatcher();
            this.ApplicationWatcher.OnApplicationWindowChange += ApplicationWatcher_OnApplicationWindowChange;
            this.ApplicationWatcher.Start();

            //RecorderView
            this.RecorderView.Recorder = this.Recorder;
            this.RecorderListView.MouseDoubleClick += RecorderListView_MouseDoubleClick;
           
            //Status
            this.Recorder.OnChangedState += (s) =>
            {
                this.MainStatusBar.SetCurrentState(s);
            };
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Initialize Preference
            Preference.Instance.MainWindow = this;
            Preference.Instance.LogWindow = this.LogWindow;
            Preference.Instance.MenuAlwaysTopItem = this.MenuAlwaysTopMostItem;
            Preference.Instance.Load();

            this.Recorder.OnAddItem += (item) =>
            {
                this.TaskQueue.QueueTask(() =>
                {
                    this.InvokeIfRequired(() =>
                    {
                        this.RecorderListView.Items.Add(item);
                        this.RecorderListView.ScrollIntoView(this.RecorderListView.Items[this.RecorderListView.Items.Count - 1]);
                    });
                });
            };

            this.Recorder.OnUpdateItem += (item) =>
            {
                this.TaskQueue.QueueTask(() =>
                {
                    this.InvokeIfRequired(() =>
                    {
                        AbsRecorderItem absItem = item as AbsRecorderItem;
                        absItem.UpdateProperties();

                        if (absItem.State == RecorderItemState.Activated)
                            this.RecorderListView.ScrollIntoView(item);
                    });
                });
            };

            this.Recorder.OnReplaceItem += (oldItem, newItem) =>
            {
                this.TaskQueue.QueueTask(() =>
                {
                    int index = this.RecorderListView.Items.IndexOf(oldItem);
                    this.InvokeIfRequired(() =>
                    {
                        this.RecorderListView.Items[index] = newItem;
                        AbsRecorderItem absItem = newItem as AbsRecorderItem;
                        absItem.UpdateProperties();
                    });
                });
            };

            this.Recorder.OnResetItem += () =>
            {
                this.TaskQueue.QueueTask(() =>
                {
                    this.InvokeIfRequired(() =>
                    {
                        this.RecorderListView?.Items.Clear();
                    });
                });
            };

            this.Recorder.OnDeleteItem += (item) =>
            {
                this.TaskQueue.QueueTask(() =>
                {
                    this.InvokeIfRequired(() =>
                    {
                        this.RecorderListView?.Items.Remove(item);
                    });
                });
            };

            this.Recorder.OnStartRecording += () =>
            {
                this.InvokeIfRequired(() =>
                {
                    this.ToastWindow.SetState(AMKState.Recording);
                    this.ToastWindow.Show();
                });
            };

            this.Recorder.OnStopRecording += () =>
            {
                this.InvokeIfRequired(() =>
                {
                    this.ToastWindow.Hide();
                });
            };

            this.Recorder.OnStartPlaying += () =>
            {
                this.InvokeIfRequired(() =>
                {
                    this.RecorderListView.UnselectAll();
                });
            };

            this.Recorder.OnStopPlaying += (isLastStep) =>
            {
                this.InvokeIfRequired(() =>
                {
                    if (isLastStep)
                    {
                        this.Recorder.State = AMKState.PlayDone;
                    }
                    else
                    {
                        this.Recorder.State = AMKState.Stop;
                    }
                });
            };

            AMKRecorderItemConfigManager.OnUpdateItem += (oldItem, newItem) =>
            {
                this.Recorder.ReplaceItem(oldItem, newItem);
            };

            //Test
            this.Recorder.AddItem(new MouseWheelRecorderItem());
            this.Recorder.AddItem(new MouseClickRecorderItem());
            this.Recorder.AddItem(new MouseMoveRecorderItem());
            this.Recorder.AddItem(new MouseSmartClickRecorderItem());
            this.Recorder.AddItem(new KeyDownRecorderItem());
            this.Recorder.AddItem(new KeyUpRecorderItem());
            this.Recorder.AddItem(new KeyPressRecorderItem());
            this.Recorder.AddItem(new WaitSmartRecorderItem());
            this.Recorder.AddItem(new WaitTimeRecorderItem());
        }

        private void LayoutRoot_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeRecorderListViewColumn();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            ResizeRecorderListViewColumn();
        }

        private void ResizeRecorderListViewColumn()
        {
            if (this.RecorderListView == null)
                return;

            const int columnCount = 3;
            double totalWidth = 0;
            for (int i = 0; i < columnCount; i++)
                totalWidth += ((GridView)this.RecorderListView.View).Columns[i].Width;

            double[] totalWidthFactor = new double[columnCount];
            for (int i = 0; i < columnCount; i++)
                totalWidthFactor[i] = ((GridView)this.RecorderListView.View).Columns[i].Width / totalWidth;

            this.RecorderListView.Width = this.ActualWidth;
            double width = this.ActualWidth - this.BorderThickness.Left - this.BorderThickness.Right - this.Margin.Left - this.Margin.Right - 2;
            for (int i = 0; i < columnCount; i++)
                ((GridView)this.RecorderListView.View).Columns[i].Width = width * totalWidthFactor[i];
        }

        private void ApplicationWatcher_OnApplicationWindowChange(object sender, ApplicationEventArgs e)
        {
            if (this.Recorder.State != AMKState.Recording)
                return;

            this.Recorder.Add(e);
        }

        private void MouseWatcher_OnMouseInput(object sender, EventHook.MouseEventArgs e)
        {
            UpdateMousePosition(e);

            if (this.Recorder.State != AMKState.Recording)
                return;

            this.Recorder.Add(e);
        }

        private void KeyboardWatcher_OnKeyInput(object sender, KeyInputEventArgs e)
        {
            if (this.Commander.ProcessKey(e))
                return;

            if (this.Recorder.State != AMKState.Recording)
                return;

            this.Recorder.Add(e);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ToastWindow.Close();
            this.Recorder.StopAll();

            this.KeyboardWatcher.Stop();
            this.MouseWatcher.Stop();
            this.ApplicationWatcher.Stop();

            this.EventHookFactory.Dispose();

            this.LogWindow.IsDestoryWindow = true;
            this.LogWindow.Close();
        }

        public void StartRecording(bool isReset)
        {
            ALog.Debug("StartRecording");
            if(isReset)
                this.Recorder.Reset();
            this.Recorder.StartRecording();
        }

        public void StopRecording()
        {
            ALog.Debug("StopRecording");
            this.Recorder.StopRecording();
        }

        public void StartPlaying(bool isReset)
        {
            ALog.Debug("StartPlaying::IsReset={0}", isReset);
            if (isReset)
                this.Recorder.ResetToStart();
            this.Recorder.StartPlaying();
        }

        public void StopPlaying()
        {
            ALog.Debug("StopPlaying");
            this.Recorder.StopPlaying();
        }

        public void Stop()
        {
            this.Recorder.StopAll();
        }

        public void ResetItems()
        {
            this.Recorder.StopPlaying();
            this.Recorder.StopRecording();
            this.Recorder.Reset();
        }

        private void MenuItem_StartRecording_Click(object sender, RoutedEventArgs e)
        {
            StartRecording(true);
        }

        private void MenuItem_StopRecording_Click(object sender, RoutedEventArgs e)
        {
            StopRecording();
        }

        private void MenuItem_ShowLog_Click(object sender, RoutedEventArgs e)
        {
            this.LogWindow.Visibility = Visibility.Visible;
            this.LogWindow.WindowState = WindowState.Normal;
        }

        private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void MenuItem_StartPlaying_Click(object sender, RoutedEventArgs e)
        {
            StartPlaying(true);
        }

        private void MenuItem_StopPlaying_Click(object sender, RoutedEventArgs e)
        {
            StopPlaying();
        }

        private void MenuItem_ResetItems_Click(object sender, RoutedEventArgs e)
        {
            ResetItems();
        }

        private void MenuItem_FileLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "AMK files (*.amk)|*.amk|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            //Get the path of specified file
            string filePath = openFileDialog.FileName;

            AMKFile file = new AMKFile();
            file.FileName = filePath;
            if (!file.LoadFile())
            {
                System.Windows.MessageBox.Show("File Load Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.Recorder.Reset();

            foreach (IRecorderItem item in file.FileBody.Items)
            {
                this.Recorder.AddItem(item);
            }
        }

        private void MenuItem_FileSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "AMK files (*.amk)|*.amk|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            //Get the path of specified file
            string filePath = saveFileDialog.FileName;

            AMKFile file = new AMKFile();
            file.FileName = filePath;
            file.FileBody.Items = this.Recorder.Items.Copy<List<IRecorderItem>>();
            if (!file.SaveFile())
            {
                System.Windows.MessageBox.Show("File Save Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MenuItem_AlwaysTopMost_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = this.MenuAlwaysTopMostItem.IsChecked;
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to exit the program?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            System.Windows.Application.Current.Shutdown();
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            aboutWindow.ShowDialog();
        }

        private void MenuItem_ResetToStart_Click(object sender, RoutedEventArgs e)
        {
            if (!AUtil.IsStopPause(this.Recorder.State))
                return;

            this.Recorder.ResetToStart();
        }

        private void UpdateMousePosition(EventHook.MouseEventArgs e)
        {
            this.InvokeIfRequired(() =>
            {
                this.MainStatusBar.lblMousePosition.Text = string.Format("X: {0,4:D}, Y: {1,4:D}", e.Point.x, e.Point.y);
            });
        }

        private void RecorderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IRecorderItem item = this.RecorderListView.SelectedItem as IRecorderItem;
            if (item == null)
                return;

            AMKRecorderItemConfigManager.ShowConfigWindow(item);
        }

    }
}
