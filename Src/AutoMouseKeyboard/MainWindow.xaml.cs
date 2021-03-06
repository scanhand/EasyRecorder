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
        private readonly ApplicationWatcher applicationWatcher;
        private readonly EventHookFactory eventHookFactory = new EventHookFactory();
        private readonly KeyboardWatcher keyboardWatcher;
        private readonly MouseWatcher mouseWatcher;

        #endregion

        #region Input

        // Simulate each key stroke
        InputSimulator inputSimulator = new InputSimulator();

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            ALog.Initialize();
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;

            LogWindow.Show();
            LogWindow.Visibility = Visibility.Hidden;
            //Test
            LogWindow.Visibility = Visibility.Visible;

            keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
            keyboardWatcher.OnKeyInput += KeyboardWatcher_OnKeyInput;
            keyboardWatcher.Start();

            mouseWatcher = eventHookFactory.GetMouseWatcher();
            mouseWatcher.OnMouseInput += MouseWatcher_OnMouseInput;
            mouseWatcher.Start();

            applicationWatcher = eventHookFactory.GetApplicationWatcher();
            applicationWatcher.OnApplicationWindowChange += ApplicationWatcher_OnApplicationWindowChange;
            applicationWatcher.Start();
        }

        private void ApplicationWatcher_OnApplicationWindowChange(object sender, ApplicationEventArgs e)
        {
            if (this.HookingState != HookingState.Start)
                return;

            ALog.Debug("Application window of '{0}' with the title '{1}' was {2}", e.ApplicationData.AppName, e.ApplicationData.AppTitle, e.Event);
        }

        private void MouseWatcher_OnMouseInput(object sender, EventHook.MouseEventArgs e)
        {
            if (this.HookingState != HookingState.Start)
                return;

            ALog.Debug("Mouse event {0} at point {1},{2}", e.Message.ToString(), e.Point.x, e.Point.y);
        }

        private void KeyboardWatcher_OnKeyInput(object sender, KeyInputEventArgs e)
        {
            if (this.HookingState != HookingState.Start)
                return;

            ALog.Debug("Key {0} event of key {1}", e.KeyData.EventType, e.KeyData.Keyname);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            keyboardWatcher.Stop();
            mouseWatcher.Stop();
            applicationWatcher.Stop();

            eventHookFactory.Dispose();

            LogWindow.IsDestoryWindow = true;
            LogWindow.Close();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ALog.Debug("MainWindow_Loaded");
        }

        private void StartHook()
        {
            this.HookingState = HookingState.Start;
        }

        private void StopHook()
        {
            this.HookingState = HookingState.Stop;
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
