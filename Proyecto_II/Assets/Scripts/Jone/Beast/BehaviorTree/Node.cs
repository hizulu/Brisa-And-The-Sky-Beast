using System;
using System.Collections;
using System.Collections.Generic;

// Jone Sainz Egea
// 04/04/2025
namespace BehaviorTree
{
    public enum NodeState {RUNNING, SUCCESS, FAILURE}   

    public class Node
    {
        public readonly string name; // Para facilitar la comprensión

        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        // Diccionario que guardará todas las variables compartidas del árbol
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node(string name)
        {
            this.name = name;
            parent = null;
        }

        public Node(string name, List<Node> children)
        {
            this.name = name;
            foreach (Node child in children)
                _Attach(child);
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        // Añadir datos al diccionario
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        // Método recursivo que lee datos del diccionario
        // Hay que buscar la key en todo el árbol, no solo en el nodo
        public object GetData(string key)
        {
            Node currentNode = this;
            while (currentNode != null)
            {
                if (currentNode._dataContext.TryGetValue(key, out var value))
                    return value;
                currentNode = currentNode.parent;
            }
            return null; // No se ha encontrado la clave
        }

        // Método recursivo que elimina datos del diccionario
        public bool ClearData(string key)
        {
            Node currentNode = this;
            while (currentNode != null)
            {
                if (currentNode._dataContext.ContainsKey(key))
                {
                    currentNode._dataContext.Remove(key);
                    return true;
                }
                currentNode = currentNode.parent;
            }
            return false; // No se ha encontrado la clave
        }
    }

    public class Leaf : Node
    {
        public Leaf(string name) : base(name)
        {
        }
    }
}


