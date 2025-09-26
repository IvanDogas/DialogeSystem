using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class StartNode : BaseNode
{
    public TextField textField;
    public Port outPort;
    

    public StartNode(GraphView graph) : base(graph)
    {
        title = "Start-Intro";

        outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(AnswerNode));
        outPort.portName = "Out";
        outPort.portColor = Color.magenta;
        outputContainer.Add(outPort);

        textField = new()
        {
            value = "Text"
        };
        mainContainer.Add(textField);

        capabilities -= Capabilities.Deletable;

        RefreshExpandedState();
        RefreshPorts();
    }

    public override NodeValues GetValues()
    {
        throw new System.NotImplementedException();
    }

    protected override void Add()
    {
        Debug.Log("Add-start");
    }

    protected override void Remove()
    {
        Debug.Log("Remvoe-start");
    }

    protected override void RemoveAll()
    {
        Debug.Log("RemoveAll-start");
    }
}