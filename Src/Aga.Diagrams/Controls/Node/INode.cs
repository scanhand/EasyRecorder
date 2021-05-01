using System.Collections.Generic;

namespace Aga.Diagrams.Controls
{
    public interface INode
    {
        IEnumerable<IPort> Ports { get; }
    }
}
