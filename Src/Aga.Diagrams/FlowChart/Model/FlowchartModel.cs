using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Aga.Diagrams.FlowChart
{
    public class FlowchartModel
    {
        private ObservableCollection<FlowNode> _nodes = new ObservableCollection<FlowNode>();
        public ObservableCollection<FlowNode> Nodes
        {
            get { return _nodes; }
        }

        private ObservableCollection<Link> _links = new ObservableCollection<Link>();
        public ObservableCollection<Link> Links
        {
            get { return _links; }
        }
    }
}
