using System.Collections.Generic;
using System.Linq;
using Utils.Extensions;

namespace Dialogues.Core
{
    public static class DialogueIteration
    {
        public static IEnumerable<DialogueLine> Iterate(this Dialogue dialogue)
        {
            var existsNextDialogue = TryGetRandomDialogueLine(dialogue.StartLines, out var nextLine);
            
            while (existsNextDialogue)
            {
                nextLine.ExecuteTrigger();
                yield return nextLine;
                var connections = dialogue.GetConnectedLines(nextLine);
                existsNextDialogue = TryGetRandomDialogueLine(connections, out nextLine);
            }
        }

        private static bool TryGetRandomDialogueLine(IList<DialogueLine> lines, out DialogueLine nextLine)
        {
            var evaluated = lines.Where(line => line.EvaluateCheck()).ToList();
            bool exists = evaluated.Count > 0;
            nextLine = exists ? evaluated.GetRandom() : null;
            return exists;
        }
    }
}