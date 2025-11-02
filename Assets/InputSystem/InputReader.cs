using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = ("InputReader"))]
public class InputReader : ScriptableObject, InputActions.IMovementActions, InputActions.ICameraActions, InputActions.IInteractionActions
{
    private InputActions _actions;

    public Vector2 moveInput;

    public Vector2 lookInput;
    public UnityAction OnLookEvent;

    public UnityAction OnInteractEvent;

    private void OnEnable()
    {
        if (_actions == null)
        {
            _actions = new InputActions();

            _actions.Movement.SetCallbacks(this);
            _actions.Camera.SetCallbacks(this);
            _actions.Interaction.SetCallbacks(this);
        }

        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed) moveInput = context.ReadValue<Vector2>();
        else if (context.canceled) moveInput = Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lookInput = context.ReadValue<Vector2>();
            OnLookEvent?.Invoke();
        }
        else if (context.canceled) lookInput = Vector2.zero;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) OnInteractEvent?.Invoke();
    }
}