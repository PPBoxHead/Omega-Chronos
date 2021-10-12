using System.Collections;
using UnityEngine;
using TMPro;


namespace DialogueSystem
{
    public class IntroDialogueLine : IntroDialogueBaseClass
    {
        [SerializeField] private string input;
        private TMP_Text textHolder;
        [Space]
        [SerializeField, Range(0.01f, 0.8f)] float delay = 0.1f;
        [SerializeField, Range(1f, 3f)] private float delayBetweenLines = 1f;

        void Awake()
        {
            textHolder = GetComponent<TMP_Text>();

        }

        private void Start()
        {
            StartCoroutine(WriteText(input, textHolder, delay, delayBetweenLines));
        }

    }
}
