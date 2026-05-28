using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class DialogeStartInteraction : InteractionBase
{
    [SerializeField] private Dialoge dialoge;

    [SerializeField] private UnityEvent SetUpEvent,UnSetUpEvent;

    public override void Interaction()
    {
        dialoge.BeginDialoge();
        Debug.Log("Interaction");
    }

    public override void SetUpInteraction()
    {
        SetUpEvent?.Invoke();
    }

    public override void UnSetUpInteraction()
    {
        UnSetUpEvent?.Invoke();
    }

    private void OnDisable()
    {
        manager.RemoveInteraction(this);
    }

    private void Update()
    {
        if(dialoge.enabled == false)
        {
            manager.RemoveInteraction(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        if(other.CompareTag("Player"))
        {
            manager.AddInteraction(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled) return;

        if (other.CompareTag("Player"))
        {
            manager.RemoveInteraction(this);
        }
    }
}
