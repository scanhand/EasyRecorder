using AMK.Global;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AMK.UI
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

        public void SetState(AMKState state)
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

        private string GetMessage(AMKState state)
        {
            return state.ToString();
        }

        private string GetImageSource(AMKState state)
        {
            switch (state)
            {
                default:
                case AMKState.Recording: return "/AutoMouseKeyboard;component/Resources/icons8-video-record-64.png";
            }
        }

        private string GetBGColor(AMKState state)
        {
            switch (state)
            {
                default:
                case AMKState.Recording: return Colors.LightGray.ToString();
            }
        }

    }
}
