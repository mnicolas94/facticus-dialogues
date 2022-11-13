using System;
using Dialogues.Checks;
using Dialogues.Editor.DialogueGraph.Ports;
using Dialogues.Editor.DialogueGraph.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogues.Editor.DialogueGraph.Nodes
{
    [Serializable]
    public sealed class BooleanNode : Node, ISerializableNode
    {
        [SerializeField] private BinaryOperation _booleanOperation;
        private Port _inA;
        private Port _inB;
        private Port _out;
        private VisualElement _inputPortsContainer;

        public BooleanNode(BinaryOperation booleanOperation)
        {
            _booleanOperation = booleanOperation;

            title = booleanOperation.ToString();
            this.AddStyleSheet("Styles/Nodes/BooleanNode");
            this.RemoveCollapsibleButton();
            
            _inA = PortsUtils.CreateCheckPort("", Direction.Input, Port.Capacity.Single);
            _inB = PortsUtils.CreateCheckPort("", Direction.Input, Port.Capacity.Single);
            _out = PortsUtils.CreateCheckPort("", Direction.Output, Port.Capacity.Multi);

            _inputPortsContainer = new VisualElement {name = "input-ports-container"};
            _inputPortsContainer.Add(_inA);
            _inputPortsContainer.Add(_inB);
            titleButtonContainer.Add(_out);
            titleContainer.Insert(0, _inputPortsContainer);

            RefreshExpandedState();
            RefreshPorts();
        }

        public Node Deserialize()
        {
            return new BooleanNode(_booleanOperation);
        }
    }
}