using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Dialoge : MonoBehaviour
{
    private DialogeManager manager;

    [SerializeField] private DialogeInfo info;
    private int currentCode;

    [SerializeField] List<UnityEvent> endingEvents = new();

    [SerializeField] private UnityEvent OnBeginEvent;
    [SerializeField] private UnityEvent OnEndEvent;

    //#if UNITY_EDITOR
    //private void OnValidate()
    //{
    //    if(info != null && addToEndEvents)
    //    {
    //        addToEndEvents = false;

    //        int amountOfOutcomes = 0;

    //        if(endingEvents.Count > 0)
    //        {
    //            for (int i = endingEvents.Count - 1; i >= 0; i--)
    //            {
    //                endingEvents.RemoveAt(i);
    //            }
    //        }

    //        for (int i = 0; i < info.values.Count; i++)
    //        {
    //            if (info.values[i].nextNodeCode.Count <= 0)
    //            {
    //                endingEvents.Add(new UnityEvent());
    //                amountOfOutcomes++;
    //            }
    //        }
    //        num = amountOfOutcomes;
    //    }

    //    if(endingEvents.Count != num)
    //    {
    //        addToEndEvents = true;
    //    }
    //}
    //#endif

    private void Start()
    {
        manager = GameObject.Find("Managers").GetComponent<DialogeManager>();
    }

    public void BeginDialoge()
    {
        currentCode = 0;
        LoadCurrentCode();

        OnBeginEvent?.Invoke();
    }

    public void LoadCurrentCode()
    {
        int index = 0;

        for(int i = 0;i < info.values.Count;i++)
        {
            if (info.values[i].code == currentCode)
            {
                index = i;
                break;
            }
        }

        manager.LoadInfo(info.values[index], this);
    }

    public void LoadFromResponse(NodeValues value)
    {
        int index = 0;

        for (int i = 0; i < info.values.Count; i++)
        {
            if (info.values[i].code == value.code)
            {
                index = i; 
                break;
            }
        }

        if(info.values[index].nextNodeCode.Count > 0)
        {
            currentCode = info.values[index].nextNodeCode[0];
            LoadCurrentCode();
        }
        else
        {
            EndDialoge();
        }
    }

    public void PickedAnswer(int num)
    {
        int currentIndex = 0;
        for (int i = 0; i < info.values.Count; i++)
        {
            if (info.values[i].code == currentCode) currentIndex = i;
        }

        currentCode = info.values[currentIndex].nextNodeCode[num];
        LoadCurrentCode();
    }

    public void EndDialoge()
    {
        int currentEnding = 0;

        for (int i = 0; i < info.values.Count; i++)
        {
            if(info.values[i].nextNodeCode.Count <= 0)
            {
                currentEnding++;

                if (info.values[i].code == currentCode)
                {
                    if (endingEvents.Count > currentEnding - 1 && endingEvents[currentEnding - 1] != null) endingEvents[currentEnding - 1]?.Invoke();
                }
            }
        }

        OnEndEvent?.Invoke();
    }

    public void ChangeDialogeInfo(DialogeInfo info)
    {
        this.info = info;
    }
}

[CustomEditor(typeof(Dialoge))]
public class DialogeEditor : Editor
{
    private SerializedProperty info;

    private SerializedProperty endingEvents;

    private int amountOfEndings;

    private void OnEnable()
    {
        info = serializedObject.FindProperty("info");
        endingEvents = serializedObject.FindProperty("endingEvents");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogeInfo info = (DialogeInfo)this.info.objectReferenceValue;
        amountOfEndings = 0;

        List<UnityEvent> endingEvents = null;

        for (int i = 0; i < info.values.Count; i++)
        {
            if (info.values[i].nextNodeCode.Count <= 0)
            {
                EditorGUILayout.LabelField($"Ending event index {amountOfEndings}: {info.values[i].texts[0]}");
                amountOfEndings++;
            }
        }
    }
}