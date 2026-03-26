using UnityEngine;

public class SetPos : MonoBehaviour
{
    public void SetPosFromTransform(Transform t)
    { 
        transform.position = t.position;
        transform.rotation = t.rotation;
    }
}
