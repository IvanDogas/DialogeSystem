using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class NodeValues
{
    public NodeType type;

    public List<Sprite> icons = new();
    public List<string> texts = new();

    public Sprite icon;

    public int code;
    public List<int> nextNodeCode = new();
}

public enum NodeType
{
    Start,
    Response,
    Answer
}