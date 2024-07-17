using System;
using System.Collections.Generic;
using System.Linq;
using Entity.States;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Levels.StoryNodes
{
    [CreateAssetMenu(fileName = "New Story Tree", menuName = "Story Nodes/Tree", order = 0)]
    public class StoryTree :
        ScriptableObject,
        IUpdatableAssetStateTree<StoryTree.Node>,
        IGlobalParameterNodeStateTree<StoryTree.Node, Tuple<Vector2, Color, string, SceneAsset>>,
        ITwoPerConnectionStateTree<StoryTree.Node>,
        IZoomableStateTree<StoryTree.Node>
    {
        [SerializeField] private bool unsaved;
        [SerializeField] private List<NodeForList> nodes;

        [field: SerializeField] public Vector3 Position { get; set; } = new Vector3(13, 221, 0);
        [field: SerializeField] public Vector3 Scale { get; set; } = new Vector3(0.8695652f, 0.8695652f, 1);
        [field: SerializeField] public Quaternion Rotation { get; set; }

        [Serializable]
        public class NodeForList
        {
            public string id;
            public string visualName;
            public Color visualColor = new Color(63f / 256f, 63f / 256f, 63f / 256f, 192f / 256f);
            public Vector2 visualPosition;
            public SceneAsset scene;
            [CanBeNull] public string nextID1 = null;
            [CanBeNull] public string nextID2 = null;

            public override string ToString()
            {
                return $"({base.ToString()})" + '{' +
                       $" id: ({id}), visualName: ({visualName}), visualColor: ({visualColor}), visualPosition: ({visualPosition}), scene: ({scene}), nextID1: ({nextID1}), nextID2: ({nextID2}) " +
                       '}';
            }
        }

        public class Node
        {
            public string ID { get; private set; }
            public SceneAsset Scene { get; private set; }

            public static implicit operator Node(NodeForList node)
            {
                if (node is null) return null;
                var newNode = new Node
                {
                    ID = node.id,
                    Scene = node.scene
                };
                return newNode;
            }
        }

        private void Reset()
        {
#if UNITY_EDITOR
            nodes = new List<NodeForList>();
            nodes.Add(new NodeForList
            {
                id = "0",
                visualName = "Initial",
                visualPosition = Vector2.zero
            });
            UpdateAsset();
#endif
        }

        public string Hash(Node node)
        {
#if UNITY_EDITOR
            var h = GUID.Generate().ToString();
            while (IsIdValid(h))
            {
                h = GUID.Generate().ToString();
            }
#else
            var h = (node.GetType().FullName + node.visualName + node.nextID1 + node.nextID2 + node.id).GetHashCode();
            while (IsIdValid(h.ToString()))
            {
                h++;
            }
#endif
            return h.ToString();
        }

        public string AddState(Node node)
        {
#if UNITY_EDITOR
            var n = new NodeForList
            {
                id = Hash(node),
                scene = node.Scene
            };
            Unsaved = true;
            return n.id;
#else
            return "0";
#endif
        }

        public bool TryConnect(string idA, string idB)
        {
            throw new NotImplementedException(
                "ITwoPerConnectionStateTree<StoryTree.Node> is implemented, use this instead");
        }

        public bool TryDisconnect(string idA, string idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;
            var nodeA = GetListState(idA);
            var nodeB = GetListState(idB);

            if (nodeA.nextID1 == idB) nodeA.nextID1 = null;
            if (nodeA.nextID2 == idB) nodeA.nextID2 = null;
            Unsaved = true;

            return true;
        }

        public bool TryDisconnect(string id)
        {
            if (!IsIdValid(id)) return false;
            var node = GetListState(id);
            node.nextID1 = null;
            node.nextID2 = null;
            foreach (var connection in FindConnections(id))
            {
                var connectionNode = GetListState(connection);
                if (connectionNode.nextID1 == id) connectionNode.nextID1 = null;
                if (connectionNode.nextID2 == id) connectionNode.nextID2 = null;
            }

            Unsaved = true;

            return true;
        }

        public string[] FindConnections(string id) => nodes
            .Where(node => node.nextID1 == id || node.nextID2 == id)
            .Select(node => node.id)
            .ToArray();

        public bool TryRemoveState(string id)
        {
            if (!IsIdValid(id)) return false;
            nodes.RemoveAt(IdToInd(id));
            return true;
        }

        public bool IsIdValid(string id) => nodes.Any(a => a.id == id);

        public bool TryGetState(string id, ref Node node)
        {
            if (!IsIdValid(id)) return false;
            node = GetState(id);
            return true;
        }

        private NodeForList GetListState(string id) => nodes.FirstOrDefault(node => node.id == id);
        public Node GetState(string id) => nodes.FirstOrDefault(node => node.id == id);

        public string[] GetNextsTo(string id)
        {
            var node = GetListState(id);
            if (node is null) return new string[0];
            var list = new List<string>();
            if (node.nextID1 is not null) list.Add(node.nextID1);
            if (node.nextID2 is not null) list.Add(node.nextID2);
            return list.ToArray();
        }

        private int IdToInd(string id)
        {
            for (var i = 0; i < nodes.Count; i++)
                if (nodes[i].id == id)
                    return i;
            return -1;
        }

        public bool IsNextsTo(string id)
        {
            if (!IsIdValid(id)) return false;
            var node = GetListState(id);
            return node.nextID1 is not null || node.nextID2 is not null;
        }

        public Node First()
        {
            return nodes.First();
        }

        public ScriptableObject CreateMissingStateObject(Type type)
        {
            throw new NotImplementedException("пошёл наху");
        }

        public Dictionary<string, Node> Nodes => nodes.ToDictionary(item => item.id, item => (Node) item);

        public void UpdateAsset()
        {
#if UNITY_EDITOR
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(this);
            Unsaved = false;
            AssetDatabase.SaveAssets();
#endif
        }

        public bool Unsaved
        {
            get => unsaved;
            private set => unsaved = value;
        }

        public bool TryConnectToPort1(string idA, string idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;
            var nodeA = GetListState(idA);
            nodeA.nextID1 = idB;
            Unsaved = true;
            return true;
        }

        public bool TryConnectToPort2(string idA, string idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;
            var nodeA = GetListState(idA);
            nodeA.nextID2 = idB;
            Unsaved = true;
            return true;
        }

        public bool TryDisconnectAPort1(string idA, string idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;
            var nodeA = GetListState(idA);

            if (nodeA.nextID1 == string.Empty) return true;
            if (nodeA.nextID1 != idB) return false;
            nodeA.nextID1 = null;
            Unsaved = true;
            return true;
        }

        public bool TryDisconnectAPort2(string idA, string idB)
        {
            if (!IsIdValid(idA) && !IsIdValid(idB)) return false;
            var nodeA = GetListState(idA);

            if (nodeA.nextID2 == string.Empty) return true;
            if (nodeA.nextID2 != idB) return false;
            nodeA.nextID2 = null;
            Unsaved = true;
            return true;
        }

        public string GetPort1(string id)
        {
            if (!IsIdValid(id)) throw new ArgumentException("id does not exist");
            return GetListState(id).nextID1;
        }

        public string GetPort2(string id)
        {
            if (!IsIdValid(id)) throw new ArgumentException("id does not exist");
            return GetListState(id).nextID2;
        }

        public bool TrySetParameters(string id, Tuple<Vector2, Color, string, SceneAsset> parameters)
        {
            if (!IsIdValid(id)) return false;
            var node = GetListState(id);
            var (position, visualColor, visualName, scene) = parameters;
            node.visualPosition = position;
            node.visualColor = visualColor;
            node.visualName = visualName;
            node.scene = scene;
            Unsaved = true;
            return true;
        }

        public Tuple<Vector2, Color, string, SceneAsset> GetParameters(string id)
        {
            if (!IsIdValid(id)) throw new ArgumentException("id does not exist");
            var node = GetListState(id);
            return new Tuple<Vector2, Color, string, SceneAsset>(
                node.visualPosition,
                node.visualColor,
                node.visualName,
                node.scene
            );
        }

        public bool TryGetParameters(string id, ref Tuple<Vector2, Color, string, SceneAsset> parameters)
        {
            if (!IsIdValid(id)) return false;
            parameters = GetParameters(id);
            return true;
        }

        /// <summary>
        /// poop, dont use
        /// </summary>
        public static NodeForList NodeToListed(Node node, IStateTree<StoryTree.Node> tree)
        {
            if (tree is StoryTree storyTree) return storyTree.GetListState(node.ID);

            if (!(tree is IGlobalParameterNodeStateTree<Node, Tuple<Vector2, Color, string, SceneAsset>>
                globalParameterNodeStateTree))
                throw new ArgumentException(
                    "tree is not IGlobalParameterNodeStateTree<Node, Tuple<Vector2, Color, string, SceneAsset>>");

            var (position, color, visualName, sceneAsset) = globalParameterNodeStateTree?.GetParameters(node.ID);
            return new NodeForList
            {
                id = node.ID,
                nextID1 = tree.GetNextsTo(node.ID)[0],
                nextID2 = tree.GetNextsTo(node.ID)[1],
                scene = sceneAsset,
                visualPosition = position,
                visualColor = color,
                visualName = visualName,
            };
        }
    }
}