using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEditor;
using UnityEditor.Playables;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Dialoge : MonoBehaviour
{
    private DialogeManager manager;

    [SerializeField] private DialogeInfo info;
    private int num;
    private int currentCode;
    private bool addToEndEvents = true;

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
            if (info.values[i] == value)
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
        currentCode = info.values[currentCode].nextNodeCode[num];
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
                    if (endingEvents[currentEnding - 1] != null) endingEvents[currentEnding - 1]?.Invoke();
                }
            }
        }

        OnEndEvent?.Invoke();
    }
}