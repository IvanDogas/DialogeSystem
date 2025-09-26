using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public abstract class BaseNode : Node
{
    protected GraphView graph;

    public abstract void GetValues();

    public BaseNode(GraphView graph)
    {
        this.graph = graph;
    }
}

public class NodeValues
{
    public Sprite icon;
    public string text;
}