using System;
using System.Collections.Generic;
using System.Linq;
using Entity.States.StateObjects;
using UnityEditor;
using UnityEngine;

namespace Entity.States
{
    [CreateAssetMenu(fileName = "New State Tree", menuName = "AI Nodes/Tree", order = 0)]
    public class StateTree :
        ScriptableObject,
        IGlobalParameterNodeStateTree<State, Tuple<Vector2>>,
        IUpdatableAssetStateTree<State>,
        IStateTreeWithEdits,
        IZoomableStateTree<State>
    {
        [SerializeField] private List<StateForList> states;
        [SerializeField] private List<EditForList> edits;
        [SerializeField] private bool unsaved;

        [field: SerializeField] public Vector3 Position { get; set; } = new(13, 221, 0);
        [field: SerializeField] public Vector3 Scale { get; set; } = new(0.8695652f, 0.8695652f, 1);
        [field: SerializeField] public Quaternion Rotation { get; set; }

        [Serializable]
        public struct StateForList
        {
            public string id;
            public Vector2 position;
            public State state;
            public List<string> nexts;
        }

        [Serializable]
        public struct EditForList
        {
            public string id;
            [SerializeReference] public ScriptableObject edit;
        }

        private void Reset()
        {
#if UNITY_EDITOR
            states = new List<StateForList>();
            edits = new List<EditForList>();
            if (AssetDatabase.FindAssets($"t:{nameof(InitialState)}").Length == 0)
                CreateMissingStateObject(typeof(InitialState));

            AddState(typeof(InitialState));
            states[0] = new StateForList
            {
                id = "0",
                nexts = states[0].nexts,
                position = states[0].position,
                state = states[0].state
            };
            UpdateAsset();
#endif
        }

        public string Hash(State state)
        {
#if UNITY_EDITOR
            var h = GUID.Generate().ToString();
            while (IsIdValid(h))
            {
                h = GUID.Generate().ToString();
            }
#else
            var h = (state.GetType().FullName + state.Name).GetHashCode();
            while (IsIdValid(h.ToString()))
            {
                h++;
            }
#endif
            return h.ToString();
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

        public string AddState(Type stateType)
        {
#if UNITY_EDITOR
            return AddState(
                AssetDatabase.LoadAssetAtPath(AssetDatabase
                        .FindAssets($"t:{stateType.Name}")
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .First(),
                    stateType
                ) as State
            );
#else
            return "-1";
#endif
        }

        public string AddState(State state)
        {
            var h = Hash(state);
            states.Add(new StateForList { id = h, state = state, nexts = new List<string>(0) });
            if (state is IEditableState editableState)
            {
                edits.Add(new EditForList { id = h, edit = CreateEdit(editableState.GetTypeOfEdit(), h, state) });
            }

            Unsaved = true;
            UpdateAsset();
            return h;
        }

        private ScriptableObject CreateEdit(Type type, string id, State state)
        {
#if UNITY_EDITOR
            var edit = CreateInstance(type);
            edit.name = $"Properies({state.Name.Replace(" ", "")}.{type.Name}) of State({id}) of Tree(\"{name}\")";
            AssetDatabase.AddObjectToAsset(edit, this);
            return edit;
#else
            return null;
#endif
        }

        public ScriptableObject CreateMissingStateObject(Type type)
        {
#if UNITY_EDITOR
            var state = CreateInstance(type);
            state.name = $"New{type.Name}";
            AssetDatabase.CreateAsset(state, $"Assets/New{type.Name}.asset");
            EditorUtility.SetDirty(state);
            UpdateAsset();
            return state;
#else
            return null;
#endif
        }

        public bool TryConnect(string idA, string idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;
            if (SflAtID(idA).Value.nexts.Contains(idB)) return false;

            SflAtID(idA).Value.nexts.Add(idB);

            Unsaved = true;
            return true;
        }

        public bool TryDisconnect(string idA, string idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;
            if (!SflAtID(idA).Value.nexts.Contains(idB)) return false;

            SflAtID(idA).Value.nexts.Remove(idB);

            Unsaved = true;
            return true;
        }

        public bool TryDisconnect(string id)
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

        public string[] FindConnections(string id)
        {
            var i = new List<string>();
            if (!IsIdValid(id)) return i.ToArray();

            foreach (var state in states)
            {
                if (Contains(state.nexts, id)) i.Add(state.id);
            }

            return i.ToArray();
        }

        public bool TryRemoveState(string id)
        {
            if (id == "0") return false;
            if (!IsIdValid(id)) return false;
            TryDisconnect(id);
            for (var i = 0; i < states.Count; i++)
            {
                if (states[i].id != id) continue;

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

        public bool IsIdValid(string id) => Contains(Ids, id);

        public bool TryGetState(string id, ref State state)
        {
            if (!IsIdValid(id)) return false;
            state = GetState(id);
            return true;
        }

        public State GetState(string id)
        {
            var a = SflAtID(id)?.state;
            if (!a) return null;
            return a;
        }

        public bool TryGetParameters(string id, ref Tuple<Vector2> parameters)
        {
            if (!IsIdValid(id)) return false;
            parameters = GetParameters(id);
            return true;
        }

        public Tuple<Vector2> GetParameters(string id)
        {
            var a = SflAtID(id);
            if (!a.HasValue) return new Tuple<Vector2>(Vector2.zero);
            return new Tuple<Vector2>(a.Value.position);
        }

        public bool TrySetParameters(string id, Tuple<Vector2> parameters)
        {
            if (!IsIdValid(id)) return false;
            for (var i = 0; i < states.Count; i++)
            {
                if (states[i].id != id) continue;
                states[i] = new StateForList
                {
                    id = states[i].id,
                    nexts = states[i].nexts,
                    position = parameters.Item1,
                    state = states[i].state
                };
                Unsaved = true;
                return true;
            }

            return false;
        }

        public bool Unsaved
        {
            get => unsaved;
            private set => unsaved = value;
        }

        public void UpdateAsset()
        {
#if UNITY_EDITOR

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(this);
            foreach (var edit in edits)
            {
                EditorUtility.SetDirty(edit.edit);
            }

            Unsaved = false;
            AssetDatabase.SaveAssets();
#endif
        }

        public string[] GetNextsTo(string id) => GetNextsIdsTo(id).ToArray();

        public bool IsNextsTo(string id) => IsIdValid(id) && SflAtID(id).Value.nexts.Count != 0;

        private IEnumerable<string> GetNextsIdsTo(string id)
        {
            if (!IsIdValid(id)) yield break;
            var inst = SflAtID(id).Value;

            foreach (var nextId in inst.nexts)
            {
                yield return nextId;
            }
        }

        public State First() => GetState("0");


        public IEnumerable<string> Ids
        {
            get
            {
                foreach (var state in states)
                {
                    yield return state.id;
                }
            }
        }

        public string IndToId(int ind) => ind > 0 && ind < states.Count ? states[ind].id : "0";

        public int IdToInd(string id)
        {
            if (!IsIdValid(id)) return -1;
            for (var i = 0; i < states.Count; i++)
            {
                if (id == states[i].id) return i;
            }

            return -1;
        }

        private StateForList? SflAtID(string id)
        {
            if (!IsIdValid(id)) return null;
            foreach (var state in states)
            {
                if (state.id == id) return state;
            }

            return null;
        }

        public Dictionary<string, State> Nodes => states.ToDictionary(item => item.id, item => item.state);


        public bool TryGetEdit(string id, ref EditableStateProperties edit)
        {
            State state = null;
            if (!TryGetState(id, ref state)) return false;
            if (state is not IEditableState) return false;
            edit = GetEdit(id);
            return true;
        }

        public EditableStateProperties GetEdit(string id)
        {
            foreach (var edit in edits)
            {
                if (edit.id == id) return edit.edit as EditableStateProperties;
            }

            throw new ArgumentException("Id is invalid");
        }
    }
}