using System.Collections.Generic;
using Dialogues.Checks;
using Dialogues.Triggers;
using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "DialoguesDatabase", menuName = "Facticus/Dialogues/Database", order = 0)]
    public class DialoguesDatabase : ScriptableObject
    {
        private static readonly string CheckAssetPrefix = "check_";
        private static readonly string TriggerAssetPrefix = "trigger_";
        
        [SerializeField] private List<Check> _checks = new List<Check>();
        [SerializeField] private List<Trigger> _triggers = new List<Trigger>();

        public PropertyDatabaseHandler<Check> ChecksDatabase =>
            new PropertyDatabaseHandler<Check>(this, _checks, CheckAssetPrefix);
        
        public PropertyDatabaseHandler<Trigger> TriggersDatabase =>
            new PropertyDatabaseHandler<Trigger>(this, _triggers, TriggerAssetPrefix);

        [ContextMenu("Clear")]
        private void Clear()
        {
            ChecksDatabase.Clear();
            TriggersDatabase.Clear();
        }
    }
}