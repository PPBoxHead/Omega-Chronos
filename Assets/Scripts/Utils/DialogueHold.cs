using UnityEngine;

public class DialogueHold : MonoBehaviour
{
    // passes text assets to Dialogue.cs
    public delegate void OnShowDialogue(TextAsset[] textAssets);
    public event OnShowDialogue onShowDialogue;
    public TextAsset[] textAssets;

    public void ShowText()
    {
        if (onShowDialogue != null)
        {
            onShowDialogue(textAssets);
        }
    }
}
