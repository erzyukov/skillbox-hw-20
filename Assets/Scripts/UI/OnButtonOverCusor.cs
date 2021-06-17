using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Компонент смены курсора при наведении на кнопку
/// </summary>
public class OnButtonOverCusor : MonoBehaviour
{
    /// <summary>
    /// Курсор для состояния наведение на интерактивный объект
    /// </summary>
    private Texture2D onOverCursor = default;

    private void Awake()
    {
        onOverCursor = Resources.Load<Texture2D>("UI/Cursor/Pointer");

        // находим все кнопки в окне и добавляем необходимые события
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
            AddOnEnterDeligate(ref trigger);
            AddOnExitDeligate(ref trigger);
            AddOnUpDeligate(ref trigger);
        }
    }

    /// <summary>
    /// Смена курсора при наведении мыши
    /// </summary>
    /// <param name="data">Данные события</param>
    public void OnMouseOver(PointerEventData data = default)
    {
        Cursor.SetCursor(onOverCursor, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Смена курсора при выходе мыши
    /// </summary>
    /// <param name="data">Данные события</param>
    public void OnMouseExit(PointerEventData data = default)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Устанавливает событие наведения мышы на триггер типа PointerEnter
    /// </summary>
    /// <param name="trigger">триггер</param>
    private void AddOnEnterDeligate(ref EventTrigger trigger)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnMouseOver((PointerEventData) data); });
        trigger.triggers.Add(entry);
    }

    /// <summary>
    /// Устанавливает событие выхода мышы на триггер типа PointerExit
    /// </summary>
    /// <param name="trigger">триггер</param>
    private void AddOnExitDeligate(ref EventTrigger trigger)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { OnMouseExit((PointerEventData) data); });
        trigger.triggers.Add(entry);
    }

    /// <summary>
    /// Устанавливает событие выхода мышы на триггер типа PointerUp
    /// </summary>
    /// <param name="trigger">триггер</param>
    private void AddOnUpDeligate(ref EventTrigger trigger)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { OnMouseExit((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

}