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
            states.Add(new StateForList {id = h, state = state as State});
        }

        public bool TryConnect(int idA, int idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;

            GetState(idA).Connect(AtID(idB));

            return true;
        }

        public bool TryDisconnect(int idA, int idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;

            GetState(idA).Disconnect(AtID(idB));

            return true;
        }

        public bool TryDisconnect(int id)
        {
            if (!IsIdValid(id)) return false;

            var i = FindConnections(id);
            foreach (var Id in i)
            {
                GetState(Id).Nexts.Remove(GetState(id));
            }

            return true;
        }

        public int[] FindConnections(int id)
        {
            var i = new List<int>();
            if (!IsIdValid(id)) return i.ToArray();
            i.AddRange(from state in states
                where (state.state as IState).Nexts.Select(item => item.Id).Contains(id)
                select state.id);
            return i.ToArray();
        }

        public bool TryRemoveState(int id)
        {
            if (!Ids.Contains(id)) return false;
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

        public IState First() => AtID(0);

        [SerializeField] private List<StateForList> states;
        private IEnumerable<int> Ids => states.Select(i => i.id);

        private State AtID(int id) =>
            states.Where(state => state.id == id).Select(state => state.state).FirstOrDefault();

        public Dictionary<int, IState> States => states.ToDictionary(item => item.id, item => (IState) item.state);
    }
}