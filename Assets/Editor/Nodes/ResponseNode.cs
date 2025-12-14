using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class ResponseNode : BaseNode
{
    public Port inPort;
    public Port outPort;

    public List<TextField> texts = new();

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

        for (int i = 0; i < texts.Count; i++)
        {
            values.texts.Add(texts[i].value);
        }

        values.code = code;

        return values;
    }

    public override void Add()
    {
        TextField tf = CreateTextField();

        texts.Add(tf);
        mainContainer.Add(tf);
    }

    public override void Remove()
    {
        if (texts.Count > 0)
        {
            TextField field = texts[texts.Count - 1];

            mainContainer.Remove(field);
            texts.Remove(field);
        }
    }

    public override void RemoveAll()
    {
        for (int i = texts.Count - 1; i >= 0; i--)
        {
            TextField field = texts[i];

            mainContainer.Remove(field);
            texts.Remove(field);
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
}