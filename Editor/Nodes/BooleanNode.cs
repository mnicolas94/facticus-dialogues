using Dialogues.Checks;
using Dialogues.Editor.Ports;
using Dialogues.Editor.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dialogues.Editor.Nodes
{
    public class BooleanNode : Node
    {
        private BinaryOperation _booleanOperation;
        private Port _inA;
        private Port _inB;
        private Port _out;
        private VisualElement _inputPortsContainer;

        public BooleanNode(BinaryOperation booleanOperation)
        {
            _booleanOperation = booleanOperation;
            
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
    }
}