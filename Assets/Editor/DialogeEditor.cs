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

        Label label = new();
        label.text = "Upate DialogeInfo:";
        label.style.marginTop = 2;

        ObjectField updateDialogeInfo = new();
        updateDialogeInfo.objectType = typeof(DialogeInfo);
        
        toolbar.Add(label); 
        toolbar.Add(updateDialogeInfo);

        rootVisualElement.Add(toolbar);

        startNode = new(graph);
        startNode.SetPosition(new Rect(0, 200, 150, 200));
        graph.AddElement(startNode);
    }

    private void OnDestroy()
    {
        Debug.Log("Closed");
    }

    private void OnSelectionChange()
    {
        Debug.Log("Changed");
    }

    private void CreateAsset()
    {
        Debug.Log(graph.selection.Count);

        for (int i = 0; i < graph.selection.Count; i++)
        {
            if (graph.selection[i] is BaseNode)
            {
                BaseNode node = (BaseNode)graph.selection[i];
                Debug.Log(node.GetValues().type);
            }

        }
    }

    private void CreateANode()
    {
        AnswerNode a = new(graph);
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