using System;
using System.Collections.Generic;

/*
 * NOMBRE CLASE: EventsManager
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 12/04/2025
 * DESCRIPCIÓN: Clase estática para gestionar eventos normales y especiales mediante delegados. Permite suscribir, desencadenar y desuscribir eventos.
 * VERSIÓN: 1.0
 *              1.1. 04/05/2025 - Jone Sainz Egea - Modificaciones para persistencia, asegurando limpieza de diccionarios
 */
public static class EventsManager
{
    private static Dictionary<string, Action> normalEvents = new Dictionary<string, Action>();
    private static Dictionary<string, Delegate> specialEvents = new Dictionary<string, Delegate>();

    /// <summary>
    /// Método que gestiona la lógica para lanzar un evento normal.
    /// Comprueba si ha sido registrado previamente.
    /// </summary>
    /// <param name="eventName">Nombre del evento.</param>
    public static void TriggerNormalEvent(string eventName)
    {
        if (normalEvents.TryGetValue(eventName, out Action action))
            action?.Invoke();
    }

    /// <summary>
    /// Método que gestiona la lógica para lanzar un evento especial, es decir, que requiera de alún tipo de parámetro.
    /// Comprueba si ha sido registrado previamente.
    /// </summary>
    /// <typeparam name="T">Tipo de dato requerido.</typeparam>
    /// <param name="eventName">Nombre del evento.</param>
    /// <param name="eventData">Los datos que se le pasan al evento.</param>
    public static void TriggerSpecialEvent<T>(string eventName, T eventData)
    {
        if (specialEvents.TryGetValue(eventName, out Delegate action))
        {
            foreach (Delegate d in action.GetInvocationList())
            {
                if (d.Target == null) continue; 
                ((Action<T>)d)?.Invoke(eventData);
            }
        }
    }

    /// <summary>
    /// Método que gestiona el registro de un evento normal.
    /// Si no existe, se añade la nueva acción.
    /// Si ya existe, lo añade al evento correspondiente.
    /// </summary>
    /// <param name="nameEvent">Nombre del evento.</param>
    /// <param name="_action">Acción que se ejecuta cuando se llame al evento.</param>
    public static void CallNormalEvents(string nameEvent, Action _action)
    {
        if (normalEvents.TryGetValue(nameEvent, out Action action))
            normalEvents[nameEvent] = action + _action;
        else
            normalEvents.Add(nameEvent, _action);
    }

    /// <summary>
    /// Método que gestiona el registro de un evento especial.
    /// Si no existe, se añade la nueva acción.
    /// Si ya existe, lo añade al evento correspondiente.
    /// </summary>
    /// <typeparam name="T">Tipo de dato requerido.</typeparam>
    /// <param name="nameEvent">Nombre del evento.</param>
    /// <param name="_action">Acción que se ejecuta cuando se llame al evento.</param>
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

    /// <summary>
    /// Método que gestiona la desuscripción de un evento nomal.
    /// </summary>
    /// <param name="eventName">Nombre del evento.</param>
    /// <param name="_action">Acción que se elimina del evento.</param>
    public static void StopCallNormalEvents(string eventName, Action _action)
    {
        if (normalEvents.TryGetValue(eventName, out Action action))
        {
            action -= _action;

            if (action == null)
                normalEvents.Remove(eventName);
            else
                normalEvents[eventName] = action;
        }
    }

    /// <summary>
    /// Método que gestiona la desuscripción de un evento especial.
    /// </summary>
    /// <typeparam name="T">Tipo de dato requerido.</typeparam>
    /// <param name="eventName">Nombre del evento.</param>
    /// <param name="_action">Acción que se elimina del evento.</param>
    public static void StopCallSpecialEvents<T>(string eventName, Action<T> _action)
    {
        if (specialEvents.TryGetValue(eventName, out Delegate action))
        {
            if (action is Action<T> typedAction)
            {
                typedAction -= _action;

                if (typedAction == null)
                    specialEvents.Remove(eventName);
                else
                    specialEvents[eventName] = typedAction;
            }
        }
    }

    /// <summary>
    /// Método que elimina todos los eventos registrados.
    /// </summary>
    public static void CleanAllEvents()
    {
        specialEvents.Clear();
        normalEvents.Clear();
    }
}
