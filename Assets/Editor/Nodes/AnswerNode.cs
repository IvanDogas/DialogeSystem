using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
public class AnswerNode : BaseNode
{
    public Port inPort;
    public ObjectField iconField;

    public List<TextFieldWithOutPort> list = new();

    public AnswerNode(GraphView graph) : base(graph)
    {
        title = "Answers";

        inPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(AnswerNode));
        inPort.portName = "In";
        inPort.portColor = Color.magenta;
        inputContainer.Add(inPort);

        iconField = new();
        iconField.objectType = typeof(Sprite);

        mainContainer.Add(iconField);

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
        NodeValues values = new();

        values.type = NodeType.Answer;

        for (int i = 0; i < list.Count; i++)
        {
            values.texts.Add(list[i].txtField.value);
        }

        values.code = code;

        return values;
    }

    protected override void Add()
    {
        TextField tf = CreateListTextField();
        Port op = CreateListOutPort();
        VisualElement elem = CreateListVisualElement(tf, op);

        list.Add(new TextFieldWithOutPort(elem, tf, op));
        mainContainer.Add(elem);
    }

    protected override void Remove()
    {
        if (list.Count > 0)
        {
            TextFieldWithOutPort t = list[list.Count - 1];

            graph.DeleteElements(t.outPort.connections);

            mainContainer.Remove(t.elem);
            list.RemoveAt(list.Count - 1);
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
    }

    public override List<Port> GetPorts()
    {
        List<Port> ports = new();

        for (int i = list.Count - 1; i >= 0; i--)
        {
            foreach (Edge item in list[i].outPort.connections)
            {
                if(item.input != null)
                {
                    ports.Add(item.input);
                }
            }
        }

        return ports;
    }
}

public class TextFieldWithOutPort
{
    public VisualElement elem;
    public TextField txtField;
    public Port outPort;

    public TextFieldWithOutPort(VisualElement elem, TextField tf, Port op)
    {
        this.elem = elem;
        txtField = tf;
        outPort = op;
    }
}
