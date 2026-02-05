using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using System;

public class DialogeEditor : EditorWindow
{
    [MenuItem("Editor/DialogeCreator")]
    public static void OpenWindow()
    {
        GetWindow<DialogeEditor>();
    }

    private DialogeGraphView graph;
    private Toolbar toolbar;
    private Button loadFromAsset;
    private Button updateAsset;
    private StartNode startNode;
    private ObjectField loadNodeDialogeInfo;

    List<BaseNode> activeNodes = new();

    private int nodeAmount;

    private void OnEnable()
    {
        graph = new DialogeGraphView
        {
            name = "Dialoge"
        };
        graph.StretchToParentSize();
        rootVisualElement.Add(graph);       

        toolbar = new();

        Button createAssetButton = new Button();

        createAssetButton.text = "Create asset";
        createAssetButton.clicked += CreateAsset;

        Button createAnswerNode = new();
        createAnswerNode.text = "New Answer Node";
        createAnswerNode.clicked += CreateANode;

        Button createResponseNodde = new();
        createResponseNodde.text = "New Response Node";
        createResponseNodde.clicked += CreateRNode;

        loadFromAsset = new();
        loadFromAsset.text = "Load";
        loadFromAsset.clicked += LoadFromAsset;

        updateAsset = new();
        updateAsset.text = "Update";
        updateAsset.clicked += UpdateAsset;

        toolbar.Add(createAssetButton);
        toolbar.Add(createAnswerNode);
        toolbar.Add(createResponseNodde);

        Label label = new();
        label.text = "Load DialogeInfo:";
        label.style.marginTop = 2;

        loadNodeDialogeInfo = new();
        loadNodeDialogeInfo.objectType = typeof(DialogeInfo);
        
        toolbar.Add(label); 
        toolbar.Add(loadNodeDialogeInfo);

        rootVisualElement.Add(toolbar);

        startNode = new(graph);
        startNode.code = 0;
        nodeAmount = 1;
        startNode.SetPosition(new Rect(0, 200, 150, 200));
        graph.AddElement(startNode);
    }

    private void Update()
    {
        if (loadNodeDialogeInfo.value != null)
        {
            if (!toolbar.Contains(loadFromAsset))
            {
                toolbar.Add(loadFromAsset);
                toolbar.Add(updateAsset);
            }
        }
        else
        {
            if (toolbar.Contains(loadFromAsset))
            {
                toolbar.Remove(loadFromAsset);
                toolbar.Remove(updateAsset);
            }
        }
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

    private void LoadFromAsset()
    {
        List<NodeValues> values = ((DialogeInfo)loadNodeDialogeInfo.value).values;

        if(activeNodes.Count > 0)
        {
            if(startNode.outPort.connected)
            {
                graph.DeleteElements(startNode.outPort.connections);
            }

            for (int i = 0; i < activeNodes.Count; i++)
            {
                if (activeNodes[i] != null)
                {
                    switch (activeNodes[i].GetValues().type)
                    {
                        case NodeType.Response:
                            ResponseNode rn = (ResponseNode)activeNodes[i];

                            if (rn.inPort.connected)
                            {
                                graph.DeleteElements(rn.inPort.connections);
                            }

                            if(rn.outPort.connected)
                            {
                                graph.DeleteElements(rn.outPort.connections);
                            }
                            break;
                        default:
                            break;
                    }

                    graph.RemoveElement(activeNodes[i]);
                }
            }
        }

        activeNodes = new();
        nodeAmount = 1;

        for (int i = 0; i < values.Count; i++)
        {
            switch (values[i].type)
            {
                case NodeType.Response:
                    CreateRNode();

                    ResponseNode r = (ResponseNode)activeNodes[activeNodes.Count - 1];

                    r.SetPosition(new Rect(100 + 100 * i,100,150,200));

                    r.code = values[i].code;

                    for (int j = 0; j < values[i].texts.Count;j++)
                    {
                        r.Add();
                        r.texts[j].value = values[i].texts[j];
                    }
                    break;
                case NodeType.Answer:
                    CreateANode();

                    AnswerNode a = (AnswerNode)activeNodes[activeNodes.Count - 1];

                    a.SetPosition(new Rect(150 * i, 200, 150, 200));

                    a.code = values[i].code;

                   for(int j = 0;j< values[i].texts.Count;j++)
                   {
                        a.Add();
                        a.list[j].txtField.value = values[i].texts[j];
                   }
                   break;
                default:
                    break;
            }
        }

        int num = 0;

        for (int i = 0; i < values.Count; i++)
        {
            if (values[i].code > num)
            {
                num = values[i].code;
            }
        }

        nodeAmount = num + 1;

        if (values.Count > 0)
        {
            for (int i = 0; i < values[0].texts.Count; i++)
            {
                if(startNode.texts.Count != values[0].texts.Count)
                startNode.Add();

                startNode.texts[i].value = values[0].texts[i];
            }

            startNode.outPort.ConnectTo(((AnswerNode)activeNodes[0]).inPort);
            foreach (Edge item in startNode.outPort.connections)
            {
                graph.AddElement(item);
            }
        }

        for (int i = 0; i < activeNodes.Count; i++)
        {
            List<int> nextCodes = new();
            NodeType type = NodeType.Answer;


            for (int j = 0; j < values.Count; j++)
            {
                if (activeNodes[i].code == values[j].code)
                {
                    nextCodes = values[j].nextNodeCode;
                    type = values[j].type;
                }
            }

            switch (type)
            {
                case NodeType.Response:
                    ResponseNode r = (ResponseNode)activeNodes[i];
                    if(nextCodes.Count > 0)
                    {
                        for (int j = 0;j < activeNodes.Count;j++)
                        {
                            if (activeNodes[j].code == nextCodes[0])
                            {
                                r.outPort.ConnectTo(((AnswerNode)activeNodes[j]).inPort);
                                foreach (Edge item in r.outPort.connections)
                                {
                                    graph.AddElement(item);
                                }
                            }
                        }
                    }
                    break;
                case NodeType.Answer:
                    AnswerNode a = (AnswerNode)activeNodes[i];

                    if(nextCodes.Count > 0)
                    {
                        for (int j = 0; j < nextCodes.Count; j++)
                        {
                            for(int k = 0; k < activeNodes.Count; k++)
                            {
                                if (activeNodes[k].code == nextCodes[j])
                                {
                                    a.list[j].outPort.ConnectTo(((ResponseNode)activeNodes[k]).inPort);
                                    foreach (Edge item in a.list[j].outPort.connections)
                                    {
                                        graph.AddElement(item);
                                    }
                                }
                            }
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        AutoSort();
    }

    private void UpdateAsset()
    {
        List<NodeValues> values = new();

        List<BaseNode> q = new();

        NodeValues startValue = startNode.GetValues();

        if (startNode.GetPorts().Count > 0)
        {
            BaseNode startNextNode = (BaseNode)startNode.GetPorts()[0].node;

            q.Add(startNextNode);
            startValue.nextNodeCode.Add(startNextNode.code);
        }

        values.Add(startValue);

        do
        {
            if (q.Count > 0)
            {
                List<BaseNode> nextQ = new();

                for (int i = 0; i < q.Count; i++)
                {
                    NodeValues value = q[i].GetValues();
                    List<Port> ports = q[i].GetPorts();

                    switch (value.type)
                    {
                        case NodeType.Answer:
                            if (ports.Count > 0)
                            {
                                for (int ii = 0; ii < ports.Count; ii++)
                                {
                                    BaseNode nextNode = (BaseNode)ports[ii].node;

                                    value.nextNodeCode.Add(nextNode.code);

                                    if (!nextQ.Contains(nextNode)) nextQ.Add(nextNode);
                                }
                            }

                            break;

                        case NodeType.Response:
                            if (ports.Count > 0)
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

        DialogeInfo asset = (DialogeInfo)loadNodeDialogeInfo.value;

        Selection.activeObject = asset;

        asset.values = values;
    }

    private void CreateANode()
    {
        AnswerNode a = new(graph);
        a.code = nodeAmount;
        nodeAmount++;
        a.SetPosition(new Rect(0, 0, 150, 200));
        activeNodes.Add(a);
        graph.AddElement(a);
    }

    private void CreateRNode()
    {
        ResponseNode r = new(graph);
        
        r.code = nodeAmount;
        nodeAmount++;
        
        r.SetPosition(new Rect(0, 0, 150, 200));

        activeNodes.Add(r);
        graph.AddElement(r);
    }

    private void AutoSort()
    {

    }
}

[Serializable]
public class LoadInfo
{
}