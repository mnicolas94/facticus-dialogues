using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dialogues.Checks;
using Dialogues.Triggers;
using UnityEditor;
using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "DialoguesDatabase", menuName = "Facticus/Dialogues/Database", order = 0)]
    public class DialoguesDatabase : ScriptableObject
    {
        [SerializeField] private List<Check> _checks = new List<Check>();
        [SerializeField] private List<Trigger> _triggers = new List<Trigger>();

        public ReadOnlyCollection<Check> Checks => _checks.AsReadOnly();

        public ReadOnlyCollection<Trigger> Triggers => _triggers.AsReadOnly();

#if UNITY_EDITOR
        private static readonly string CheckAssetPrefix = "check_";
        private static readonly string TriggerAssetPrefix = "trigger_";
        
        public Check CreateNewCheck(string checkName)
        {
            var check = CreateInstance<Check>();
            var assetName = $"{CheckAssetPrefix}{checkName}";
            check.name = assetName;
            _checks.Add(check);

            AddAsSubAsset(check);

            return check;
        }
        
        public Trigger CreateNewTrigger(string triggerName)
        {
            var trigger = CreateInstance<Trigger>();
            var assetName = $"{TriggerAssetPrefix}{triggerName}";
            trigger.name = assetName;
            _triggers.Add(trigger);
            
            AddAsSubAsset(trigger);

            return trigger;
        }

        private void AddAsSubAsset(ScriptableObject asset)
        {
            var databasePath = AssetDatabase.GetAssetPath(this);
            AssetDatabase.AddObjectToAsset(asset, databasePath);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        [ContextMenu("Clear database")]
        private void ClearDatabase()
        {
            ClearChecks();
            ClearTriggers();
        }

        private void ClearChecks()
        {
            var temp = new List<Check>(_checks);
            foreach (var check in temp)
            {
                RemoveCheck(check);
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        private void ClearTriggers()
        {
            var temp = new List<Trigger>(_triggers);
            foreach (var trigger in temp)
            {
                RemoveTrigger(trigger);
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        private void RemoveCheck(Check check)
        {
            _checks.Remove(check);
            AssetDatabase.RemoveObjectFromAsset(check);
        }
        
        private void RemoveTrigger(Trigger trigger)
        {
            _triggers.Remove(trigger);
            AssetDatabase.RemoveObjectFromAsset(trigger);
        }

        public static string GetCheckDisplayName(Check check)
        {
            return check.name.Replace(CheckAssetPrefix, "");
        }
        
        public static string GetTriggerDisplayName(Trigger trigger)
        {
            return trigger.name.Replace(TriggerAssetPrefix, "");
        }
#endif
    }
}