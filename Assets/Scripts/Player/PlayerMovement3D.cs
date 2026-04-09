using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [SerializeField] private InputReader reader;

    private Rigidbody _rb;
    private Animator _anim;

    [SerializeField] private float speed;

    [Header("Rotation"), Space(5)]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3Variable currentPlayerCameraRotation;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rb.linearVelocity = (transform.forward * reader.moveInput.y + transform.right * reader.moveInput.x) * speed + transform.up * _rb.linearVelocity.y;

        if (reader.moveInput != Vector2.zero)
        {
            _anim.SetBool("IsMoving", true);
            SetYRotation();
        }
        else
        {
            _anim.SetBool("IsMoving", false);
        }
    }

    private void SetYRotation()
    {
        if (transform.localEulerAngles.y == currentPlayerCameraRotation.Value.y) return;

        float YRot = transform.localEulerAngles.y;

        bool positive1 = (currentPlayerCameraRotation.Value.y > YRot && (currentPlayerCameraRotation.Value.y - YRot) <= 180);
        bool positive2 = (YRot > currentPlayerCameraRotation.Value.y && ((360 - YRot) + currentPlayerCameraRotation.Value.y) <= 180);

        if (positive1 || positive2)
        {
            YRot += rotationSpeed * .01f;

            if (positive1)
            {
                if (currentPlayerCameraRotation.Value.y - YRot <= .05f)
                {
                    YRot = currentPlayerCameraRotation.Value.y;
                }
            }
            else
            {
                if (YRot > 360)
                {
                    YRot = 0;
                }

                if (currentPlayerCameraRotation.Value.y - YRot <= .05f && YRot < currentPlayerCameraRotation.Value.y)
                {
                    YRot = currentPlayerCameraRotation.Value.y;
                }
            }
        }
        else
        {
            YRot -= rotationSpeed * .01f;

            if (YRot < 0)
            {
                YRot = 359.9f;
            }

            if (YRot - currentPlayerCameraRotation.Value.y <= .05f && YRot > currentPlayerCameraRotation.Value.y)
            {
                YRot = currentPlayerCameraRotation.Value.y;
            }
        }

        transform.localEulerAngles = Vector3.up * YRot;
    }

    private void OnDisable()
    {
        reader.moveInput = Vector2.zero;
        _anim.SetBool("IsMoving", false);
    }
}
