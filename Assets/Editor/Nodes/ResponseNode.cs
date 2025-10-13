using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class ResponseNode : BaseNode
{
    private Port inPort;
    private Port outPort;

    public ResponseNode(GraphView graph) : base(graph)
    {
        title = "Response";

        inPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(ResponseNode));
        inPort.portName = "In";
        inPort.portColor = Color.cyan;

        outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(AnswerNode));
        outPort.portName = "Out";
        outPort.portColor = Color.magenta;

        inputContainer.Add(inPort);
        outputContainer.Add(outPort);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override List<Port> GetPorts()
    {
        List<Port> ports = new();

        foreach (Edge item in outPort.connections)
        {
            if (item.input != null)
            {
                ports.Add(item.input);
            }
        }

        return ports;
    }

    public override NodeValues GetValues()
    {
        NodeValues values = new();

        values.type = NodeType.Response;

        values.code = code;

        return values;
    }

    protected override void Add()
    {
        Debug.Log("Add-response");
    }

    protected override void Remove()
    {
        Debug.Log("Remove-response");
    }

    protected override void RemoveAll()
    {
        Debug.Log("RemoveAll-response");
    }
}
