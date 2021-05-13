using AUT.Global;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AUT.UI
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

        public void SetState(AUTState state)
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

        private string GetMessage(AUTState state)
        {
            return state.ToString();
        }

        private string GetImageSource(AUTState state)
        {
            switch (state)
            {
                default:
                case AUTState.Recording: return "/AutoUnitTesting;component/Resources/icons8-video-record-64.png";
                case AUTState.Playing: return "/AutoUnitTesting;component/Resources/icons8-play-64.png";
            }
        }

        private string GetBGColor(AUTState state)
        {
            switch (state)
            {
                default:
                case AUTState.Recording: return Colors.LightGray.ToString();
                case AUTState.Playing: return Colors.LightGray.ToString();
            }
        }

    }
}
