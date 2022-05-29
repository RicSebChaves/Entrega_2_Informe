using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryFriendsAgents
{
    public class CNode
    {
        private ArrayList _edges;
        public bool[] _actions;
        private CNode _parent;
        private State _state;

        public CNode()
        {
            this._edges = new ArrayList();
            this._parent = null;
            this._state = null;
            this._actions = new bool[5];
            for(int i = 0; i < 5; i++)
            {
                _actions[i] = false;
                if(i == 4)
                {
                    _actions[i] = true;
                }
            }
        }

        //Método para crear nodos segun un estado
        public CNode(State s)
        {
            this._edges = new ArrayList();
            this._parent = null;
            this._state = s;
            this._actions= new bool[5];
            for (int i = 0; i < 5; i++)
            {
                _actions[i] = false;
                if (i == 4)
                {
                    _actions[i] = true;
                }
            }
        }

        //Se hace un set a la accion que realizará el nodo
        public void setAction(int action)
        {
            _actions[action] = true;
        }

        //Se retorna que accion esta realizando el nodo.
        public bool isAction(int action)
        {
            return (bool)_actions[action];
        }

        //Se retorna el estado en el que se encuentra el nodo
        public State getState()
        {
            return this._state;
        }

        //Se retorna el valor de todas las acciones realizadas por el nodo
        public bool allActions()
        {
            foreach(bool b in _actions)
            {
                if (!b)
                {
                    return false;
                }
            }
            return true;
        }

        //Se retorna si el nodo se encuentra realizando una accion
        public bool isBusy()
        {
            if (this.allActions())
            {
                if(_edges.Count == 0)
                {
                    return true;
                }
            }
            return false;
        }
        
        public int getNrNodes()
        {
            return _edges.Count;
        }

        //Se retorna el nodo padre
        public CNode getParent()
        {
            return this._parent;
        }

        //Se asigna el nodo padre
        public void setParent(CNode p)
        {
            this._parent = p;
        }

        //Se agregan acciones al nodo hijo
        public void addEdge(CNode child, int action)
        {
            CEdge edge = new CEdge(this, child, action);
            edge.setBusy();
            this._edges.Add(edge);
            child.setParent(this);
        }

        //Se eliminan las acciones del nodo hijo
        public void removeEdge(CNode child)
        {
            foreach(CEdge ce in this._edges)
            {
                if (ce.getChild().Equals(child))
                {
                    this._edges.Remove(ce);
                    return;
                }
            }
        }

        //Se compara si las acciones del nodo actual coinicden con las del nodo padre
        public bool isConnected(CNode node)
        {
            foreach(CEdge ce in this._edges)
            {
                if (ce.getChild().Equals(node))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
