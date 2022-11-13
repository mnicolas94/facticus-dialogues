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
        [SerializeField] private List<SerializableEdge> _edges;

        public List<SerializedNodePosition> Nodes => _nodes;

        public List<SerializableEdge> Edges => _edges;

        public DialogueEditorData() : this(new List<SerializedNodePosition>(), new List<SerializableEdge>())
        {
        }
        
        public DialogueEditorData(List<SerializedNodePosition> nodes, List<SerializableEdge> serializableEdges)
        {
            _nodes = nodes;
            _edges = serializableEdges;
        }
    }

    public interface ISerializableNode
    {
        Node Deserialize();
        
        /// <summary>
        /// Get the nodes ports. This list should be created deterministically in order to serialize edges based
        /// on indices.
        /// </summary>
        /// <returns></returns>
        List<Port> GetPorts();
    }

    [Serializable]
    public class SerializableEdge
    {
        public int inputNodeIndex;
        public int inputPortIndex;
        public int outputNodeIndex;
        public int outputPortIndex;
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