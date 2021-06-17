using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Абстрактный класс для компонент управления эффектом
/// </summary>
public abstract class UFOEffect : MonoBehaviour
{
    public bool Enabled { get { return isEnabled; } }

    protected GameObject effect = null;
    protected bool isEnabled = false;

    public void InitEffects(GameObject effect)
    {
        this.effect = effect;
    }

    /// <summary>
    /// Проставляет состояние активности эффекта
    /// </summary>
    /// <param name="state">Состояние активноси</param>
    public virtual void UpdateState(bool state)
    {
        isEnabled = state;
        effect.SetActive(state);
    }

}
