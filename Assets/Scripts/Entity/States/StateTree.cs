using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "New State Tree", menuName = "States/State Tree", order = 0)]
    public class StateTree : ScriptableObject, IStateTree
    {
        [SerializeField] private List<StateForList> states;

        [Serializable]
        public struct StateForList
        {
            public int id;
            public State state;
            public List<int> nexts;
        }

        public int Hash(State state)
        {
            var n = state.GetType().FullName + state.Name;
            var h = n.GetHashCode();
            while (IsIdValid(h))
            {
                h++;
            }

            return h;
        }

        private static bool Contains<T>(IEnumerable<T> where, T that)
        {
            foreach (var it in where)
            {
                if (it.Equals(that)) return true;
            }

            return false;
        }

        private static bool Contains<T>(T[] where, T that) => Contains(where.AsEnumerable(), that);
        private static bool Contains<T>(List<T> where, T that) => Contains(where.AsEnumerable(), that);

        public void AddState(State state)
        {
            var h = Hash(state);
            state.Id = h;
            states.Add(new StateForList {id = h, state = state, nexts = new List<int>(0)});
        }

        public bool TryConnect(int idA, int idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;

            SflAtID(idA).Value.nexts.Add(idB);

            return true;
        }

        public bool TryDisconnect(int idA, int idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;

            SflAtID(idA).Value.nexts.Remove(idB);

            return true;
        }

        public bool TryDisconnect(int id)
        {
            if (!IsIdValid(id)) return false;

            var i = FindConnections(id);
            foreach (var Id in i)
            {
                SflAtID(Id).Value.nexts.Remove(id);
            }

            return true;
        }

        public int[] FindConnections(int id)
        {
            var i = new List<int>();
            if (!IsIdValid(id)) return i.ToArray();

            foreach (var state in states)
            {
                if (Contains(state.nexts, id)) i.Add(state.id);
            }

            return i.ToArray();
        }

        public bool TryRemoveState(int id)
        {
            if (!IsIdValid(id)) return false;
            TryDisconnect(id);
            for (var i = 0; i < states.Count; i++)
            {
                if (states[i].id != id) continue;
                (states[i].state as State).Id = -1;
                states.RemoveAt(i);
                return true;
            }

            return false;
        }

        public bool IsIdValid(int id) => Contains(Ids, id);

        public bool TryGetState(int id, ref State state)
        {
            if (!IsIdValid(id)) return false;
            state = GetState(id);
            return true;
        }

        public State GetState(int id)
        {
            var a = SflAtID(id)?.state;
            if (!a) return null;
            a.Id = id;
            return a;
        }

        public State[] GetNextsTo(int id)
        {
            if (!IsIdValid(id)) return new State[0];
            var inst = SflAtID(id).Value;
            var i = new List<State>(inst.nexts.Count);

            foreach (var nextId in inst.nexts)
            {
                i.Add(GetState(nextId));
            }

            return i.ToArray();
        }

        public State First() => GetState(0);


        public IEnumerable<int> Ids
        {
            get
            {
                foreach (var state in states)
                {
                    yield return state.id;
                }
            }
        }

        public int IndToId(int ind) => ind > 0 && ind < states.Count ? states[ind].id : -1;

        public int IdToInd(int id)
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
            foreach (var state in states)
            {
                if (state.id == id) return state;
            }

            return null;
        }

        public Dictionary<int, State> States => states.ToDictionary(item => item.id, item => item.state);
    }
}