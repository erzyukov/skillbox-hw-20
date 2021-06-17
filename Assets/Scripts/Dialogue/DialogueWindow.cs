using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Компонент диалогового окна.
/// Отвечает за отображение диалогов в игре.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class DialogueWindow : MonoBehaviour
{
    /// <summary>
    /// Ссылка на компонент объекта Canvas
    /// </summary>
    [SerializeField] private Canvas canvas = default;

    [Space]
    /// <summary>
    /// Заголовок
    /// </summary>
    [SerializeField] private Text title = null;
    /// <summary>
    /// Блок текста
    /// </summary>
    [SerializeField] private Text content = null;
    /// <summary>
    /// Изображение
    /// </summary>
    [SerializeField] private Image image = null;
    
    [Space]
    /// <summary>
    /// Чекбокс пропуска подсказок
    /// </summary>
    [SerializeField] private Toggle skipTipsToggle = null;

    [Space]
    /// <summary>
    /// Ссылка на процедуру запускающую сдедующее предложение
    /// </summary>
    [SerializeField] private UnityEvent nextActionEvent = default;
    /// <summary>
    /// Ссылка на процедуру пропускающую текущий диалог
    /// </summary>
    [SerializeField] private UnityEvent skipActionEvent = default;
    /// <summary>
    /// Ссылка на процедуру позволяющую отключать подсказки
    /// </summary>
    [SerializeField] private SkipTipsToggleEvent skipTipsEvent = default;

    private Coroutine textCoroutine;

    private void Awake()
    {
        gameObject.SetActive(true);
        canvas.enabled = false;
        gameObject.AddComponent<OnButtonOverCusor>();
        if (skipTipsToggle)
        {
            skipTipsToggle.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Показать окно
    /// </summary>
    public void Show()
    {
        canvas.enabled = true;
    }

    /// <summary>
    /// Спрятать окно
    /// </summary>
    public void Hide()
    {
        canvas.enabled = false;
    }

    /// <summary>
    /// Установить заголовок окна
    /// </summary>
    /// <param name="value">Текст заголовка</param>
    public void SetTitle(string value)
    {
        title.text = value;
    }

    /// <summary>
    /// Установить текст
    /// </summary>
    /// <param name="value">Текст</param>
    public void SetContent(string value)
    {
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
        }
        textCoroutine = StartCoroutine(TypeSentence(value));
    }

    /// <summary>
    /// Установить изображение
    /// </summary>
    /// <param name="value">Изображение</param>
    public void SetImage(Sprite value)
    {
        image.sprite = value;
    }

    /// <summary>
    /// Корутин для анимации отображения текста
    /// </summary>
    /// <param name="sentence">Текст</param>
    /// <returns></returns>
    IEnumerator TypeSentence(string sentence)
    {
        content.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            content.text += letter;
            yield return null;
        }
    }

    /// <summary>
    /// Вызывает действие определенное для нажатия на кнопку следующего предложения
    /// </summary>
    public void OnNextButtonPress()
    {
        nextActionEvent.Invoke();
    }

    /// <summary>
    /// Вызывает действие определенное для нажатия на кнопку окончания диалога
    /// </summary>
    public void OnSkipButtonPress()
    {
        skipActionEvent.Invoke();
    }

    /// <summary>
    /// Устанавливает значение чекбокса состояния пропуска подсказок
    /// </summary>
    /// <param name="state">Состояние</param>
    public void SetSkipTips(bool state)
    {
        skipTipsToggle.isOn = state;
    }

    /// <summary>
    /// Вызывает действие определенное для нажатия на чекбокс пропуск подсказок
    /// </summary>
    /// <param name="state">Состояние</param>
    public void OnSkipTipsChecked(bool state)
    {
        skipTipsEvent.Invoke(state);
    }

    /// <summary>
    /// Меняет состояние отображения объекта чексбокса пропуска подсказок
    /// </summary>
    /// <param name="state">Состояние</param>
    public void SetActiveSkipTipsToggle(bool state)
    {
        if (skipTipsToggle)
        {
            skipTipsToggle.gameObject.SetActive(state);
        }
    }


}

[System.Serializable]
public class SkipTipsToggleEvent : UnityEvent<bool>
{
}
