using AMK.Global;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;

namespace AMK.UI
{
    public class Preference : SingletonBase<Preference>
    {
        public bool IsTopMost { get; set; } = true;

        public bool IsShowToastMessage { get; set; } = true;

        public DoubleClickActionType DoubleClickAction { get; set; } = DoubleClickActionType.Memo;

        [JsonIgnore]
        public Window MainWindow { get; set; } = null;

        [JsonIgnore]
        public Window LogWindow { get; set; } = null;

        [JsonIgnore]
        public MenuItem MenuAlwaysTopItem { get; set; } = null;

        public bool Load()
        {

            return Adjust();
        }

        public bool Adjust()
        {
            this.MenuAlwaysTopItem.IsChecked = this.IsTopMost;
            this.MainWindow.Topmost = this.IsTopMost;
            this.LogWindow.Topmost = this.IsTopMost;

            AUtil.MoveToLeftBottom(this.LogWindow);
            return true;
        }

    }
}
