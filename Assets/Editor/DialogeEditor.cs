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

        //values.Add(startNode.GetValues());

        //if(startNode.outPort.connected)
        //{
        //    q.Add((BaseNode)startNode.GetPorts()[0].node);

        //    do
        //    {
        //        if (q.Count > 0)
        //        {
        //            List<BaseNode> savedQ = q;

        //            for (int i = 0; i < q.Count; i++)
        //            {
        //                NodeValues value = q[i].GetValues();
        //                List<Port> ports = q[i].GetPorts();

        //                switch (value.type)
        //                {
        //                    case NodeType.Response:

        //                        values.Add(value);

        //                        if(ports.Count > 0)
        //                        {
        //                            value.nextNodeValues.Add(((BaseNode)ports[0].node).GetValues());
        //                            q.Add((BaseNode)ports[0].node);
        //                        }    
        //                        break;

        //                    case NodeType.Answer:
        //                        values.Add(value);
        //                        for (int ii = 0; ii < ports.Count; ii++)
        //                        {
        //                            if (ports[ii].connected)
        //                            {
        //                                value.nextNodeValues.Add(((BaseNode)ports[ii].node).GetValues());
        //                                q.Add((BaseNode)ports[ii].node);
        //                            }
        //                        }

        //                        break;

        //                    default:
        //                        break;
        //                }
        //            }

        //            for (int i = 0; i < savedQ.Count; i++)
        //            {
        //                q.Remove(savedQ[i]);
        //            }
        //        }
        //        else break;
        //    } while (true);
        //}

        //values.Add(startNode.GetValues());
        //if (startNode.outPort.connected)
        //{
        //    bool next;
        //    BaseNode currentNode = (BaseNode)startNode.GetPorts()[0].node;

        //    values[0].nextNodeValue = currentNode.GetValues();

        //    values.Add(currentNode.GetValues());

        //    do
        //    {
        //        next = false;

        //        NodeType type = values[values.Count - 1].type;

        //        List<Port> ports = new();
        //        BaseNode nextNode = null;

        //        ports = currentNode.GetPorts();

        //        if(ports.Count > 0)
        //        {
        //            next = true;

        //            switch (type)
        //            {
        //                case NodeType.Response:
        //                    currentNode = (BaseNode)ports[0].node;

        //                    values[values.Count - 1].nextNodeValue = currentNode.GetValues();

        //                    values.Add(currentNode.GetValues());

        //                    Debug.Log(currentNode.GetValues().type);
        //                    break;

        //                case NodeType.Answer:
        //                    int index = 0;

        //                    for (int i = 0; i < values.Count; i++)
        //                    {
        //                        if (values[i] == currentNode.GetValues())
        //                        {
        //                            index = i;
        //                            Debug.Log("BOK" + i);
        //                        }
        //                    }

        //                    for (int i = 0; i < ports.Count; i++)
        //                    {
        //                        currentNode = (BaseNode)ports[i].node;

        //                        values[index].nextNodeValues.Add(currentNode.GetValues());

        //                        values.Add(currentNode.GetValues());

        //                        Debug.Log(currentNode.GetValues().type);
        //                    }

        //                    break;

        //                default:
        //                    break;
        //            }
        //        }

        //    } while (next);
        //}

        //Debug.Log(graph.selection.Count);

        //for (int i = 0; i < graph.selection.Count; i++)
        //{
        //    if (graph.selection[i] is BaseNode)
        //    {
        //        BaseNode node = (BaseNode)graph.selection[i];
        //        Debug.Log(node.GetValues().type);
        //    }

        //}

        DialogeInfo asset = CreateInstance<DialogeInfo>();

        string name = AssetDatabase.GenerateUniqueAssetPath("Assets/NewScripableObject.asset");
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