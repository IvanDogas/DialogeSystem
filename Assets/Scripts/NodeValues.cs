using UnityEngine;
using static TreeEditor.TreeEditorHelper;
using System.Collections.Generic;

[System.Serializable]
public class NodeValues
{
    public NodeType type;

    public List<Sprite> icons = new();
    public List<string> texts = new();

    public Sprite icon;

    public NodeValues nextNodeValue;
}

public enum NodeType
{
    Start,
    Response,
    Answer
}