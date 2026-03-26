using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickUp : InteractionBase
{
    [SerializeField] private UnityEvent OnPickUpEvent;

    [SerializeField] private TextMeshProUGUI text;

    public override void Interaction()
    {
        OnPickUpEvent?.Invoke();
    }

    public override void SetUpInteraction()
    {
        text.text = "Press E to pick up";
    }

    public override void UnSetUpInteraction()
    {
        text.text = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        if (other.CompareTag("Player"))
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
