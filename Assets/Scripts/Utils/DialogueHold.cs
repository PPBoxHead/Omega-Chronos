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

    // no los cachee porque me parece que se usan poco estos 
    // y se podria hacer de otra manera para no llamar 2 evento
    // del mismo script pero bueh.. era mas facil asi
    public void PlayTrack(int value)
    {
        AudioManager.Getinstance.FadeMusic((AudioManager.BackgroundMusic)value);
    }

    public void DialogueExit()
    {
        AudioManager.Getinstance.MusicSelector();
    }
}
