using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class DialogeGraphView : GraphView
{
    public DialogeGraphView()
    {
        AddManipulators();
        InsertGrid();
    }

    private void AddManipulators()
    {
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
    }

    private void InsertGrid()
    {
        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new();

        ports.ForEach(port =>
        {
            if (startPort == port || startPort.node == port.node || startPort.direction == port.direction || startPort.portType != port.portType)
            {
                return;
            }

            compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }
}