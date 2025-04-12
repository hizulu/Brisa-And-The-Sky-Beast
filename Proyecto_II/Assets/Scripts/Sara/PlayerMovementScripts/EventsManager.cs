using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventsManager
{
    private static Dictionary<string, Action> normalEvents = new Dictionary<string, Action>();
    private static Dictionary<string, Delegate> specialEvents = new Dictionary<string, Delegate>();

    public static void CallNormalEvents(string nameEvent, Action _action)
    {
        if (normalEvents.TryGetValue(nameEvent, out Action action))
            action += _action;
        else
        {
            action = _action;
            normalEvents.Add(nameEvent, action);
        }
    }

    public static void CallSpecialEvents<T>(string nameEvent, Action<T> _action)
    {
        if (specialEvents.TryGetValue(nameEvent, out Delegate action))
            action = Delegate.Combine(action, _action);
        else
        {
            action = _action;
            specialEvents.Add(nameEvent, action);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        if (normalEvents.TryGetValue(eventName, out Action action))
        {
            action?.Invoke();
        }
    }

    public static void TriggerEvent<T>(string eventName, T eventData)
    {
        if (specialEvents.TryGetValue(eventName, out Delegate action))
        {
            ((Action<T>)action)?.Invoke(eventData);
        }
    }

    public static void StopCallNormalEvents(string eventName, Action _action)
    {
        if (normalEvents.TryGetValue(eventName, out Action action))
        {
            action -= _action;
        }
    }

    public static void StopCallSpecialEvents<T>(string eventName, Action<T> _action)
    {
        if (specialEvents.TryGetValue(eventName, out Delegate action))
        {
            if (action is Action<T> typedAction)
            {
                typedAction -= _action;
            }
        }
    }
}
