using Aga.Diagrams.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Aga.Diagrams.Adorners
{
    public class SelectionAdorner : Adorner
    {
        private VisualCollection _visuals;
        private Control _control;

        protected override int VisualChildrenCount
        {
            get { return _visuals.Count; }
        }

        public SelectionAdorner(DiagramItem item, Control control)
            : base(item)
        {
            _control = control;
            _control.DataContext = item;
            _visuals = new VisualCollection(this);
            _visuals.Add(_control);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _control.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }
    }
}
