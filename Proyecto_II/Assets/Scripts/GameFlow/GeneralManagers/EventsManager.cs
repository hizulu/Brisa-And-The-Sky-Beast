using System;
using System.Collections.Generic;

/*
 * NOMBRE CLASE: EventsManager
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 12/04/2025
 * DESCRIPCI�N: Clase est�tica para gestionar eventos normales y especiales mediante delegados. Permite suscribir, desencadenar y desuscribir eventos.
 * VERSI�N: 1.0
 *              1.1. 04/05/2025 - Jone Sainz Egea - Modificaciones para persistencia, asegurando limpieza de diccionarios
 */
public static class EventsManager
{
    private static Dictionary<string, Action> normalEvents = new Dictionary<string, Action>();
    private static Dictionary<string, Delegate> specialEvents = new Dictionary<string, Delegate>();

    /// <summary>
    /// M�todo que gestiona la l�gica para lanzar un evento normal.
    /// Comprueba si ha sido registrado previamente.
    /// </summary>
    /// <param name="eventName">Nombre del evento.</param>
    public static void TriggerNormalEvent(string eventName)
    {
        if (normalEvents.TryGetValue(eventName, out Action action))
            action?.Invoke();
    }

    /// <summary>
    /// M�todo que gestiona la l�gica para lanzar un evento especial, es decir, que requiera de al�n tipo de par�metro.
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
    /// M�todo que gestiona el registro de un evento normal.
    /// Si no existe, se a�ade la nueva acci�n.
    /// Si ya existe, lo a�ade al evento correspondiente.
    /// </summary>
    /// <param name="nameEvent">Nombre del evento.</param>
    /// <param name="_action">Acci�n que se ejecuta cuando se llame al evento.</param>
    public static void CallNormalEvents(string nameEvent, Action _action)
    {
        if (normalEvents.TryGetValue(nameEvent, out Action action))
            normalEvents[nameEvent] = action + _action;
        else
            normalEvents.Add(nameEvent, _action);
    }

    /// <summary>
    /// M�todo que gestiona el registro de un evento especial.
    /// Si no existe, se a�ade la nueva acci�n.
    /// Si ya existe, lo a�ade al evento correspondiente.
    /// </summary>
    /// <typeparam name="T">Tipo de dato requerido.</typeparam>
    /// <param name="nameEvent">Nombre del evento.</param>
    /// <param name="_action">Acci�n que se ejecuta cuando se llame al evento.</param>
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
    /// M�todo que gestiona la desuscripci�n de un evento nomal.
    /// </summary>
    /// <param name="eventName">Nombre del evento.</param>
    /// <param name="_action">Acci�n que se elimina del evento.</param>
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
    /// M�todo que gestiona la desuscripci�n de un evento especial.
    /// </summary>
    /// <typeparam name="T">Tipo de dato requerido.</typeparam>
    /// <param name="eventName">Nombre del evento.</param>
    /// <param name="_action">Acci�n que se elimina del evento.</param>
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
    /// M�todo que elimina todos los eventos registrados.
    /// </summary>
    public static void CleanAllEvents()
    {
        specialEvents.Clear();
        normalEvents.Clear();
    }
}
