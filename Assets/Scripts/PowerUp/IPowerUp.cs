using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Интерфейс усилений
/// </summary>
public interface IPowerUp
{
    /// <summary>
    /// Цель на которую воздействует усиление
    /// </summary>
    GameObject Target { get; set; }
    UFOEffect Effect { get; set; }

    /// <summary>
    /// Тип усиления
    /// </summary>
    PowerUpBehaviour Behaviour { get; set; }

    /// <summary>
    /// Усиление активированно
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    /// Обработка поднятия усиления
    /// </summary>
    void PickUp(GameObject target, UFOEffect effect);

    /// <summary>
    /// Обработка активации усиления
    /// </summary>
    void Enable();

    /// <summary>
    /// Обработка деактивации усиления
    /// </summary>
    void Disable();

}

/// <summary>
/// Типы усилений
/// Usable          - используемый
/// Disposable      - используемый один раз
/// SelfActivating  - активируется при поднятии
/// Permanent       - всегда включен
/// </summary>
public enum PowerUpBehaviour
{
    None, Usable, Disposable, SelfActivating, Permanent
}