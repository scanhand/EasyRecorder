using ESR.Global;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ESR.UI
{
    /// <summary>
    /// Interaction logic for ToastWindow.xaml
    /// </summary>
    public partial class ToastWindow : Window
    {
        private ToastMessageItem ToastMessage { get; set; } = new ToastMessageItem();

        private readonly DoubleAnimation FadeInOutAnimation = null;

        public ToastWindow()
        {
            InitializeComponent();

            this.ShowInTaskbar = false;

            this.DataContext = this.ToastMessage;

            this.Topmost = true;

            this.FadeInOutAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
            };

            AUtil.MoveToRightBottom(this);
        }

        public void SetState(ESRState state)
        {
            this.InvokeIfRequired(() =>
            {
                this.ToastMessage.ToastMessage = GetMessage(state);
                this.ToastMessage.ToastImageSource = GetImageSource(state);
                this.ToastMessage.ToastBGColor = GetBGColor(state);
                this.imgToast.BeginAnimation(OpacityProperty, this.FadeInOutAnimation);
                this.ToastMessage.UpdateProperties();
            });
        }

        private string GetMessage(ESRState state)
        {
            return state.ToString();
        }

        private string GetImageSource(ESRState state)
        {
            switch (state)
            {
                default:
                case ESRState.Recording: return "/EasyRecorder;component/Resources/icons8-video-record-64.png";
                case ESRState.Playing: return "/EasyRecorder;component/Resources/icons8-play-64.png";
            }
        }

        private string GetBGColor(ESRState state)
        {
            switch (state)
            {
                default:
                case ESRState.Recording: return Colors.LightGray.ToString();
                case ESRState.Playing: return Colors.LightGray.ToString();
            }
        }

    }
}
