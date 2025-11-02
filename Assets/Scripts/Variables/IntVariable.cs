using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Int")]
public class IntVariable : ScriptableObject
{
    public int Value;

    public void SetValue(int value)
    {
        Value = value;
    }
}
