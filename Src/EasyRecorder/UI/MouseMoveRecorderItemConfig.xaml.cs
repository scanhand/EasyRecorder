using ESR.Recorder;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ESR.UI
{
    /// <summary>
    /// Interaction logic for MouseMoveRecorderItemConfig.xaml
    /// </summary>
    public partial class MouseMoveRecorderItemConfig : MetroWindow, IRecorderItemConfig
    {
        #region inner

        #endregion

        public IRecorderItem RecorderItem { get; set; }

        private IKeyRecorderItem RecorderKeyItem
        {
            get
            {
                return this.RecorderItem as IKeyRecorderItem;
            }
        }

        public MouseMoveRecorderItemConfig()
        {
            InitializeComponent();

            this.KeyDown += (e, k) =>
            {
                if (k.Key == Key.Enter)
                    buttonOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };

            this.Loaded += MouseMoveRecorderItemConfig_Loaded;
        }

        private void MouseMoveRecorderItemConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.RecorderItem.Recorder.ToString() + " Configuration";

            TimeSpan timeDuration = TimeSpan.FromSeconds(0);
            DateTime timeStart = this.RecorderItem.Time;
            int startX = (int)this.RecorderItem.Point.X;
            int startY = (int)this.RecorderItem.Point.Y;
            this.textBoxStartX.Text = string.Format("{0}", startX);
            this.textBoxStartY.Text = string.Format("{0}", startY);

            int endX = startX;
            int endY = startY;
            if(this.RecorderItem.ChildItems.LastOrDefault() != null)
            {
                timeDuration = this.RecorderItem.ChildItems.Last().Time - timeStart;
                endX = (int)this.RecorderItem.ChildItems.Last().Point.X;
                endY = (int)this.RecorderItem.ChildItems.Last().Point.Y;
            }

            this.textBoxEndX.Text = string.Format("{0}", endX);
            this.textBoxEndY.Text = string.Format("{0}", endY);
            this.textBoxTimeDuration.Text = string.Format("{0:F2}", timeDuration.TotalSeconds);
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            int startX = 0;
            int startY = 0;
            int endX = 0;
            int endY = 0;
            double timeDuration = 0;
            try
            {
                startX = int.Parse(this.textBoxStartX.Text);
                startY = int.Parse(this.textBoxStartY.Text);
                endX = int.Parse(this.textBoxEndX.Text);
                endY = int.Parse(this.textBoxEndY.Text);
                timeDuration = double.Parse(this.textBoxTimeDuration.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.RecorderItem.Point = new Point(startX, startY);
            this.RecorderItem.ChildItems.Clear();

            int deltaLength = 0;
            if(Math.Abs(endX - startX) > Math.Abs(endY - startY))
                deltaLength = Math.Abs(endX - startX);
            else
                deltaLength = Math.Abs(endY - startY);

            double deltaX = ((double)endX - startX) / (double)deltaLength;
            double deltaY = ((double)endY - startY) / (double)deltaLength;
            double deltaTime = timeDuration / (double)deltaLength;

            IMouseRecorderItem rootItem = this.RecorderItem as IMouseRecorderItem;
            int newPosX = startX;
            int newPosY = startY;
            for (int i=0; i<deltaLength; i++)
            {
                this.RecorderItem.ChildItems.Add(new MouseMoveRecorderItem()
                {
                    Dir = this.RecorderItem.Dir,
                    MouseData = rootItem.MouseData,
                    Point = new Point(startX + ((double)i*deltaX), startY + ((double)i * deltaY)),
                    Time = this.RecorderItem.Time + TimeSpan.FromSeconds(deltaTime * (double)i),
                });
            }

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
