using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogeManager : MonoBehaviour
{
    private NodeValues currentValues;
    private Dialoge currentDialoge;

    [SerializeField] private Camera dialogeCamera;

    [SerializeField] private GameObject answerButton;
    [SerializeField] private Transform buttonParent;

    [SerializeField] private TextMeshProUGUI responseTMP;
    private int currentResponseIndex;
    private bool canSkipToNextResponse;

    [SerializeField] private UnityEvent OnResponseBegin,OnResponseEnd,OnAnswerBegin,OnAnswerEnd;

    private void Awake()
    {
        dialogeCamera.gameObject.SetActive(false);
    }

    public void LoadInfo(NodeValues value,Dialoge dialoge)
    {
        if(value.type == NodeType.Answer)
        {
            OnAnswerBegin?.Invoke();

            for (int i = 0; i < buttonParent.childCount; i++)
            {
                Destroy(buttonParent.GetChild(i).gameObject);
            }

            for (int i = 0; i < value.nextNodeCode.Count; i++)
            {
                Button buttonI = Instantiate(answerButton, buttonParent).GetComponent<Button>();

                buttonI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value.texts[i];
                
                DialogeButtonInfo info = new(i);
                buttonI.onClick.AddListener(() => PickOption(info.GetValue(),dialoge));
            }
        }
        else
        {
            currentValues = value;
            currentDialoge = dialoge;

            OnResponseBegin?.Invoke();

            StartCoroutine(ShowText(value.texts[0]));
            currentResponseIndex = 0;
            canSkipToNextResponse = true;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canSkipToNextResponse)
        {
            StopAllCoroutines();
            NextResponse();
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
        OnAnswerEnd?.Invoke();

        dialoge.PickedAnswer(num); 
    }

    private void NextResponse()
    {
        currentResponseIndex++;

        if(currentValues.texts.Count > currentResponseIndex)
        {
            StartCoroutine(ShowText(currentValues.texts[currentResponseIndex]));
        }
        else
        {
            EndOfResponses();
        }
    }

    private void EndOfResponses()
    {
        OnResponseEnd?.Invoke();

        canSkipToNextResponse = false;

        currentDialoge.LoadFromResponse(currentValues);
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