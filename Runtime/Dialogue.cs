using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dialogues.Checks;
using Dialogues.Triggers;
using TNRD;
using UnityEditor;
using UnityEngine;
using Utils.Serializables;

namespace Dialogues
{
    [Serializable]
    public class DialogueLinesConnectionsDictionary : SerializableDictionary<DialogueLine, List<DialogueLine>>{}
    
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Facticus/Dialogues/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueLine> _lines;
        [SerializeField] private DialogueLinesConnectionsDictionary _linesConnections;

        public ReadOnlyCollection<DialogueLine> Lines => _lines.AsReadOnly();

        public List<DialogueLine> NextLines(DialogueLine line)
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
        
        public void EditorAddLine(DialogueLine line)
        {
            _lines.Add(line);
            AssetDatabase.AddObjectToAsset(line, this);
            EditorUtility.SetDirty(this);
        }
        
        public void EditorRemoveLine(DialogueLine line)
        {
            _lines.Remove(line);
            _linesConnections.Remove(line);
            AssetDatabase.RemoveObjectFromAsset(line);
            EditorUtility.SetDirty(this);
        }
        
        public void EditorAddLineConnections(DialogueLine line, List<DialogueLine> connections)
        {
            if (!_lines.Contains(line))
            {
                EditorAddLine(line);
            }
            
            connections.ForEach(connection =>
            {
                if (!_lines.Contains(line))
                {
                    EditorAddLine(line);
                }
            });

            if (!_linesConnections.ContainsKey(line))
            {
                // TODO
            }
        }
#endif
    }
}