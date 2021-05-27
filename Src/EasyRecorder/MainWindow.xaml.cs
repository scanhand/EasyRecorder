using AvalonDock.Themes;
using ESR.Files;
using ESR.Global;
using ESR.Recorder;
using ESR.UI;
using EventHook;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Threading;
using WindowsInput.Native;

namespace ESR
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

        public ESRCommander Commander = new ESRCommander();

        #endregion

        #region Recorder

        public ESRRecorder Recorder = new ESRRecorder();

        private System.Windows.Controls.ListView RecorderListView
        {
            get
            {
                return this.RecorderView.RecorderListView;
            }
        }

        ToastWindow ToastWindow = new ToastWindow();

        #endregion

        #region Theme

        public Theme DockTheme { get; set; } = new MetroTheme();

        #endregion

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
            if(GM.Instance.IsTest)
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
            this.RecorderView.Initialize(this.Recorder);

            //MainToolbar
            this.MainToolbar.Initialize(this.Recorder);

            //Status
            this.Recorder.OnChangedState += (s) =>
            {
                this.MainStatusBar.SetCurrentState(s);
            };
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Child controls are loaded!
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                //Initialize ESRRecorder
                this.Recorder.Initialize();
            }));

            //Initialize Preference
            Preference.Instance.MainWindow = this;
            Preference.Instance.LogWindow = this.LogWindow;
            Preference.Instance.MenuAlwaysTopItem = this.MenuAlwaysTopMostItem;
            Preference.Instance.MenuInfiniteRepeatItem = this.MenuInfiniteRepeatItem;
            Preference.Instance.Load();

            this.Recorder.OnAddItem += (item) =>
            {
                IRecorderItem recorderItem = item;
                this.TaskQueue.QueueTask(() =>
                {
                    this.InvokeIfRequired(() =>
                    {
                        this.RecorderListView.Items.Add(item);
                        this.RecorderListView.ScrollIntoView(this.RecorderListView.Items[this.RecorderListView.Items.Count - 1]);
                    });
                });
            };

            this.Recorder.OnInsertItem += (prevItem, newItem) =>
            {
                this.InvokeIfRequired(() =>
                {
                    int startIndex = -1;
                    if (prevItem != null)
                        startIndex = this.RecorderListView.Items.IndexOf(prevItem);

                    this.RecorderListView.Items.Insert(startIndex + 1, newItem);
                    this.RecorderListView.ScrollIntoView(newItem);
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
                    if (Preference.Instance.IsShowToastMessage)
                    {
                        this.ToastWindow.SetState(ESRState.Recording);
                        this.ToastWindow.Show();
                    }
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
                    if (Preference.Instance.IsShowToastMessage)
                    {
                        this.ToastWindow.SetState(ESRState.Playing);
                        this.ToastWindow.Show();
                    }

                    this.RecorderListView.UnselectAll();
                });
            };

            this.Recorder.OnStopPlaying += (isLastStep) =>
            {
                this.InvokeIfRequired(() =>
                {
                    if (isLastStep)
                    {
                        this.Recorder.State = ESRState.PlayDone;
                    }
                    else
                    {
                        this.Recorder.State = ESRState.Stop;
                    }
                    this.ToastWindow.Hide();
                });
            };

            if (GM.Instance.IsTest)
            {
                this.Recorder.AddItem(new MouseWheelRecorderItem());
                this.Recorder.AddItem(new MouseMoveRecorderItem());
                this.Recorder.AddItem(new MouseUpDownRecorderItem());
                this.Recorder.AddItem(new MouseSmartClickRecorderItem());
                this.Recorder.AddItem(new KeyUpDownRecorderItem());
                this.Recorder.AddItem(new KeyPressRecorderItem());
                this.Recorder.AddItem(new WaitSmartRecorderItem());
                this.Recorder.AddItem(new WaitTimeRecorderItem());
            }
        }

        private void LayoutRoot_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ALog.Debug("");
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ALog.Debug("");
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            ALog.Debug("");
        }

        private void ApplicationWatcher_OnApplicationWindowChange(object sender, ApplicationEventArgs e)
        {
            this.Recorder.Add(e);
        }

        private void MouseWatcher_OnMouseInput(object sender, EventHook.MouseEventArgs e)
        {
            UpdateMousePosition(e);

            this.Recorder.Add(e);
        }

        private void KeyboardWatcher_OnKeyInput(object sender, KeyInputEventArgs e)
        {
            ALog.Debug("KeyboardWatcher::EventType={0}, VKCode={1}", e.KeyData.EventType.ToString(), ((VirtualKeyCode)e.KeyData.VkCode).ToString());
            if (this.Commander.ProcessKey(e))
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

        public void StopRecording()
        {
            ALog.Debug("");
            this.Recorder.StopRecording();
        }

        public void StopPlaying()
        {
            ALog.Debug("");
            this.Recorder.StopPlaying();
        }

        #region Menu

        private void MenuItem_StartRecording_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.Recorder.StartRecordingWithConfirm();
        }

        private void MenuItem_StopRecording_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.Recorder.StopRecording();
        }

        private void MenuItem_ShowLog_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.LogWindow.Visibility = Visibility.Visible;
            this.LogWindow.WindowState = WindowState.Normal;
        }

        private void MenuItem_StartPlaying_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.Recorder.StartPlaying(true);
        }

        private void MenuItem_StopPlaying_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.Recorder.StopPlaying();
        }

        private void MenuItem_ResetItems_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");

            if (MessageBox.Show("Do you want to reset all of recorder items?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            this.Recorder.ResetItems();
        }

        private void MenuItem_FileLoad_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            ESRFile file = ESRFile.LoadFileDialog();
            if (file == null)
                return;

            this.Recorder.Reset();
            foreach (IRecorderItem item in file.FileBody.Items)
            {
                this.Recorder.AddItem(item);
            }
        }

        private void MenuItem_FileSave_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            ESRFile.SaveFileDialog(this.Recorder.Items);
        }

        private void MenuItem_AlwaysTopMost_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.Topmost = this.MenuAlwaysTopMostItem.IsChecked;
        }

        private void MenuInfiniteRepeatItem_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            Preference.Instance.IsInfiniteRepeat = this.MenuInfiniteRepeatItem.IsChecked;
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to exit the program?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            System.Windows.Application.Current.Shutdown();
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            aboutWindow.ShowDialog();
        }

        private void MenuItem_ResetToStart_Click(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.Recorder.ResetToStart();
        }

        #endregion

        private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ALog.Debug("");
            this.WindowState = WindowState.Normal;
        }

        private void UpdateMousePosition(EventHook.MouseEventArgs e)
        {
            this.InvokeIfRequired(() =>
            {
                this.MainStatusBar.lblMousePosition.Text = string.Format("X: {0,4:D}, Y: {1,4:D}", e.Point.x, e.Point.y);
            });
        }

    }
}
