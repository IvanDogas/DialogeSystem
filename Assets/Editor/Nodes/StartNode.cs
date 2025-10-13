using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class StartNode : BaseNode
{
    public TextField textField;
    public Port outPort;

    public List<TextFieldWithObjectField> fields = new();


    public StartNode(GraphView graph) : base(graph)
    {
        title = "Start-Intro";      

        capabilities -= Capabilities.Deletable;

        outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(AnswerNode));
        outPort.portName = "Out";
        outPort.portColor = Color.magenta;
        outputContainer.Add(outPort);

        RefreshExpandedState();
        RefreshPorts();
    }
    public override NodeValues GetValues()
    {
        NodeValues values = new NodeValues();
        
        values.type = NodeType.Start;

        values.code = code;

        return values;
    }

    public override List<Port> GetPorts()
    {
        List<Port> ports = new();

        foreach (Edge item in outPort.connections)
        {
            if(item.input != null)
            {
                ports.Add(item.input);
            }
        }
        return ports;
    }

    protected override void Add()
    {
        TextField tf = CreateTextField();
        ObjectField of = CreateObjectField();
        VisualElement elem = CreateVisualElement(of, tf);

        fields.Add(new(elem,tf,of));
        mainContainer.Add(elem);
    }

    protected override void Remove()
    {
        if(fields.Count > 0)
        {
            TextFieldWithObjectField field = fields[fields.Count - 1];

            mainContainer.Remove(field.elem);
            fields.Remove(field);
        }
    }

    protected override void RemoveAll()
    {
        for (int i = fields.Count - 1; i >= 0; i--)
        {
            TextFieldWithObjectField field = fields[i];

            mainContainer.Remove(field.elem);
            fields.Remove(field);
        }
    }

    private TextField CreateTextField()
    {
        TextField tf = new()
        {
            value = "Text"
        };
        tf.style.minWidth = 120;

        return tf;
    }

    private ObjectField CreateObjectField()
    {
        ObjectField of = new();
        of.objectType = typeof(Sprite);
        of.style.maxWidth = 80;

        return of;
    }

    private VisualElement CreateVisualElement(ObjectField of, TextField tf)
    {
        VisualElement elem = new();
        elem.style.flexDirection = FlexDirection.Row;
        elem.style.alignItems = Align.Center;
        
        elem.Add(of);
        elem.Add(tf);

        return elem;
    }
}

public class TextFieldWithObjectField
{
    public VisualElement elem;
    public TextField tf;
    public ObjectField of;

    public TextFieldWithObjectField(VisualElement elem,TextField tf,ObjectField of)
    {
        this.elem = elem;
        this.tf = tf;
        this.of = of;
    }
        
}