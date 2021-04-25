using Aga.Diagrams.FlowChart;
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

			var ac0 = new FlowNode(NodeKinds.Action);
			ac0.Row = 1;
			ac0.Column = 1;
			ac0.Text = "i = 0";

			var cond = new FlowNode(NodeKinds.Condition);
			cond.Row = 2;
			cond.Column = 1;
			cond.Text = "i < n";

			var ac1 = new FlowNode(NodeKinds.Action);
			ac1.Row = 3;
			ac1.Column = 1;
			ac1.Text = "do something";

			var ac2 = new FlowNode(NodeKinds.Action);
			ac2.Row = 4;
			ac2.Column = 1;
			ac2.Text = "i++";

			var end = new FlowNode(NodeKinds.End);
			end.Row = 3;
			end.Column = 2;
			end.Text = "End";

			model.Nodes.Add(start);
			model.Nodes.Add(cond);
			model.Nodes.Add(ac0);
			model.Nodes.Add(ac1);
			model.Nodes.Add(ac2);
			model.Nodes.Add(end);

			model.Links.Add(new Link(start, PortKinds.Bottom, ac0, PortKinds.Top));
			model.Links.Add(new Link(ac0, PortKinds.Bottom, cond, PortKinds.Top));

			model.Links.Add(new Link(cond, PortKinds.Bottom, ac1, PortKinds.Top) { Text = "True" });
			model.Links.Add(new Link(cond, PortKinds.Right, end, PortKinds.Top) { Text = "False" });

			model.Links.Add(new Link(ac1, PortKinds.Bottom, ac2, PortKinds.Top));
			model.Links.Add(new Link(ac2, PortKinds.Bottom, cond, PortKinds.Top));

			return model;
		}
	}
}
