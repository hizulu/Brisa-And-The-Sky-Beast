using System;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 06/04/2025
// Clase que almacena los datos accesibles por todos los nodos del árbol
public class Blackboard
{
    private Dictionary<string, object> _data = new Dictionary<string, object>();

    public void SetValue(string key, object value) => _data[key] = value;

    public T GetValue<T>(string key)
    {
        if (_data.TryGetValue(key, out var value))
            return (T)value;
        return default;
    }

    public bool HasKey(string key) => _data.ContainsKey(key);
    public void ClearKey(string key) => _data.Remove(key);
}

