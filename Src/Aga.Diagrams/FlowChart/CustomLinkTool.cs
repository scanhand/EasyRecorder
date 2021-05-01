using Aga.Diagrams.Controls;
using Aga.Diagrams.Tools;
using System.Windows;

namespace Aga.Diagrams.FlowChart
{
    public class CustomLinkTool : LinkTool
    {
        public CustomLinkTool(DiagramView view)
            : base(view)
        {
        }

        protected override ILink CreateNewLink(IPort port)
        {
            var link = new OrthogonalLink();
            BindNewLinkToPort(port, link);
            return link;
        }

        protected override void UpdateLink(Point point, IPort port)
        {
            base.UpdateLink(point, port);
            var link = Link as OrthogonalLink;
        }
    }
}
