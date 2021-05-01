using Aga.Diagrams.Controls;
using System.Windows;

namespace Aga.Diagrams.Tools
{
    public interface IMoveResizeTool
    {
        void BeginDrag(Point start, DiagramItem item, DragThumbKinds kind);
        void DragTo(Vector vector);
        bool CanDrop();
        void EndDrag(bool doCommit);
    }
}
