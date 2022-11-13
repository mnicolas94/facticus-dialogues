using System;
using System.Collections.Generic;
using System.Linq;
using TNRD;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialogues
{
#if UNITY_EDITOR
    /// <summary>
    /// Data needed to (re)construct a dialogue in the graph editor.
    /// </summary>
    [Serializable]
    public class DialogueEditorData
    {
        [SerializeField] private List<SerializedNodePosition> _nodes;

        public List<SerializedNodePosition> Nodes => _nodes;

        public DialogueEditorData() : this(new List<Node>())
        {
        }
        
        public DialogueEditorData(List<Node> nodes)
        {
            _nodes = nodes
                .Where(node => node is ISerializableNode)
                .Select(node => ((ISerializableNode) node, node.GetPosition().position))
                .Select(tuple =>
                {
                    var (node, position) = tuple;
                    var serializableInterface = new SerializableInterface<ISerializableNode> {Value = node};
                    var serializedNodePosition = new SerializedNodePosition
                    {
                        node = serializableInterface,
                        position = position
                    };
                    return serializedNodePosition;
                })
                .ToList();
        }
    }

    public interface ISerializableNode
    {
        Node Deserialize();
    }

    [Serializable]
    public class SerializedNodePosition
    {
        [SerializeField] public SerializableInterface<ISerializableNode> node;
        [SerializeField] public Vector2 position;

        public void Deconstruct(out SerializableInterface<ISerializableNode> node, out Vector2 position)
        {
            node = this.node;
            position = this.position;
        }
    }
    
#endif

}