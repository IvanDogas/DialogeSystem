using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Vector3")]
public class Vector3Variable : ScriptableObject
{
    public Vector3 Value;

    public void SetValue(Vector3 value)
    {
        Value = value;
    }
}