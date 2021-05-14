using ESR.Global;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ESR.UI
{
    /// <summary>
    /// Interaction logic for ESRStatusBar.xaml
    /// </summary>
    public partial class ESRStatusBar : UserControl
    {
        public ESRStatusBarItem StatusBarItem { get; set; } = new ESRStatusBarItem();

        private DoubleAnimation FadeInOutAnimation = null;

        public ESRStatusBar()
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

        public void SetCurrentState(ESRState state)
        {
            this.InvokeIfRequired(() =>
            {
                if (state == ESRState.Playing)
                {
                    this.StatusBarItem.StatusText = "Playing...";
                    this.StatusBarItem.StatusImageSource = "/EasyRecorder;component/Resources/icons8-play-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, this.FadeInOutAnimation);
                }
                else if (state == ESRState.PlayingPause)
                {
                    this.StatusBarItem.StatusText = "Pause playing";
                    this.StatusBarItem.StatusImageSource = "/EasyRecorder;component/Resources/icons8-pause-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, new DoubleAnimation());
                }
                else if (state == ESRState.PlayDone)
                {
                    this.StatusBarItem.StatusText = "Done";
                    this.StatusBarItem.StatusImageSource = "/EasyRecorder;component/Resources/icons8-simplestop-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, new DoubleAnimation());
                }
                else if (state == ESRState.Recording)
                {
                    this.StatusBarItem.StatusText = "Recording...";
                    this.StatusBarItem.StatusImageSource = "/EasyRecorder;component/Resources/icons8-video-record-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, this.FadeInOutAnimation);
                }
                else if (state == ESRState.RecordingPause)
                {
                    this.StatusBarItem.StatusText = "Pause recording";
                    this.StatusBarItem.StatusImageSource = "/EasyRecorder;component/Resources/icons8-pause-64.png";
                    this.imgStatusStatusBar.BeginAnimation(OpacityProperty, new DoubleAnimation());
                }
                else if (state == ESRState.Stop)
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
