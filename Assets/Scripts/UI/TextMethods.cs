using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextMethods : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake() => text = GetComponent<TextMeshProUGUI>();

    public void SetText(string s) => text.text = s;

    public void SetStringValueToText(StringVariable s) => text.text = s.Value;
    public void SetIntValueToText(IntVariable i) => text.text = i.Value.ToString();
    public void SetFloatValueToText(FloatVariable f) => text.text = f.Value.ToString();
    public void SetOnOffFromBoolValueToText(BoolVariable b) => text.text = b.Value ? "On" : "Off";
    public void SetSliderValueToText(Slider s) => text.text = s.value.ToString();
    
    public void AddText(string s) => text.text += s;
    public void AddStringValue(StringVariable s) => text.text += s.Value;
    public void AddIntValue(IntVariable i) => text.text += i.Value.ToString();
    public void AddFloatValue(FloatVariable f) => text.text += f.Value.ToString(); 
}