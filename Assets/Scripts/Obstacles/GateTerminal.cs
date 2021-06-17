using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Компонент терминал шлюза. 
/// Если игрок контактирует с объектом, 
/// то после определенного времени посылается сигнал на открытие шлюза
/// </summary>
public class GateTerminal : MonoBehaviour
{
    /// <summary>
    /// Время через которое произойдет действие, если игрок контактирует с объектом
    /// </summary>
    [SerializeField] private float timeToAction = 2;
    /// <summary>
    /// Картинка для визуализация прогресса запуска терминала
    /// </summary>
    [SerializeField] private Image progress = null;
    /// <summary>
    /// Переходить ли в состояние закрытия если игрок потерял контакт с объектом
    /// </summary>
    [SerializeField] private bool isCloseAfterExit = false;
    /// <summary>
    /// Ссылка на компонент управления шлюзом
    /// </summary>
    [SerializeField] private GateController gc = default;

    public StateType state;
    public enum StateType { Opened, Opening, Closed, Closing };

    private float startActionTime = 0;

    private Coroutine currentCoroutine;

    private GameObject target = default;

    private void Awake()
    {
        state = StateType.Closed;
    }

    private void FixedUpdate()
    {
        // если цель есть, но она не активна (игрок погиб) и были в состоянии открытия
        if ((target == default || !target.activeSelf) && state == StateType.Opening)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            state = StateType.Closed;
            StopAction();
        }

        // анимация UI прогресса открытия/закрытия
        switch (state)
        {
            case StateType.Opening:
                progress.fillAmount = Mathf.Clamp((Time.time - startActionTime) / timeToAction , 0, 1);
                break;
            case StateType.Closing:
                progress.fillAmount = 1 - Mathf.Clamp((Time.time - startActionTime) / timeToAction, 0, 1);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (target == default)
            {
                target = other.gameObject;
            }
            // Если был запущен таймер закрытия - прерываем его
            if (state == StateType.Closing)
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }
                state = StateType.Opened;
                StopAction();
            }

            // Если статус в состоянии закрыто - запускаем таймер на открытие
            if (state == StateType.Closed)
            {
                currentCoroutine = StartCoroutine(ActionCoroutine(StateType.Opened));
                state = StateType.Opening;
                StartAction(0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (target)
            {
                target = default;
            }
            // Если был запущен таймер открытия - прерываем его
            if (state == StateType.Opening)
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }
                state = StateType.Closed;
                StopAction();
            }

            // Если статус в состоянии открыто и терминал автоматически закрывает - запускаем таймер на закрытие
            if (state == StateType.Opened && isCloseAfterExit)
            {
                currentCoroutine = StartCoroutine(ActionCoroutine(StateType.Closed));
                state = StateType.Closing;
                StartAction(1);
            }
        }
    }

    /// <summary>
    /// Проставляет переменные для запуска действия
    /// </summary>
    /// <param name="fillAmount">Стартовое значение прогресса интерфейса</param>
    private void StartAction(float fillAmount)
    {
        startActionTime = Time.time;
        progress.enabled = true;
        progress.fillAmount = fillAmount;
    }

    /// <summary>
    /// Проставляет переменные для остановки действия
    /// </summary>
    private void StopAction()
    {
        startActionTime = 0;
        progress.enabled = false;
    }

    IEnumerator ActionCoroutine(StateType actionType)
    {
        yield return new WaitForSeconds(timeToAction);
        state = actionType;
        if (state == StateType.Opened)
        {
            gc.OpenGate();
        }
        else if (state == StateType.Closed)
        {
            gc.CloseGate();
        }
        StopAction();
    }

}
