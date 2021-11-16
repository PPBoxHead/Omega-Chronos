using UnityEngine;
using System.Collections;
using TMPro;

public class Dialogue : MonoBehaviour
{
    // shows dialogue in game
    #region Variables
    #region Settings
    [Header("Settings")]
    [Range(0.01f, 0.8f), SerializeField] private float delay = 0.1f;
    [Range(1, 10), SerializeField] private float timeBetweenDialogues = 2;
    private bool finished = false;

    #endregion
    #region DialoguePosition
    [Header("DialoguePosition")]
    [SerializeField] private DialogueHold[] dialogueHolds;
    [SerializeField] private TMP_Text dialogueBox;
    #endregion
    #endregion

    #region Methods
    #region DelegateSettings
    private void Start()
    {
        foreach (DialogueHold dialogueHold in dialogueHolds)
        {
            dialogueHold.onShowDialogue += ShowText;
        }
    }
    private void OnDestroy()
    {
        foreach (DialogueHold dialogueHold in dialogueHolds)
        {
            dialogueHold.onShowDialogue -= ShowText;
        }
    }
    #endregion

    #region TextShowing
    void ShowText(TextAsset[] textAssets)
    {
        // reads between text files in gameobject
        StopAllCoroutines();
        StartCoroutine("ReadText", textAssets);
    }

    IEnumerator ReadText(TextAsset[] textAssets)
    {
        // passes 1 textasset and waits between those
        foreach (TextAsset textAsset in textAssets)
        {
            StartCoroutine("WriteText", textAsset);
            yield return new WaitUntil(() => finished);
            // cambiar luego, lo puse para que se pueda leer
            yield return new WaitForSeconds(timeBetweenDialogues);
            finished = false;
        }
    }

    IEnumerator WriteText(TextAsset textAsset)
    {
        dialogueBox.text = ""; // cleans dialogue

        // write dialogue
        for (int i = 0; i < textAsset.ToString().Length; i++)
        {
            dialogueBox.text += textAsset.ToString()[i];
            yield return new WaitForSeconds(delay);
        }

        finished = true;
    }
    #endregion
    #endregion
}
