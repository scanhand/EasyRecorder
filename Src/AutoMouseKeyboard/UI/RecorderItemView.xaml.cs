using AMK.Recorder;
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

namespace AMK.UI
{
    /// <summary>
    /// Interaction logic for RecordeItemView.xaml
    /// </summary>
    public partial class RecorderItemView : UserControl
    {
        public AMKRecorder Recorder { get; set; } = null;

        public RecorderItemView()
        {
            InitializeComponent();
        }

        private void MenuItem_PlaySelectedItems_Click(object sender, RoutedEventArgs e)
        {
            if (this.RecoderListView.SelectedItems == null || this.RecoderListView.SelectedItems.Count <= 0)
                return;

            this.Recorder.Player.ResetLastItem();
            foreach(var i in this.RecoderListView.SelectedItems)
            {
                IRecorderItem item = i as IRecorderItem;
                ALog.Debug("PlaySelectedItems::Recorder={0}", item.Recorder);
                item.Play(this.Recorder.Player);

                Thread.Sleep(1);
            }
        }
    }
}
