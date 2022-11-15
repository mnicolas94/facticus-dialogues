using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor;
using UnityEngine;
using Utils.Extensions;
using Utils.Serializables;

namespace Dialogues
{
    [Serializable]
    public class DialogueLinesConnectionsDictionary : SerializableDictionary<DialogueLine, List<DialogueLine>, DialogueConnectionsStorage>{}
    
    [Serializable]
    public class DialogueConnectionsStorage : SerializableDictionary.Storage<List<DialogueLine>>{}
    
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Facticus/Dialogues/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [ContextMenu("Iterate")]
        public void IterateTest()
        {
            foreach (var line in this.Iterate())
            {
                Debug.Log(line.LineData.ToString());
            }
        }
        
        
        [SerializeField] private List<DialogueLine> _startLines;
        [SerializeField] private List<DialogueLine> _lines;
        [SerializeField] private DialogueLinesConnectionsDictionary _linesConnections;

        public ReadOnlyCollection<DialogueLine> StartLines => _startLines.AsReadOnly();
        
        public ReadOnlyCollection<DialogueLine> Lines => _lines.AsReadOnly();

        public List<DialogueLine> GetConnectedLines(DialogueLine line)
        {
            var nextLines = new List<DialogueLine>();
            
            if (_linesConnections.ContainsKey(line))
            {
                nextLines.AddRange(_linesConnections[line]);
            }

            return nextLines;
        }
        
#if UNITY_EDITOR
        
        [SerializeField] private DialogueEditorData _editorData;

        public DialogueEditorData EditorData
        {
            get => _editorData;
            set => _editorData = value;
        }
        
        public void EditorAddStartLine(DialogueLine line)
        {
            _startLines.Add(line);
            
            // add line in case it hadn't been added yet
            EditorAddLine(line);
        }
        
        public void EditorAddLine(DialogueLine line)
        {
            if (!_lines.Contains(line))
            {
                _lines.Add(line);
                var index = _lines.Count;
                line.name = $"{name}_line-{index}";
                AssetDatabase.AddObjectToAsset(line, this);
                EditorUtility.SetDirty(this);
            }
        }
        
        public void EditorRemoveLine(DialogueLine line)
        {
            if (_lines.Contains(line))
            {
                _lines.Remove(line);
                _linesConnections.Remove(line);
                AssetDatabase.RemoveObjectFromAsset(line);
                EditorUtility.SetDirty(this);
            }
        }

        public void EditorClearLines()
        {
            _startLines.Clear();
            
            var lines = new List<DialogueLine>(_lines);
            foreach (var line in lines)
            {
                EditorRemoveLine(line);
            }
        }
        
        public void EditorAddLineConnections(DialogueLine line, List<DialogueLine> connections)
        {
            if (connections.Count == 0)
            {
                return;
            }
            
            // add lines if haven't been added yet
            EditorAddLine(line);
            
            connections.ForEach(connection =>
            {
                EditorAddLine(line);
            });

            // add connections
            if (!_linesConnections.ContainsKey(line))
            {
                _linesConnections.Add(line, new List<DialogueLine>());
            }
            
            _linesConnections[line].AddRangeIfNotExists(connections);
        }

        public void EditorClearConnections()
        {
            _linesConnections.Clear();
        }
#endif
    }
}