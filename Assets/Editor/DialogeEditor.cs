using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class DialogeEditor : EditorWindow
{
    [MenuItem("Editor/DialogeCreator")]
    public static void OpenWindow()
    {
        GetWindow<DialogeEditor>();
    }

    DialogeGraphView graph;
    StartNode startNode;

    private void OnEnable()
    {
        graph = new DialogeGraphView
        {
            name = "Dialoge"
        };
        graph.StretchToParentSize();
        rootVisualElement.Add(graph);

        Toolbar toolbar = new();

        Button createAssetButton = new Button();

        createAssetButton.text = "Create asset";
        createAssetButton.clicked += CreateAsset;

        Button createAnswerNode = new();
        createAnswerNode.text = "New Answer Node";
        createAnswerNode.clicked += CreateANode;

        Button createResponseNodde = new();
        createResponseNodde.text = "New Response Node";
        createResponseNodde.clicked += CreateRNode;

        toolbar.Add(createAssetButton);
        toolbar.Add(createAnswerNode);
        toolbar.Add(createResponseNodde);

        rootVisualElement.Add(toolbar);

        startNode = new(graph);
        startNode.SetPosition(new Rect(0, 200, 150, 200));
        graph.AddElement(startNode);

        AnswerNode a = new(new(),graph);
        a.SetPosition(new Rect(200, 200, 150, 200));
        graph.AddElement(a);

        ResponseNode r = new(graph);
        r.SetPosition(new Rect(400, 200, 150, 200));
        graph.AddElement(r);
    }

    private void CreateAsset()
    {
        Debug.Log(startNode.textField.value);
    }

    private void CreateANode()
    {
        AnswerNode a = new(new(),graph);
        a.SetPosition(new Rect(0, 0, 150, 200));
        graph.AddElement(a);
    }

    private void CreateRNode()
    {
        ResponseNode r = new(graph);
        r.SetPosition(new Rect(0, 0, 150, 200));
        graph.AddElement(r);
    }
}