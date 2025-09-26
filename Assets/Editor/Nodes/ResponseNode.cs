using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class ResponseNode : BaseNode
{
    Port inPort;
    Port outPort;

    public ResponseNode(GraphView graph) : base(graph)
    {
        title = "Response";

        inPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(ResponseNode));
        inPort.portName = "In";
        inPort.portColor = Color.cyan;

        outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(AnswerNode));
        outPort.portName = "Out";
        outPort.portColor = Color.magenta;

        inputContainer.Add(inPort);
        outputContainer.Add(outPort);
    }

    public override void GetValues()
    {
        throw new System.NotImplementedException();
    }
}
