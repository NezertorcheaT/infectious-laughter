using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [Serializable]
    public class dialogue
    {
        [Serializable]
        public class phrase
        {
            public string character;
            [TextArea] public string text;
        }

        public bool monologue;

        public phrase[] _phrase;
    }


    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Space(10), SerializeField] private GameObject window;

    [Space(10), SerializeField] private Image dialogueWindow;

    [Space(10), SerializeField] private Sprite dialogueWindowTexture;
    [SerializeField] private Sprite monologueWindowTexture;

    [Space(10), SerializeField] private dialogue[] _dialogue;

    public bool isDialogueEnable { get; private set; }


    private bool _isNotCloseable;
    private int _dialogueID;

    private int _phraseNum;

    /// <summary>
    /// Это метод для начала диалога с персонажем. Переменная importance означает его важность: false - пропускаемый, true - соответственно, не пропускаемый
    /// </summary>
    /// <param name="importance"></param>
    public void StartDialogue(bool importance, int dialogueNum)
    {
        _isNotCloseable = importance;
        _dialogueID = dialogueNum - 1;

        window.active = true;

        dialogueWindow.sprite = dialogueWindowTexture;
        if (_dialogue[_dialogueID].monologue) dialogueWindow.sprite = monologueWindowTexture;
        nameText.text = "N";

        NextPhrase();
    }

    public void NextPhrase()
    {
        var _dl = _dialogue[_dialogueID];

        if (_phraseNum + 1 > _dl._phrase.Length)

        if (!_dialogue[_dialogueID].monologue) nameText.text = _dl._phrase[_phraseNum].character;
        dialogueText.text = _dl._phrase[_phraseNum].text;
        nameText.text = _dl._phrase[_phraseNum].character;
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
        window.active = false;
    }
}
