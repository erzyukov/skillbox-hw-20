using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Компонент управления шлюзами
/// </summary>
public class GateController : MonoBehaviour
{
    /// <summary>
    /// Верхняя часть шлюза
    /// </summary>
    [SerializeField] private GameObject topGate = null;
    /// <summary>
    /// Нижняя часть шлюза
    /// </summary>
    [SerializeField] private GameObject bottomGate = null;
    /// <summary>
    /// Общая высота шлюза
    /// </summary>
    [SerializeField] private float gateHeight = 0;
    /// <summary>
    /// Скорость открытия/закрытия шлюза
    /// </summary>
    [SerializeField] private float actionSpeed = 2;

    [Range(0f, 1f)]
    [SerializeField] private float openProgress = 0;

    [Space]
    [SerializeField] private AudioSource move = default;

    /// <summary>
    /// Состояние шлюза
    /// Opened  - открыт
    /// Opening - открывается
    /// Closing - закрывается
    /// Closed  - закрыт
    /// </summary>
    private enum StateType { None, Opened, Opening, Closing, Closed };

    private StateType state;

    private void Awake()
    {
        if (openProgress == 1) {
            state = StateType.Opened;
        }
        else if (openProgress == 0)
        {
            state = StateType.Closed;
        }
        SetOpenProgress(openProgress);
    }

    private void Update()
    {
        // обработка закрытия и открытия шлюза
        switch(state)
        {
            case StateType.Opening:
                OpeningHandle();
                break;
            case StateType.Closing:
                ClosingHandle();
                break;
        }

    }

    /// <summary>
    /// Запуск открытия шлюза
    /// </summary>
    public void OpenGate()
    {
        if (move && state != StateType.Opened)
        {
            move.Play();
        }
        state = StateType.Opening;
    }

    /// <summary>
    /// Запуск закрытия шлюза
    /// </summary>
    public void CloseGate()
    {
        if (move && state != StateType.Closed)
        {
            move.Play();
        }
        state = StateType.Closing;
    }

    /// <summary>
    /// Процесс открытия шлюза
    /// </summary>
    private void OpeningHandle()
    {
        openProgress += Time.deltaTime * actionSpeed;
        if (openProgress >= 1)
        {
            openProgress = 1;
            state = StateType.Opened;
        }
        SetOpenProgress(openProgress);
    }

    /// <summary>
    /// Процесс закрытития шлюза
    /// </summary>
    private void ClosingHandle()
    {
        openProgress -= Time.deltaTime * actionSpeed;
        if (openProgress <= 0)
        {
            openProgress = 0;
            state = StateType.Closed;
        }
        SetOpenProgress(openProgress);
    }

    /// <summary>
    /// Выставляет прогресс открытия шлюза (от 0 до 1)
    /// </summary>
    /// <param name="value">Значение прогресса открытия</param>
    private void SetOpenProgress(float value)
    {
        value = Mathf.Clamp(value, 0, 1);
        topGate.transform.localPosition = Vector3.up * (value * gateHeight / 2);
        bottomGate.transform.localPosition = Vector3.down * (value * gateHeight / 2);
    }
}
