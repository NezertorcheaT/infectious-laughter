using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "New State Tree", menuName = "States/State Tree", order = 0)]
    public class StateTree : ScriptableObject, IStateTree
    {
        [Serializable]
        public struct StateForList
        {
            public int id;
            public State state;
            public List<StateForList> nexts;
        }

        public int Hash(IState state)
        {
            var n = state.GetType().FullName + state.Name;
            var h = n.GetHashCode();
            while (Ids.Contains(h))
            {
                h++;
            }

            return h;
        }

        public void AddState(IState state)
        {
            var h = Hash(state);
            state.Id = h;
            states.Add(new StateForList {id = h, state = state as State, nexts = new List<StateForList>(0)});
        }

        public bool TryConnect(int idA, int idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;

            SflAtID(idA).Value.nexts.Add(SflAtID(idB).Value);

            return true;
        }

        public bool TryDisconnect(int idA, int idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;

            SflAtID(idA).Value.nexts.Remove(SflAtID(idB).Value);

            return true;
        }

        public bool TryDisconnect(int id)
        {
            if (!IsIdValid(id)) return false;

            var i = FindConnections(id);
            var th = SflAtID(id).Value;
            foreach (var Id in i)
            {
                SflAtID(Id).Value.nexts.Remove(th);
            }

            return true;
        }

        public int[] FindConnections(int id)
        {
            var i = new List<int>();
            if (!IsIdValid(id)) return i.ToArray();
            i.AddRange(from state in states
                where state.nexts.Select(item => item.id).Contains(id)
                select state.id);
            return i.ToArray();
        }

        public bool TryRemoveState(int id)
        {
            if (!Ids.Contains(id)) return false;
            TryDisconnect(id);
            for (var i = 0; i < states.Count; i++)
            {
                if (states[i].id != id) continue;
                (states[i].state as IState).Id = -1;
                states.RemoveAt(i);
                return true;
            }

            return false;
        }

        public bool IsIdValid(int id) => Ids.Contains(id);

        public bool TryGetState(int id, ref IState state)
        {
            if (!Ids.Contains(id)) return false;
            state = AtID(id);
            return true;
        }

        public IState GetState(int id) => AtID(id);

        public IState[] GetNextsTo(int id)
        {
            var i = new List<IState>(0);
            
            if (!IsIdValid(id)) return i.ToArray();
            
            var inst = SflAtID(id).Value;
            i.AddRange(inst.nexts.Select(next => next.state));
            return i.ToArray();
        }

        public IState First() => AtID(0);

        [SerializeField] private List<StateForList> states;
        private IEnumerable<int> Ids => states.Select(i => i.id);

        private int IdToInd(int id)
        {
            if (!IsIdValid(id)) return -1;
            for (var i = 0; i < states.Count; i++)
            {
                if (id == states[i].id) return i;
            }

            return -1;
        }

        private StateForList? SflAtID(int id)
        {
            if (!IsIdValid(id)) return null;
            for (var i = 0; i < states.Count; i++)
            {
                if (id == states[i].id) return states[i];
            }

            return null;
        }

        private State AtID(int id) =>
            states.Where(state => state.id == id).Select(state => state.state).FirstOrDefault();

        public Dictionary<int, IState> States => states.ToDictionary(item => item.id, item => (IState) item.state);
    }
}