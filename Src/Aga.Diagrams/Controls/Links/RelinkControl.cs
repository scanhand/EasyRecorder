using System.Windows;
using System.Windows.Controls;

namespace Aga.Diagrams.Controls
{
    public class RelinkControl : Control
    {
        static RelinkControl()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RelinkControl), new FrameworkPropertyMetadata(typeof(RelinkControl)));
        }
    }
}
