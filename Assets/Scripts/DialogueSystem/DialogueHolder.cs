using System.Collections;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        public bool npcDialogue = false; // si elegís esto, tenes que poner en el texto el NPCsDialogueLine
        public bool introDialogue = false; // si elegís esto, tenes que poner en el texto el IntroDialogueLine

        private IEnumerator dialogueSeq;
        // private bool dialogueFinished; //todo lo relacionado está comentado por causa de un problema que no supe solucionar xd

        private void Awake()
        {
            dialogueSeq = dialogueSequence();
            StartCoroutine(dialogueSeq);
        }

        private void Update()
        {
            if (npcDialogue == true)
            {
                Deactivate();
                gameObject.SetActive(false);
                StopCoroutine(dialogueSeq);
            }
            else
                StopCoroutine(dialogueSequence());
        }

        private IEnumerator dialogueSequence()
        {
            if (npcDialogue)
            {
                /*if(!dialogueFinished)
                {*/
                for (int i = 0; i < transform.childCount - 1; i++)
                {
                    Debug.Log("Pepe");
                    Deactivate();
                    transform.GetChild(i).gameObject.SetActive(true);
                    yield return new WaitUntil(() => transform.GetChild(i).GetComponent<NPCsDialogueLine>().finished);
                }
                // }
                /*else //esto tiene que ser testeado, es para que los npcs tengan varias opciones de dialogo
                {
                    int index = transform.childCount - 1;
                    Deactivate();
                    transform.GetChild(index).gameObject.SetActive(true);
                    yield return new WaitUntil(() => transform.GetChild(index).GetComponent<NPCsDialogueLine>().finished);
                }   */
                //dialogueFinished = true;
                gameObject.SetActive(false);

            }
            if (introDialogue)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Deactivate();
                    transform.GetChild(i).gameObject.SetActive(true);
                    yield return new WaitUntil(() => transform.GetChild(i).GetComponent<IntroDialogueLine>().finished);
                }
                gameObject.SetActive(false);
            }
        }

        private void Deactivate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}