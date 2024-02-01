using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "New State Tree", menuName = "States/State Tree", order = 0)]
    public class StateTree : ScriptableObject, IStateTree
    {
        public int Hash(IState state)
        {
            var n = state.GetType().FullName + state.Name;
            var h = n.GetHashCode();
            while (_states.ContainsKey(h))
            {
                h++;
            }

            return h;
        }

        void IStateTree.AddState(IState state)
        {
            var h = Hash(state);
            state.Id = h;
            _states.Add(h, state);
        }

        bool IStateTree.TryConnect(int idA, int idB)
        {
            var th = this as IStateTree;
            var b = IsIdValid(idA) && IsIdValid(idB);

            if (!b) return false;
            th.GetState(idA).Next = th.GetState(idB);
            return true;
        }

        bool IStateTree.TryDisconnect(int idA, int idB)
        {
            var th = this as IStateTree;
            var b = IsIdValid(idA) && IsIdValid(idB);

            if (!b) return false;
            th.GetState(idA).Next = null;
            return true;
        }

        bool IStateTree.TryDisconnect(int id)
        {
            var th = this as IStateTree;
            var b = IsIdValid(id);

            if (!b) return false;
            foreach (var idA in th.FindConnections(id))
            {
                th.GetState(idA).Next = null;
            }

            return true;
        }

        int[] IStateTree.FindConnections(int id) =>
            (from value in _states.Values where value.Next.Id == id select value.Id).ToArray();

        public bool IsIdValid(int id) => _states.ContainsKey(id);

        bool IStateTree.TryRemoveState(int id)
        {
            if (!_states.ContainsKey(id)) return false;
            _states[id].Id = -1;
            _states.Remove(id);
            return true;
        }

        bool IStateTree.TryGetState(int id, ref IState state)
        {
            if (!_states.ContainsKey(id)) return false;
            state = _states[id];
            return true;
        }

        IState IStateTree.GetState(int id) => _states[id];

        private Dictionary<int, IState> _states;

        Dictionary<int, IState> IStateTree.States
        {
            get
            {
                if (_states != null) return _states;

                _states = new Dictionary<int, IState>();
                _states.Add(0,
                    AssetDatabase.LoadAssetAtPath<InitialState>(
                        AssetDatabase.GUIDToAssetPath(
                            AssetDatabase.FindAssets("t:InitialState")[0])));
                return _states;
            }
        }
    }
}