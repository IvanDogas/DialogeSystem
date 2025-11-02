using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.VersionControl;
using Unity.VisualScripting;
using PlasticGui.WorkspaceWindow.Security;

public class DialogeEditor : EditorWindow
{
    [MenuItem("Editor/DialogeCreator")]
    public static void OpenWindow()
    {
        GetWindow<DialogeEditor>();
        GetWindow<TEST>();
    }

    DialogeGraphView graph;
    StartNode startNode;

    private int nodeAmount;

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
        startNode.code = 0;
        nodeAmount = 1;
        startNode.SetPosition(new Rect(0, 200, 150, 200));
        graph.AddElement(startNode);
    }

    private void OnDestroy()
    {
        Debug.Log("Closed");
    }

    private void OnSelectionChange()
    {
        if(graph.selection.Count > 0)
        Debug.Log("Changed");
    }

    private void CreateAsset()
    {
        List<NodeValues> values = new();

        List<BaseNode> q = new();

        NodeValues startValue = startNode.GetValues();

        if(startNode.GetPorts().Count > 0)
        {
            BaseNode startNextNode = (BaseNode)startNode.GetPorts()[0].node;

            q.Add(startNextNode);
            startValue.nextNodeCode.Add(startNextNode.code);
        }

        values.Add(startValue);

        do
        {
            if(q.Count > 0)
            {
                List<BaseNode> nextQ = new();

                for (int i = 0; i < q.Count; i++)
                {
                    NodeValues value = q[i].GetValues();
                    List<Port> ports = q[i].GetPorts();

                    switch (value.type)
                    {
                        case NodeType.Answer:
                            if(ports.Count > 0)
                            {
                                for(int ii = 0; ii < ports.Count; ii++)
                                {
                                    BaseNode nextNode = (BaseNode)ports[ii].node;

                                    value.nextNodeCode.Add(nextNode.code);

                                    if(!nextQ.Contains(nextNode)) nextQ.Add(nextNode);
                                }
                            }

                            break;
                        
                        case NodeType.Response:
                            if(ports.Count > 0)
                            {
                                BaseNode nextNode = (BaseNode)ports[0].node;
                                value.nextNodeCode.Add(nextNode.code);

                                if (!nextQ.Contains(nextNode)) nextQ.Add(nextNode);
                            }
                            break;
                        
                        default:
                            break;
                    }
                    values.Add(value);
                }
                q = nextQ;
            }
            else
            {
                break;
            }
        } while (true);

        DialogeInfo asset = CreateInstance<DialogeInfo>();

        string name = AssetDatabase.GenerateUniqueAssetPath("Assets/DialogeInfo.asset");
        AssetDatabase.CreateAsset(asset, name);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

        asset.values = values;
    }


    private void CreateANode()
    {
        AnswerNode a = new(graph);
        a.code = nodeAmount;
        nodeAmount++;
        a.SetPosition(new Rect(0, 0, 150, 200));
        graph.AddElement(a);
    }

    private void CreateRNode()
    {
        ResponseNode r = new(graph);
        r.code = nodeAmount;
        nodeAmount++;
        r.SetPosition(new Rect(0, 0, 150, 200));
        graph.AddElement(r);
    }
}