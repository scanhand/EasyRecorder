using System.Windows;

namespace Aga.Diagrams.Adorners
{
    public class MoveResizeAdorner : DragAdorner
    {
        public MoveResizeAdorner(DiagramView view, Point start)
            : base(view, start)
        {
        }

        protected override bool DoDrag()
        {
            View.DragTool.DragTo(End - Start);
            return View.DragTool.CanDrop();
        }

        protected override void EndDrag()
        {
            View.DragTool.EndDrag(DoCommit);
        }
    }
}
