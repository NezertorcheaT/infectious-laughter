using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace NPC
{
    public class Dialogue : MonoBehaviour
    {
        [Serializable]
        public class Talk
        {
            [Serializable]
            public class Phrase
            {
                public string character;
                public Sprite characterIcon;
                [TextArea] public string text;
            }

            public bool monologue;
            [FormerlySerializedAs("_phrase")] public Phrase[] phrases;
        }


        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Image characterImage;

        [Space(10), SerializeField] private GameObject window;

        [Space(10), SerializeField] private Image dialogueWindow;

        [Space(10), SerializeField] private Sprite dialogueWindowTexture;
        [SerializeField] private Sprite monologueWindowTexture;

        [FormerlySerializedAs("_dialogue")] [Space(10), SerializeField] private Talk[] talks;

        private bool _isNotCloseable;
        private int _dialogueID;
        private int _phraseNum;

        public void Start()
        {
            StartDialogue(false, 1);
        }

        /// <summary>
        /// Это метод для начала диалога с персонажем. Переменная importance означает его важность: false - пропускаемый, true - соответственно, не пропускаемый
        /// </summary>
        /// <param name="importance"></param>
        public void StartDialogue(bool importance, int dialogueNum)
        {
            _isNotCloseable = importance;
            _dialogueID = dialogueNum - 1;

            window.SetActive(true);

            dialogueWindow.sprite = dialogueWindowTexture;
            nameText.enabled = true;
            characterImage.enabled = true;

            if (talks[_dialogueID].monologue)
            {
                dialogueWindow.sprite = monologueWindowTexture;
                nameText.enabled = false;
                characterImage.enabled = false;
            }

            nameText.text = "N";

            NextPhrase();
        }

        public void NextPhrase()
        {
            var talk = talks[_dialogueID];

            if (_phraseNum + 1 > talk.phrases.Length)
            {
                EndDialogue();
                return;
            }
            
            if (!talks[_dialogueID].monologue) nameText.text = talk.phrases[_phraseNum].character;
            dialogueText.text = talk.phrases[_phraseNum].text;
            nameText.text = talk.phrases[_phraseNum].character;
            characterImage.sprite = talk.phrases[_phraseNum].characterIcon;
            _phraseNum++;
        }

        /// <summary>
        /// Резкое закрытие диалога с персонажем (Если переменная importance не равна 1).
        /// </summary>
        public void CloseDialogue()
        {
            if (_isNotCloseable) return;
            EndDialogue();
        }


        private void EndDialogue()
        {
            window.SetActive(false);
        }
    }
}
