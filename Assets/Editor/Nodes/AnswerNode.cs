using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
public class AnswerNode : BaseNode
{
    public Port inPort;

    public AnswerNodeData data;
    public List<TextFieldWithOutPort> list = new();

    public AnswerNode(AnswerNodeData nodeData, GraphView graph) : base(graph)
    {
        title = "Answers";
        data = nodeData;

        inPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(AnswerNode));
        inPort.portName = "In";
        inPort.portColor = Color.magenta;
        inputContainer.Add(inPort);

        RefreshExpandedState();
        RefreshPorts();
    }

    private VisualElement CreateListVisualElement(TextField tf, Port op)
    {
        VisualElement elem = new();
        elem.style.flexDirection = FlexDirection.Row;
        elem.style.alignItems = Align.Center;

        elem.Add(tf);
        elem.Add(op);

        return elem;
    }

    private TextField CreateListTextField()
    {
        TextField tf = new()
        {
            value = "Text"
        };
        tf.style.minWidth = 60;

        return tf;
    }

    private Port CreateListOutPort()
    {
        Port op = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(ResponseNode));
        op.portName = "Out";
        op.portColor = Color.cyan;

        return op;
    }

    public override NodeValues GetValues()
    {
        throw new System.NotImplementedException();
    }

    protected override void Add()
    {
        TextField tf = CreateListTextField();
        Port op = CreateListOutPort();
        VisualElement elem = CreateListVisualElement(tf, op);

        list.Add(new TextFieldWithOutPort(elem, tf, op));
        mainContainer.Add(elem);

        Debug.Log("Added Element");
    }

    protected override void Remove()
    {
        if (list.Count > 0)
        {
            TextFieldWithOutPort t = list[list.Count - 1];

            graph.DeleteElements(t.outPort.connections);

            mainContainer.Remove(t.elem);
            list.RemoveAt(list.Count - 1);

            Debug.Log("Removed Element");
        }
    }

    protected override void RemoveAll()
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            graph.DeleteElements(list[i].outPort.connections);

            mainContainer.Remove(list[i].elem);
            list.RemoveAt(i);
        }

        Debug.Log("Removed all Elements");
    }
}

public class AnswerNodeData
{
    public List<TextFieldWithOutPort> values = new();
}

public class TextFieldWithOutPort
{
    public VisualElement elem;
    public TextField txtField;
    public string txtValue;
    public Port outPort;

    public TextFieldWithOutPort(VisualElement elem, TextField tf, Port op)
    {
        this.elem = elem;
        txtField = tf;
        outPort = op;
    }
}
