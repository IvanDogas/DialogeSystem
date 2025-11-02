using UnityEngine;

public class PlayerCamera3D : MonoBehaviour
{
    [SerializeField] private InputReader reader;
    
    [SerializeField] private Transform player;

    [SerializeField] private FloatReference sensitivity;
    [SerializeField] private float minRot, maxRot;

    private float _pitch,_yaw;

    [SerializeField] private Vector3Variable currentPlayerCameraRotation;

    private void Start()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    private void Update()
    {
        transform.position = player.position;
    }

    private void OnEnable()
    {
        reader.OnLookEvent += Look;
        reader.OnLookEvent += SetCurrentPlayerCameraYRotation;
    }

    private void OnDisable()
    {
        reader.OnLookEvent -= Look;
        reader.OnLookEvent -= SetCurrentPlayerCameraYRotation;
    }

    private void Look()
    { 
        float inputX = reader.lookInput.x * sensitivity.value * .01f;
        float inputY = reader.lookInput.y * sensitivity.value * .01f;

        _pitch += -inputY;
        _pitch = Mathf.Clamp(_pitch, minRot, maxRot);
        _yaw += inputX;

        transform.rotation = Quaternion.Euler(_pitch, _yaw, transform.eulerAngles.z);
    }

    private void SetCurrentPlayerCameraYRotation()
    {
        currentPlayerCameraRotation.Value = transform.localEulerAngles;
    }
}
