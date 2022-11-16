using System.Collections.Generic;
using Dialogues.Core.Checks;
using Dialogues.Core.Triggers;
using UnityEngine;

namespace Dialogues.Core
{
    [CreateAssetMenu(fileName = "DialoguesDatabase", menuName = "Facticus/Dialogues/Database", order = 0)]
    public class DialoguesDatabase : ScriptableObject
    {
        private static readonly string CheckAssetPrefix = "check_";
        private static readonly string TriggerAssetPrefix = "trigger_";
        
        [SerializeField] private List<Check> _checks = new List<Check>();
        [SerializeField] private List<Trigger> _triggers = new List<Trigger>();

        private PropertyDatabaseHandler<Check> _checksDatabase;
        private PropertyDatabaseHandler<Trigger> _triggersDatabase;

        public PropertyDatabaseHandler<Check> ChecksDatabase
        {
            get
            {
                if (_checksDatabase == null)
                {
                    _checksDatabase = new PropertyDatabaseHandler<Check>(this, _checks, CheckAssetPrefix);
                }

                return _checksDatabase;
            }
        }
            
        
        public PropertyDatabaseHandler<Trigger> TriggersDatabase
        {
            get
            {
                if (_triggersDatabase == null)
                {
                    _triggersDatabase = new PropertyDatabaseHandler<Trigger>(this, _triggers, TriggerAssetPrefix);
                }

                return _triggersDatabase;
            }
        }

        [ContextMenu("Clear")]
        private void Clear()
        {
            ChecksDatabase.Clear();
            TriggersDatabase.Clear();
        }
    }
}