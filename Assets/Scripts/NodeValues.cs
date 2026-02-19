using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class NodeValues
{
    public NodeType type;

    public List<string> texts = new();

    public int code;
    public List<int> nextNodeCode = new();
}

public enum NodeType
{
    Start,
    Response,
    Answer
}