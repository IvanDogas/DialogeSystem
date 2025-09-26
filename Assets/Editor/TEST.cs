using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TEST : EditorWindow
{
    [MenuItem("Window/UI Toolkit/TEST")]
    public static void ShowExample()
    {
        TEST wnd = GetWindow<TEST>();
        wnd.titleContent = new GUIContent("Node Inspector");
    }

    public VisualElement root;
    public BaseNode selectedNode;

    public void CreateGUI()
    {
        root = rootVisualElement;

        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);
    }
}