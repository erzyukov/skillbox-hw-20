using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Компонент управления диалогами
/// Диалоги без ветвлений
/// Во время диалога игра ставится на паузу
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public bool IsSkipTips { get { return isSkipTips; } }

    /// <summary>
    /// Объект диалога
    /// </summary>
    [SerializeField] private DialogueObject dialogue = null;
    /// <summary>
    /// Окно диалога
    /// </summary>
    [SerializeField] private DialogueWindow window = null;

    [Space]
    /// <summary>
    /// Отображать ли заданный диалог при старте игры
    /// </summary>
    [SerializeField] private bool isActivateOnStart = false;

    /// <summary>
    /// Состояние на которое переключится игра при завершении цепочки диалога
    /// </summary>
    [SerializeField] private GameManager.StateType finishState = default;

    private Queue<DialogueSentence> sentences;
    private bool isSkipTips = false;

    private void Awake()
    {
        sentences = new Queue<DialogueSentence>();
        isSkipTips = (PlayerPrefs.GetInt("isSkipTips") == 1);
    }

    private void Start()
    {
        if (isActivateOnStart)
        {
            StartDialogue();
        }
        window.SetSkipTips(isSkipTips);
    }

    /// <summary>
    /// Начать диалог
    /// Использует объект диалога заданный в менеджере, либо переданный в функцию
    /// </summary>
    /// <param name="dialogue">Объект диалога</param>
    public void StartDialogue(DialogueObject dialogue = null)
    {
        if (dialogue)
        {
            this.dialogue = dialogue;
        }
        if (this.dialogue)
        {
            GameManager.instance.SetDialogueState();

            // если указан пропуск обучающих диалогов - пропускаем
            if (TestSkipTips())
            {
                return;
            }

            window.SetActiveSkipTipsToggle((this.dialogue.type == DialogType.Lesson));

            window.Show();
            sentences.Clear();
            foreach (DialogueSentence sentence in this.dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }
    }

    /// <summary>
    /// Показывает следующее предложение
    /// </summary>
    public void DisplayNextSentence()
    {
        // если указан пропуск обучающих диалогов - пропускаем
        if (TestSkipTips())
        {
            return;
        }

        // если предложения закончились запускаем следующий диалог
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueSentence sentence = sentences.Dequeue();

        // устанавливаем параметры диалогового окна
        window.SetTitle(dialogue.speakerName);
        window.SetContent(sentence.text);
        if (sentence.image)
        {
            window.SetImage(sentence.image);
        }
    }

    /// <summary>
    /// Пропустить диалог
    /// </summary>
    public void SkipDialogue()
    {
        EndDialogue();
    }

    /// <summary>
    /// Переключает активность отображения подсказок
    /// </summary>
    /// <param name="state"></param>
    public void ToggleSkipTip(bool state)
    {
        isSkipTips = state;
        window.SetSkipTips(isSkipTips);
        PlayerPrefs.SetInt("isSkipTips", (isSkipTips? 1: 0));
    }

    /// <summary>
    /// Завершает текущий диалог
    /// </summary>
    private void EndDialogue()
    {
        // если есть ссылка на следующий диалог - запускаем его
        if (dialogue.nextDialogue != null)
        {
            dialogue = dialogue.nextDialogue;
            StartDialogue();
        }
        else
        {
            window.Hide();
            GameManager.instance.FinishDialogue(finishState);
        }
    }

    /// <summary>
    /// Проверяем можно ли пропустить обущающий диалог, если да - пропускаем
    /// </summary>
    /// <returns>Можно ли пропустить</returns>
    private bool TestSkipTips()
    {
        if (dialogue.type == DialogType.Lesson && isSkipTips)
        {
            EndDialogue();
            return true;
        }

        return false;
    }
}
