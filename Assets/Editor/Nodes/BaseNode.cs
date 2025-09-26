using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Compilation;
using UnityEngine.UIElements;

public abstract class BaseNode : Node
{
    protected GraphView graph;

    public abstract NodeValues GetValues();

    public BaseNode(GraphView graph)
    {
        this.graph = graph;

        Button add = new() { text = "Add"};
        Button remove = new() { text = "Remove" };
        Button removeAll = new() { text = "Remove all" };

        add.clicked += Add;
        remove.clicked += Remove;
        removeAll.clicked += RemoveAll;

        mainContainer.Add(add);
        mainContainer.Add(remove);
        mainContainer.Add(removeAll);
    }

    protected abstract void Add();
    protected abstract void Remove();
    protected abstract void RemoveAll();
}

public class NodeValues
{
    NodeType type;
    public List<Sprite> icon = new();
    public List<string> text = new();
}

public enum NodeType
{
    Start,
    Response,
    Answer
}