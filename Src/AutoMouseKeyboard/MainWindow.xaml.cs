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

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            ALog.Initialize();
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
            this.SizeChanged += MainWindow_SizeChanged;

            LogWindow.Show();
            LogWindow.Visibility = Visibility.Hidden;
            //Test
            LogWindow.Visibility = Visibility.Visible;

            KeyboardWatcher = EventHookFactory.GetKeyboardWatcher();
            KeyboardWatcher.OnKeyInput += KeyboardWatcher_OnKeyInput;
            KeyboardWatcher.Start();

            MouseWatcher = EventHookFactory.GetMouseWatcher();
            MouseWatcher.OnMouseInput += MouseWatcher_OnMouseInput;
            MouseWatcher.Start();

            ApplicationWatcher = EventHookFactory.GetApplicationWatcher();
            ApplicationWatcher.OnApplicationWindowChange += ApplicationWatcher_OnApplicationWindowChange;
            ApplicationWatcher.Start();

            this.Loaded += MainWindow_Loaded1;
        }

        private void MainWindow_Loaded1(object sender, RoutedEventArgs e)
        {
            RecoderListView.Items.Add(new MouseClickRecorderItem()
            {

            });

            RecoderListView.Items.Add(new MouseMoveRecorderItem()
            {

            });

            RecoderListView.Items.Add(new MouseSmartClickRecorderItem()
            {

            });

            RecoderListView.Items.Add(new MouseWheelRecorderItem()
            {

            });

            RecoderListView.Items.Add(new KeyDownRecorderItem()
            {

            });

            RecoderListView.Items.Add(new KeyUpRecorderItem()
            {

            });

            RecoderListView.Items.Add(new KeyPressRecorderItem()
            {

            });

            RecoderListView.Items.Add(new WaitSmartRecorderItem()
            {

            });

            RecoderListView.Items.Add(new WaitTimeRecorderItem()
            {

            });
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //gridRecordItem.Width = this.Width;
        }

        private void ApplicationWatcher_OnApplicationWindowChange(object sender, ApplicationEventArgs e)
        {
            if (this.HookingState != HookingState.Start)
                return;

            this.Recorder.Add(e);
            //ALog.Debug("Application window of '{0}' with the title '{1}' was {2}", e.ApplicationData.AppName, e.ApplicationData.AppTitle, e.Event);
        }

        private void MouseWatcher_OnMouseInput(object sender, EventHook.MouseEventArgs e)
        {
            if (this.HookingState != HookingState.Start)
                return;

            this.Recorder.Add(e);
            //ALog.Debug("Mouse event {0} at point {1},{2}", e.Message.ToString(), e.Point.x, e.Point.y);
        }

        private void KeyboardWatcher_OnKeyInput(object sender, KeyInputEventArgs e)
        {
            if (this.HookingState != HookingState.Start)
                return;

            this.Recorder.Add(e);
            //ALog.Debug("Key {0} event of key {1}", e.KeyData.EventType, e.KeyData.Keyname);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            KeyboardWatcher.Stop();
            MouseWatcher.Stop();
            ApplicationWatcher.Stop();

            EventHookFactory.Dispose();

            LogWindow.IsDestoryWindow = true;
            LogWindow.Close();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ALog.Debug("MainWindow_Loaded");
        }

        private void StartHook()
        {
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
