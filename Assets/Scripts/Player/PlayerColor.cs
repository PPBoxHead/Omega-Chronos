using UnityEngine;
using System.Collections.Generic;

public class PlayerColor : MonoBehaviour
{
    [SerializeField] private List<Color> colorList = new List<Color>();
    private SpriteRenderer spriteRenderer;
    private int colorSelected;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorSelected = PlayerPrefs.GetInt("OCspriteColor", 0);

        // cambiar esta lista a lo que quieran
        colorList.Add(Color.white);
        colorList.Add(Color.blue);
        colorList.Add(Color.yellow);
        colorList.Add(Color.green);
        spriteRenderer.color = colorList[colorSelected];
    }

    public void NextColor()
    {
        colorSelected += 1;
        // loops array
        if (colorSelected >= colorList.Count)
        {
            colorSelected = 0;
        }
        ApplySkin();
    }

    public void PreviousColor()
    {
        colorSelected -= 1;
        // loops array
        if (colorSelected < 0)
        {
            colorSelected = colorList.Count - 1;
        }
        ApplySkin();
    }

    private void ApplySkin()
    {
        spriteRenderer.color = colorList[colorSelected];
        PlayerPrefs.SetInt("OCspriteColor", colorSelected);
    }

    public Color GetColor
    {
        get { return colorList[colorSelected]; }
    }
}
