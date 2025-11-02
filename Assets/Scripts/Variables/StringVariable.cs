using UnityEngine;

[CreateAssetMenu(menuName ="Variables/String")]
public class StringVariable : ScriptableObject
{
    public string Value;

    public void SetValue(string value)
    {
        Value = value;
    }
}
