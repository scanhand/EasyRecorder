using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Aga.Diagrams.Adorners
{
    public class RubberbandAdorner : DragAdorner
    {
        private Pen PenBorder;
        private Brush BrushBG;

        public RubberbandAdorner(DiagramView view, Point start)
            : base(view, start)
        {
            this.PenBorder = new Pen(Brushes.Black, 1);
            this.PenBorder.DashStyle = new DashStyle(new double[] { 5, 5 }, 0);

            this.BrushBG = new SolidColorBrush(Colors.Gray);
            this.BrushBG.Opacity = 0.5;
        }

        protected override bool DoDrag()
        {
            InvalidateVisual();
            return true;
        }

        protected override void EndDrag()
        {
            if (DoCommit)
            {
                var rect = new Rect(Start, End);
                var items = View.Items.Where(p => p.CanSelect && rect.Contains(p.Bounds));
                View.Selection.SetRange(items);
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(this.BrushBG,
                             this.PenBorder,
                             new Rect(Start, End));
        }
    }
}
