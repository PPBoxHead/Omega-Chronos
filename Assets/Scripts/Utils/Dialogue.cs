using UnityEngine;
using System.Collections;
using TMPro;

public class Dialogue : MonoBehaviour
{
    // shows dialogue in game
    #region Variables
    #region Settings
    [Header("Settings")]
    [Range(0.01f, 0.8f), SerializeField] private float delay = 0.05f;
    [Range(1, 10), SerializeField] private float timeBetweenDialogues = 2;
    private AudioManager audioManager;
    private bool finished = false;
    #endregion
    #region DialoguePosition
    [Header("DialoguePosition")]
    [SerializeField] private DialogueHold[] dialogueHolds;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;
    #endregion
    #endregion

    #region Methods
    #region Settings
    private void Start()
    {
        dialogueBox.SetActive(false);
        //Referencia este "Cachea" el audiomanager
        audioManager = AudioManager.Getinstance;

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
        dialogueBox.SetActive(true);
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

        dialogueBox.SetActive(false);
    }

    IEnumerator WriteText(TextAsset textAsset)
    {
        dialogueText.text = ""; // cleans dialogue

        // write dialogue
        for (int i = 0; i < textAsset.ToString().Length; i++)
        {
            dialogueText.text += textAsset.ToString()[i];
            //Referencia este lo reproduce
            audioManager.PlaySFX(AudioManager.SFX.Dialogue);
            yield return new WaitForSeconds(delay);
        }

        finished = true;
    }

    public void StopText()
    {
        dialogueBox.SetActive(false);
        StopAllCoroutines();
    }
    #endregion
    #endregion
}
