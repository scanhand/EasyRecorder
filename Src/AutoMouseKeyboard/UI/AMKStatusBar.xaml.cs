using AMK.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMK.UI
{
    /// <summary>
    /// Interaction logic for AMKStatusBar.xaml
    /// </summary>
    public partial class AMKStatusBar : UserControl
    {
        public AMKStatusBarItem StatusBarItem { get; set; } = new AMKStatusBarItem();

        private DoubleAnimation FadeInOutAnimation = null;

        public AMKStatusBar()
        {
            InitializeComponent(); 
            
            this.DataContext = this.StatusBarItem;

            this.FadeInOutAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
            };
        }

        public void SetCurrentState(AMKState state)
        {
            this.InvokeIfRequired(() =>
            {
                if (state == AMKState.Playing)
                {
                    this.StatusBarItem.StatusText = "Playing...";
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-play-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, this.FadeInOutAnimation);
                }
                else if (state == AMKState.PlayingPause)
                {
                    this.StatusBarItem.StatusText = "Pause playing";
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-pause-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, new DoubleAnimation());
                }
                else if (state == AMKState.PlayDone)
                {
                    this.StatusBarItem.StatusText = "Done";
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-simplestop-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, new DoubleAnimation());
                }
                else if(state == AMKState.Recording)
                {
                    this.StatusBarItem.StatusText = "Recording...";
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-video-record-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, this.FadeInOutAnimation);
                }
                else if (state == AMKState.RecordingPause)
                {
                    this.StatusBarItem.StatusText = "Pause recording";
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-pause-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, new DoubleAnimation());
                }
                else if (state == AMKState.Stop)
                {
                    this.StatusBarItem.StatusText = string.Empty;
                    this.StatusBarItem.StatusImageSource = string.Empty;
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, new DoubleAnimation());
                }
                this.StatusBarItem.UpdateProperties();
            });
        }
    }
}
