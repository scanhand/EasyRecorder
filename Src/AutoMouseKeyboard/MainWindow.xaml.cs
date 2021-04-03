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

        private HookingState HookingState = HookingState.Stop;
        private readonly ApplicationWatcher ApplicationWatcher;
        private readonly EventHookFactory EventHookFactory = new EventHookFactory();
        private readonly KeyboardWatcher KeyboardWatcher;
        private readonly MouseWatcher MouseWatcher;

        #endregion

        #region Input

        // Simulate each key stroke
        InputSimulator inputSimulator = new InputSimulator();

        #endregion

        #region Recorder

        AMKRecorder Recorder = new AMKRecorder();

        ListView RecorderListView = null;

        #endregion

        bool IsInitialize = false;

        public MainWindow()
        {
            InitializeComponent();

            ALog.Initialize();
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
            this.SizeChanged += MainWindow_SizeChanged;

            this.LogWindow.Show();
            this.LogWindow.Visibility = Visibility.Hidden;
            //Test
            this.LogWindow.Visibility = Visibility.Visible;

            this.KeyboardWatcher = EventHookFactory.GetKeyboardWatcher();
            this.KeyboardWatcher.OnKeyInput += KeyboardWatcher_OnKeyInput;
            this.KeyboardWatcher.Start();

            this.MouseWatcher = EventHookFactory.GetMouseWatcher();
            this.MouseWatcher.OnMouseInput += MouseWatcher_OnMouseInput;
            this.MouseWatcher.Start();

            this.ApplicationWatcher = EventHookFactory.GetApplicationWatcher();
            this.ApplicationWatcher.OnApplicationWindowChange += ApplicationWatcher_OnApplicationWindowChange;
            this.ApplicationWatcher.Start();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Test
            this.RecorderListView = this.RecorderView.RecoderListView;
            this.RecorderListView.Items.Add(new MouseClickRecorderItem());
            this.RecorderListView.Items.Add(new MouseMoveRecorderItem());
            this.RecorderListView.Items.Add(new MouseSmartClickRecorderItem());
            this.RecorderListView.Items.Add(new MouseWheelRecorderItem());
            this.RecorderListView.Items.Add(new KeyDownRecorderItem());
            this.RecorderListView.Items.Add(new KeyUpRecorderItem());
            this.RecorderListView.Items.Add(new KeyPressRecorderItem());
            this.RecorderListView.Items.Add(new WaitSmartRecorderItem());
            this.RecorderListView.Items.Add(new WaitTimeRecorderItem());

            this.Recorder.OnAddItem += (item) =>
            {
                this.InvokeIfRequired(() =>{
                    this.RecorderListView.Items.Add(item);
                    this.RecorderListView.ScrollIntoView(this.RecorderListView.Items[this.RecorderListView.Items.Count - 1]);
                });
            };

            this.Recorder.OnUpdateItem += (item) =>
            {
                this.InvokeIfRequired(() => {
                    AbsRecorderItem absItem = item as AbsRecorderItem;
                    absItem.UpdateProperties();
                });
            };

            this.Recorder.OnReplaceItem += (oldItem, newItem) =>
            {
                int index = this.RecorderListView.Items.IndexOf(oldItem);
                this.InvokeIfRequired(() => {
                    this.RecorderListView.Items[index] = newItem;
                    AbsRecorderItem absItem = newItem as AbsRecorderItem;
                    absItem.UpdateProperties();
                });
            };

            IsInitialize = true;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsInitialize)
                return;

            //this.RecoderListView.Width = this.Width;
            double width = this.Width - this.BorderThickness.Left -this.BorderThickness.Right - this.Margin.Left - this.Margin.Right - 2;
            ((GridView)this.RecorderListView.View).Columns[0].Width = width * 0.25;
            ((GridView)this.RecorderListView.View).Columns[1].Width = width * 0.5;
            ((GridView)this.RecorderListView.View).Columns[2].Width = width * 0.25;
        }

        private void ApplicationWatcher_OnApplicationWindowChange(object sender, ApplicationEventArgs e)
        {
            if (this.HookingState != HookingState.Start)
                return;

            this.Recorder.Add(e); 
        }

        private void MouseWatcher_OnMouseInput(object sender, EventHook.MouseEventArgs e)
        {
            if (this.HookingState != HookingState.Start)
                return;

            this.Recorder.Add(e); 
        }

        private void KeyboardWatcher_OnKeyInput(object sender, KeyInputEventArgs e)
        {
            if (this.HookingState != HookingState.Start)
                return;

            this.Recorder.Add(e);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.KeyboardWatcher.Stop();
            this.MouseWatcher.Stop();
            this.ApplicationWatcher.Stop();

            this.EventHookFactory.Dispose();

            this.LogWindow.IsDestoryWindow = true;
            this.LogWindow.Close();
        }

        private void StartHook()
        {
            //Test
            this.RecorderListView?.Items.Clear();

            this.Recorder.Start();
            this.HookingState = HookingState.Start;
        }

        private void StopHook()
        {
            this.HookingState = HookingState.Stop;
            this.Recorder.Stop();
        }

        private void MenuItem_StartHook_Click(object sender, RoutedEventArgs e)
        {
            StartHook();
        }

        private void MenuItem_StopHook_Click(object sender, RoutedEventArgs e)
        {
            StopHook();
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
    }
}
