using System.Collections;
using UnityEngine;
using TMPro;

namespace DialogueSystem
{
    public class NPCsDialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; protected set; }
        protected IEnumerator WriteText(string input, TMP_Text textHolder, float delay, float delayBetweenLines)
        {
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                yield return new WaitForSeconds(delay);
            }

            // yield return new WaitUntil(() => Input.GetKey(KeyCode.E));
            yield return new WaitForSeconds(delay);
            finished = true;
        }

    }
}

