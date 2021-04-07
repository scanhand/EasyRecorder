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

        System.Windows.Controls.ListView RecorderListView = null;

        #endregion

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

            this.RecorderListView = this.RecorderView.RecoderListView;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
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

            //Test
            this.Recorder.AddItem(new MouseClickRecorderItem());
            this.Recorder.AddItem(new MouseClickRecorderItem());
            this.Recorder.AddItem(new MouseMoveRecorderItem());
            this.Recorder.AddItem(new MouseSmartClickRecorderItem());
            this.Recorder.AddItem(new MouseWheelRecorderItem());
            this.Recorder.AddItem(new KeyDownRecorderItem());
            this.Recorder.AddItem(new KeyUpRecorderItem());
            this.Recorder.AddItem(new KeyPressRecorderItem());
            this.Recorder.AddItem(new WaitSmartRecorderItem());
            this.Recorder.AddItem(new WaitTimeRecorderItem());
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.RecorderListView == null)
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

        private void StartRecording()
        {
            //Test
            this.RecorderListView?.Items.Clear();

            this.Recorder.StartRecording();
            this.HookingState = HookingState.Start;
        }

        private void StopRecording()
        {
            this.HookingState = HookingState.Stop;
            this.Recorder.StopRecording();
        }

        private void MenuItem_StartRecording_Click(object sender, RoutedEventArgs e)
        {
            StartRecording();
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
            this.Recorder.StartPlaying();
        }

        private void MenuItem_StopPlaying_Click(object sender, RoutedEventArgs e)
        {
            this.Recorder.StopPlaying();
        }

        private void MenuItem_FileLoad_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
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

                this.Recorder.Items.Clear();
                this.RecorderListView?.Items.Clear();

                foreach (IRecorderItem item in file.FileBody.Items)
                {
                    this.Recorder.AddItem(item);
                }
            }
        }

        private void MenuItem_FileSave_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
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
        }
    }
}
