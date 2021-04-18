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

        public AMKStatusBar()
        {
            InitializeComponent();
            this.DataContext = this.StatusBarItem;
        }

        public void SetCurrentState(AMKState state)
        {
            this.InvokeIfRequired(() =>
            {
                if (state == AMKState.Playing)
                {
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-play-64.png";
                }
                else if(state == AMKState.Recording)
                {
                    this.StatusBarItem.StatusImageSource = "/AutoMouseKeyboard;component/Resources/icons8-video-record-64.png";
                }

                this.StatusBarItem.UpdateProperties();
            });
        }
    }
}
