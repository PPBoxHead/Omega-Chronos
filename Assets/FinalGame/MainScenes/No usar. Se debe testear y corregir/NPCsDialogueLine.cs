using System.Collections;
using UnityEngine;
using TMPro;


namespace DialogueSystem
{
    public class NPCsDialogueLine : NPCsDialogueBaseClass
    {
        [SerializeField] private string input;
        private TMP_Text textHolder;
        [Space]
        [SerializeField, Range(0.01f, 0.8f)] float delay = 0.1f;
        [SerializeField, Range(1f, 3f)] private float delayBetweenLines = 1f;

        private IEnumerator lineAppear;

        void Awake()
        {
            textHolder = GetComponent<TMP_Text>();

        }

        private void Start()
        {
            lineAppear = WriteText(input, textHolder, delay, delayBetweenLines);
            StartCoroutine(lineAppear);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E)) // con esto haces que el dialogo pase rapidisimo
            {
                if(textHolder.text != input)
                {
                    StopCoroutine(lineAppear);
                    textHolder.text = input;
                }
                else 
                    finished = true;
            }
        }

    }
}

