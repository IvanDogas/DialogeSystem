using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogeManager : MonoBehaviour
{
    [SerializeField] private GameObject answerButton;
    [SerializeField] private Transform buttonParent;

    [SerializeField] private TextMeshProUGUI responseTMP;

    [SerializeField] private Button button;

    public void LoadInfo(NodeValues value,Dialoge dialoge)
    {
        if(value.type == NodeType.Answer)
        {
            for (int i = 0; i < value.texts.Count; i++)
            {
                Button buttonI = Instantiate(answerButton, buttonParent).GetComponent<Button>();

                buttonI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value.texts[i];
                
                DialogeButtonInfo info = new(i);
                buttonI.onClick.AddListener(() => PickOption(info.GetValue(),dialoge));
            }
        }
        else
        {
        }
    }

    private IEnumerator ShowText(string text)
    {
        string showText = null;

        for (int i = 0; i < text.Length; i++)
        {
            showText += text[i];
            responseTMP.text = showText;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void PickOption(int num,Dialoge dialoge)
    {
        dialoge.PickedAnswer(num);
    }
}

[Serializable]
public class DialogeButtonInfo
{
    private int value;

    public DialogeButtonInfo(int num)
    {
        value = num;
    }

    public int GetValue()
    {
        return value;
    }
}