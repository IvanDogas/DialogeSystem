using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public abstract class BaseNode : Node
{
    protected GraphView graph;
    public int code;

    public abstract NodeValues GetValues();

    public abstract List<Port> GetPorts();

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

