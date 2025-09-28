using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

public class DialogeGraphView : GraphView
{
    public DialogeGraphView()
    {
        AddManipulators();
        InsertGrid();
    }

    public ContentZoomer zoomer;
    public ContentDragger cDragger;
    public SelectionDragger dragger;
    public RectangleSelector selector;

    private void AddManipulators()
    {
        zoomer = new();
        cDragger = new();
        dragger = new();
        selector = new();

        this.AddManipulator(zoomer);
        this.AddManipulator(cDragger);
        this.AddManipulator(dragger);
        this.AddManipulator(selector);
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