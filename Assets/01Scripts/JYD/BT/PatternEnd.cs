using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/PatternEnd")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "PatternEnd", message: "PatternEnd", category: "Events", id: "6b3b20c979f3151f10b9fe133ae4002e")]
public partial class PatternEnd : EventChannelBase
{
    public delegate void PatternEndEventHandler();
    public event PatternEndEventHandler Event; 

    public void SendEventMessage()
    {
        Event?.Invoke();
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        Event?.Invoke();
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        PatternEndEventHandler del = () =>
        {
            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as PatternEndEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as PatternEndEventHandler;
    }
}

