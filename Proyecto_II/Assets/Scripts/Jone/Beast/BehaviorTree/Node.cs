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
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        // Diccionario que guarda todas las variables compartidas del árbol
        //private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
                _Attach(child);
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        // TODO: borrar esto
        // Estos son unos métodos previos a la implementación de la blackboard
        /*
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
        }*/
    }
}


