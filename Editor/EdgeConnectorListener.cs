using System.Collections.Generic;
using Dialogues.Editor.SearchWindow;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialogues.Editor
{
    public class EdgeConnectorListener : IEdgeConnectorListener
    {
        private DialogueGraphEditor _editorView;
        private CreateNodeSearchWindowProvider _searchWindowProvider;
        private GraphViewChange _graphViewChange;
        private List<Edge> _edgesToCreate;
        private List<GraphElement> _edgesToDelete;

        private GraphViewChange m_GraphViewChange;
        private List<Edge> m_EdgesToCreate;
        private List<GraphElement> m_EdgesToDelete;

        public EdgeConnectorListener()
        {
            this.m_EdgesToCreate = new List<Edge>();
            this.m_EdgesToDelete = new List<GraphElement>();
            this.m_GraphViewChange.edgesToCreate = this.m_EdgesToCreate;
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
        }

        public void OnDrop(UnityEditor.Experimental.GraphView.GraphView graphView, Edge edge)
        {
            this.m_EdgesToCreate.Clear();
            this.m_EdgesToCreate.Add(edge);
            this.m_EdgesToDelete.Clear();
            if (edge.input.capacity == Port.Capacity.Single)
            {
                foreach (Edge connection in edge.input.connections)
                {
                    if (connection != edge)
                        this.m_EdgesToDelete.Add((GraphElement) connection);
                }
            }
            if (edge.output.capacity == Port.Capacity.Single)
            {
                foreach (Edge connection in edge.output.connections)
                {
                    if (connection != edge)
                        this.m_EdgesToDelete.Add((GraphElement) connection);
                }
            }
            if (this.m_EdgesToDelete.Count > 0)
                graphView.DeleteElements((IEnumerable<GraphElement>) this.m_EdgesToDelete);
            List<Edge> edgesToCreate = this.m_EdgesToCreate;
            if (graphView.graphViewChanged != null)
                edgesToCreate = graphView.graphViewChanged(this.m_GraphViewChange).edgesToCreate;
            foreach (Edge edge1 in edgesToCreate)
            {
                graphView.AddElement((GraphElement) edge1);
                edge.input.Connect(edge1);
                edge.output.Connect(edge1);
            }
        }
        
//        public EdgeConnectorListener(DialogueGraphEditor editorView, CreateNodeSearchWindowProvider searchWindowProvider)
//        {
////            this._editorView = editorView;
////            this._searchWindowProvider = searchWindowProvider;
////            _edgesToCreate = new List<Edge>();
////            _edgesToDelete = new List<GraphElement>();
////            _graphViewChange.edgesToCreate = _edgesToCreate;
//        }
//
//        public void OnDropOutsidePort(Edge edge, Vector2 position)
//        {
////            var port = edge.output?.edgeConnector.edgeDragHelper.draggedPort ?? edge.input?.edgeConnector.edgeDragHelper.draggedPort;
////            _searchWindowProvider.ConnectedPort = port;
////            _searchWindowProvider.RegenerateEntries = true;
////            SearcherWindow.Show(_editorView.EditorWindow, _searchWindowProvider.LoadSearchWindow(), item => _searchWindowProvider.OnSelectEntry(item, position), position, null);
////            _searchWindowProvider.RegenerateEntries = true;
//        }
//
//        public void OnDrop(GraphView graphView, Edge edge)
//        {
////            if(_editorView.DlogObject.DlogGraph.HasEdge(edge)) return;
////            _editorView.DlogObject.RegisterCompleteObjectUndo("Connect edge");
////            _editorView.DlogObject.DlogGraph.AddEdge(edge);
//        }
    }
}