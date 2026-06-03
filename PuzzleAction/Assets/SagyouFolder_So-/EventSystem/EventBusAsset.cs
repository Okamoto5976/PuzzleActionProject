using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EventBusAsset", menuName = "Scriptable Objects/EventBusAsset")]
public class EventBusAsset : ScriptableObject
{
    public event UnityAction OnTrigger = delegate { };
    public void Trigger() => OnTrigger.Invoke();
}
