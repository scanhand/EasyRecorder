using Aga.Diagrams.FlowChart;
using System.Windows.Controls;

namespace AMK.Group
{
    /// <summary>
    /// Interaction logic for AMKGroupControl.xaml
    /// </summary>
    public partial class AMKGroupControl : UserControl
    {
        public AMKGroupControl()
        {
            InitializeComponent();

            var model = CreateModel();

            this.editorDiagram.Controller = new Controller(this.editorDiagram, model);
            this.editorDiagram.DragDropTool = new DragDropTool(this.editorDiagram, model);
            this.editorDiagram.DragTool = new CustomMoveResizeTool(this.editorDiagram, model)
            {
                MoveGridCell = this.editorDiagram.GridCellSize
            };
            this.editorDiagram.LinkTool = new CustomLinkTool(this.editorDiagram);
        }

        private FlowchartModel CreateModel()
        {
            var model = new FlowchartModel();

            var start = new FlowNode(NodeKinds.Start);
            start.Row = 0;
            start.Column = 1;
            start.Text = "Start";

            var act = new FlowNode(NodeKinds.Action);
            act.Row = 1;
            act.Column = 1;
            act.Text = "i = 0";

            var cond = new FlowNode(NodeKinds.Condition);
            cond.Row = 2;
            cond.Column = 1;
            cond.Text = "i < n";

            var end = new FlowNode(NodeKinds.End);
            end.Row = 3;
            end.Column = 1;
            end.Text = "End";

            model.Nodes.Add(start);
            model.Nodes.Add(cond);
            model.Nodes.Add(act);
            model.Nodes.Add(end);

            model.Links.Add(new Link(start, PortKinds.Bottom, act, PortKinds.Top));
            model.Links.Add(new Link(act, PortKinds.Bottom, cond, PortKinds.Top));
            model.Links.Add(new Link(cond, PortKinds.Bottom, end, PortKinds.Top) { Text = "True" });
            return model;
        }
    }
}
