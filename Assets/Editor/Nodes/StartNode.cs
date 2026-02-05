using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class StartNode : BaseNode
{
    public Port outPort;

    public List<TextField> texts = new();

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

        for (int i = 0; i < texts.Count; i++)
        {
            values.texts.Add(texts[i].value);
        }

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

    public override void Add()
    {
        TextField tf = CreateTextField();

        texts.Add(tf);
        mainContainer.Add(tf);
    }

    public override void Remove()
    {
        if(texts.Count > 0)
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