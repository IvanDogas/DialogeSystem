using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class InteractionBase : MonoBehaviour
{
    protected InteractionManger manager;

    protected void Awake() => manager = GameObject.Find("Managers").GetComponent<InteractionManger>();

    public abstract void SetUpInteraction();
    public abstract void Interaction();
    public abstract void UnSetUpInteraction();
}

public class InteractionManger : MonoBehaviour
{
    [SerializeField] private InputReader reader;

    private List<InteractionBase> _interactions = new List<InteractionBase>();
    private InteractionBase _currentInteraction;

    public void AddInteraction(InteractionBase interaction)
    {
        _interactions.Add(interaction);

        ManageInteractions();
    }

    public void RemoveInteraction(InteractionBase interaction)
    {
        if (!_interactions.Contains(interaction)) return;

        _interactions.Remove(interaction);

        if(_currentInteraction == interaction)
        {
            _currentInteraction.UnSetUpInteraction();
            _currentInteraction = null;

            reader.OnInteractEvent = null;
        }

        ManageInteractions();
    }

    private void ManageInteractions()
    {
        if(_currentInteraction == null && _interactions.Count > 0)
        {
            _currentInteraction = _interactions[0];
            _currentInteraction.SetUpInteraction();

            reader.OnInteractEvent = _currentInteraction.Interaction;
        }
    }
}