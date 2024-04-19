using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "New State Tree", menuName = "States/State Tree", order = 0)]
    public class StateTree : ScriptableObject, IPositionableStateTree, IUpdatableAssetStateTree, IStateTreeWithEdits
    {
        [SerializeField] private List<StateForList> states;
        [SerializeField] private List<EditForList> edits;

        [Serializable]
        public struct StateForList
        {
            public int id;
            public Vector2 position;
            public State state;
            public List<int> nexts;
            public Type Edit => (state as IEditableState)?.GetTypeOfEdit();
        }

        [Serializable]
        public struct EditForList
        {
            public int id;
            public IEditableState.Properties edit;
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

        public int AddState(State state)
        {
            var h = Hash(state);
            state.Id = h;
            states.Add(new StateForList {id = h, state = state, nexts = new List<int>(0)});
            if (state is IEditableState editableState)
            {
                var edit = CreateInstance(editableState.GetTypeOfEdit()) as IEditableState.Properties;
                AssetDatabase.AddObjectToAsset(edit, this);
                edits.Add(new EditForList {id = h, edit = edit});
            }

            Unsaved = true;
            UpdateAsset();
            return h;
        }

        public bool TryConnect(int idA, int idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;

            SflAtID(idA).Value.nexts.Add(idB);

            Unsaved = true;
            return true;
        }

        public bool TryDisconnect(int idA, int idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;

            SflAtID(idA).Value.nexts.Remove(idB);

            Unsaved = true;
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

            Unsaved = true;
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
                states[i].state.Id = -1;

                if (states[i].state is IEditableState)
                {
                    for (var j = 0; j < edits.Count; j++)
                    {
                        if (edits[j].id != id) continue;
                        DestroyImmediate(edits[j].edit, true);
                        edits.RemoveAt(j);
                        break;
                    }
                }

                states.RemoveAt(i);
                Unsaved = true;
                UpdateAsset();
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

        public bool TryGetPosition(int id, ref Vector2 position)
        {
            if (!IsIdValid(id)) return false;
            position = GetPosition(id);
            return true;
        }

        public State GetState(int id)
        {
            var a = SflAtID(id)?.state;
            if (!a) return null;
            a.Id = id;
            return a;
        }

        public Vector2 GetPosition(int id)
        {
            var a = SflAtID(id);
            if (!a.HasValue) return Vector2.zero;
            a.Value.state.Id = id;
            return a.Value.position;
        }

        public bool TrySetPosition(int id, Vector2 position)
        {
            if (!IsIdValid(id)) return false;
            for (var i = 0; i < states.Count; i++)
            {
                if (states[i].id != id) continue;
                states[i] = new StateForList
                {
                    id = states[i].id,
                    nexts = states[i].nexts,
                    position = position,
                    state = states[i].state
                };
                Unsaved = true;
                return true;
            }

            return false;
        }

        [SerializeField] private bool unsaved;

        public bool Unsaved
        {
            get => unsaved;
            private set => unsaved = value;
        }

        public void UpdateAsset()
        {
            Unsaved = false;
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
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


        public bool TryGetEdit(int id, ref IEditableState.Properties edit)
        {
            State state = null;
            if (!TryGetState(id, ref state)) return false;
            if (state is not IEditableState) return false;
            edit = GetEdit(id);
            return true;
        }

        public IEditableState.Properties GetEdit(int id)
        {
            foreach (var edit in edits)
            {
                if (edit.id == id) return edit.edit;
            }

            throw new ArgumentException("Id is invalid");
        }
    }
}