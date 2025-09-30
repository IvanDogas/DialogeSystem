using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialoge : MonoBehaviour
{
    [SerializeField] private int num;
    private int lastNum;
    [SerializeField] List<UnityEvent> events = new();

    private void OnValidate()
    {
        if (num == 0)
        {
            lastNum = 0;

            for (int i = events.Count - 1; i >= 0; i--)
            {
                events.RemoveAt(i);
            }
        }
        else if (lastNum != num || num != events.Count)
        {
            lastNum = num;

            List<UnityEvent> list = events;

            for (int i = events.Count - 1; i >= 0; i--)
            {
                events.RemoveAt(i);
            }

            for (int i = 0; i < num; i++)
            {
                events.Add(new UnityEvent());
            }

            //for (int i = 0; i < list.Count; i++)
            //{
            //    if (i < events.Count)
            //    {
            //        events[i] = list[i];
            //    }
            //}
        }
    }
}
