using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс объекта усиления
/// </summary>
[RequireComponent(typeof(Collider))]
public class PowerUpItem : MonoBehaviour
{
    public IPowerUp PowerUp { get { return powerUp; } }

    /// <summary>
    /// Может ли усилитель обновиться при сбросе уровня
    /// </summary>
    [SerializeField] private bool isRenewable = true;

    /// <summary>
    /// Типы усилений
    /// </summary>
    [System.Serializable]
    public enum Type { ForceFeld, EnergyBattery, EnergyCapacity, Stabilize, Lantern, Invisible };

    /// <summary>
    /// Тип усиления
    /// </summary>
    public Type type;

    /// <summary>
    /// Количество потребления энергии усилителем
    /// </summary>
    [HideInInspector] public float energyConsumption;

    /// <summary>
    /// Количество добавляемой энергии усилителем
    /// </summary>
    [HideInInspector] public float energyAmount;

    /// <summary>
    /// Текущая мощьность двигателей
    /// </summary>
    [HideInInspector] public float jetsPower;

    /// <summary>
    /// Отображаемый эффект при активации усилителя
    /// </summary>
    //[HideInInspector] public GameObject targetEffect;

    private IPowerUp powerUp;

    private void Awake()
    {
        gameObject.tag = "PowerUp";
        // в зависимости от типа усиления создаем объект соответствующего класса
        switch (type)
        {
            case Type.ForceFeld:
                powerUp = new ForceField(energyConsumption);
                break;
            case Type.EnergyBattery:
                powerUp = new EnergyBattery(energyAmount);
                break;
            case Type.EnergyCapacity:
                powerUp = new EnergyCapacity(energyAmount);
                break;
            case Type.Stabilize:
                powerUp = new Stabilize(jetsPower);
                break;
            case Type.Lantern:
                powerUp = new Lantern(energyConsumption);
                break;
            case Type.Invisible:
                powerUp = new Invisible(energyConsumption, jetsPower);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Обработка поднятия предмета
    /// </summary>
    public void TakeItemHandler()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Сбрасывает состояние предмета, если он возобновляемый
    /// </summary>
    public void ResetItemObject()
    {
        if (isRenewable)
        {
            gameObject.SetActive(true);
        }
    }

}
