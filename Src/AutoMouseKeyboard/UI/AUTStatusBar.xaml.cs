using AUT.Global;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AUT.UI
{
    /// <summary>
    /// Interaction logic for AUTStatusBar.xaml
    /// </summary>
    public partial class AUTStatusBar : UserControl
    {
        public AUTStatusBarItem StatusBarItem { get; set; } = new AUTStatusBarItem();

        private DoubleAnimation FadeInOutAnimation = null;

        public AUTStatusBar()
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

        public void SetCurrentState(AUTState state)
        {
            this.InvokeIfRequired(() =>
            {
                if (state == AUTState.Playing)
                {
                    this.StatusBarItem.StatusText = "Playing...";
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-play-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, this.FadeInOutAnimation);
                }
                else if (state == AUTState.PlayingPause)
                {
                    this.StatusBarItem.StatusText = "Pause playing";
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-pause-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, new DoubleAnimation());
                }
                else if (state == AUTState.PlayDone)
                {
                    this.StatusBarItem.StatusText = "Done";
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-simplestop-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, new DoubleAnimation());
                }
                else if (state == AUTState.Recording)
                {
                    this.StatusBarItem.StatusText = "Recording...";
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-video-record-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, this.FadeInOutAnimation);
                }
                else if (state == AUTState.RecordingPause)
                {
                    this.StatusBarItem.StatusText = "Pause recording";
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-pause-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, new DoubleAnimation());
                }
                else if (state == AUTState.Stop)
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
