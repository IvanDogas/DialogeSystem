using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.Events;

public class Dialoge : MonoBehaviour
{
    private DialogeManager manager;

    [SerializeField] private DialogeInfo info;
    private int num;
    private int currentValue;

    [SerializeField] List<UnityEvent> events = new();

    [SerializeField] private UnityEvent OnBeginEvent;

    private void OnValidate()
    {
        if(info != null)
        {
            int amountOfOutcomes = 0;
            for (int i = 0; i < info.values.Count; i++)
            {
                if (info.values[i].nextNodeCode.Count <= 0)
                {
                    events.Add(new UnityEvent());
                    amountOfOutcomes++;
                }
            }
            num = amountOfOutcomes;
        }

        if(events.Count != num)
        {
            List<UnityEvent> list = events;

            for (int i = events.Count - 1; i >= 0; i--)
            {
                events.RemoveAt(i);
            }

            for (int i = 0; i < num; i++)
            {
                events.Add(new UnityEvent());
            }
        }
    }

    private void Start()
    {
        manager = GameObject.Find("Managers").GetComponent<DialogeManager>();
    }

    public void BeginDialoge()
    {
        currentValue = 0;
        //LoadCurrentValue();

        OnBeginEvent?.Invoke();
    }

    public void LoadCurrentValue()
    {
        manager.LoadInfo(info.values[currentValue], this);
    }

    public void PickedAnswer(int num)
    {
        currentValue = info.values[currentValue].nextNodeCode[num];
        LoadCurrentValue();
    }

    public void EndDialoge()
    {

    }
}