using System.Collections;
using System.Collections.Generic;

// Jone Sainz Egea
// 04/04/2025
namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        // Diccionario que guardar� todas las variables compartidas del �rbol
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

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

        // A�adir datos al diccionario
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        // M�todo recursivo que lee datos del diccionario
        // Hay que buscar la key en todo el �rbol, no solo en el nodo
        public object GetData(string key)
        {
            object value = null;

            // Se busca en el nodo actual
            if(_dataContext.TryGetValue(key, out value))
                return value;


            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }

            // Ha llegado a la ra�z del �rbol sin encontrar la key
            return null;
        }

        // M�todo recursivo que elimina datos del diccionario
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }

            return false;
        }
    }
}


