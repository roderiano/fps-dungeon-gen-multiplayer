using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour
{
    public EventTrigger.TriggerEvent interactionCallback;

    /// <summary>
    /// Interaction call
    /// </summary>
    /// <param name="eventData">Event data of interaction</param>
    public void Interact(BaseEventData eventData)
    {
        interactionCallback.Invoke(eventData);
    }
}
