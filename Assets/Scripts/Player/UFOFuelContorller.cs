using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс управления топливом.
/// </summary>
public class UFOFuelContorller : MonoBehaviour
{
    public bool OutOfFuel { get { return isOutOfFuel; } }
    public float CapacityPercent { get { return capacity / maxCapacity; } }
    public float MaxCapacity { get { return maxCapacity; } }

    public delegate void FuelAction();
    public event FuelAction OnOutOfFuel;
    public event FuelAction OnLowFuel;
    public event FuelAction OnEnoughFuel;
    public event FuelAction OnStartChargingFuel;
    public event FuelAction OnFinishChargingFuel;
    public delegate void MaxCapacityAction(float capacity);
    public event MaxCapacityAction OnMaxCapacityChange;

    /// <summary>
    /// Класс управления двигателем
    /// </summary>
    [SerializeField] private UFOJetsController jets = null;

    [Space]

    /// <summary>
    /// Максимальное количество топлива
    /// </summary>
    [SerializeField] private float maxCapacity = 10000;
    /// <summary>
    /// Потребление топлива за единицу заданной силы двигателю
    /// </summary>
    [SerializeField] private float consumptionPerPowerUnit = 0.005f;
    /// <summary>
    /// Скорость заправки
    /// </summary>
    [SerializeField] private float chargingSpeed = 1000;

    /// <summary>
    /// Процент топлива меньше которого загорается предупреждение о минимальном количестве топлива
    /// </summary>
    [Range(0, 1f)]
    [SerializeField] private float minLowPowerIndicator = .3f;

    [Space]
    [SerializeField] private AudioSource chargeSound = default;

    [Space]
    private bool isCharging = false;
    private float capacity;

    private bool isLowPower = false;
    private bool isOutOfFuel = false;

    private float additionalConsumtionAmount = 0;

    private void Awake()
    {
        capacity = maxCapacity;
    }

    private void Start()
    {
        jets.OnJetsWork += OnJetsWork;
        jets.OnChargeEnebled += OnChargeEnebled;
        jets.OnChargeDisabled += OnChargeDisabled;
    }

    private void FixedUpdate()
    {
        if (additionalConsumtionAmount > 0)
        {
            ConsumeEnergy(additionalConsumtionAmount * Time.fixedDeltaTime);
        }

        if (isCharging)
        {
            if (capacity > 0 && isOutOfFuel)
            {
                jets.SetOutOfFuel(false);
                isOutOfFuel = false;
            }

            if (isLowPower)
            {
                isLowPower = false;
                OnEnoughFuel?.Invoke();
            }

            if (capacity <= maxCapacity)
            {
                capacity += chargingSpeed * Time.fixedDeltaTime;
            }
            else
            {
                capacity = maxCapacity;
                SetCharging(false);
            }
        }
        else
        {
            if (CapacityPercent < minLowPowerIndicator && !isLowPower)
            {
                isLowPower = true;
                OnLowFuel?.Invoke();
            }
        }
    }

    /// <summary>
    /// Добавление дополнительной затраты топлива в секунду
    /// </summary>
    /// <param name="amount">Количество топлива</param>
    public void AddConsumtion(float amount)
    {
        additionalConsumtionAmount += amount;
    }

    /// <summary>
    /// Удаление дополнительной затраты топлива в секунду
    /// </summary>
    /// <param name="amount">Количество топлива</param>
    public void RemoveConsumtion(float amount)
    {
        additionalConsumtionAmount -= amount;
        if (additionalConsumtionAmount < 0)
        {
            additionalConsumtionAmount = 0;
        }
    }

    /// <summary>
    /// Добавление топлива
    /// </summary>
    /// <param name="amount">Количество топлива</param>
    public void AddFuel(float amount)
    {
        capacity += amount;
        if (capacity > maxCapacity)
        {
            capacity = maxCapacity;
        }
    }

    /// <summary>
    /// Удаляет определенное количество энергии в зависимости от переданой (потраченной) мощьности
    /// </summary>
    /// <param name="power">Мощьность</param>
    public void RemoveFuel(float power)
    {
        ConsumeEnergy(power * consumptionPerPowerUnit);
    }

    /// <summary>
    /// Заправляет полный бак
    /// </summary>
    public void FillUp()
    {
        capacity = maxCapacity;
    }

    /// <summary>
    /// Увеличение вместимости топлива
    /// </summary>
    /// <param name="amount">Количество топлива</param>
    public void RaiseCapacity(float amount)
    {
        maxCapacity += amount;
        OnMaxCapacityChange?.Invoke(maxCapacity);
    }

    public void SetCurrentCapacity(float value)
    {
        capacity = value;
    }

    public void SetCurrentMaxCapacity(float value)
    {
        maxCapacity = value;
        capacity = value;
        OnMaxCapacityChange?.Invoke(maxCapacity);
    }

    private void OnJetsWork(float power, UFOJetsController.JetSide side)
    {
        ConsumeEnergy(power * consumptionPerPowerUnit);
    }

    private bool ConsumeEnergy(float amount)
    {
        if (capacity > amount)
        {
            capacity -= amount;
            return true;
        }
        capacity = 0;
        SetOutOfFuel();
        return false;
    }

    private void SetOutOfFuel()
    {
        jets.SetOutOfFuel(true);
        isOutOfFuel = true;
        OnOutOfFuel?.Invoke();
    }

    private void OnChargeEnebled()
    {
        SetCharging(true);
    }

    private void OnChargeDisabled()
    {
        SetCharging(false);
    }

    private void SetCharging(bool state)
    {
        isCharging = state;
        if (state)
        {
            if (chargeSound)
            {
                chargeSound.Play();
            }
            OnStartChargingFuel?.Invoke();
        }
        else
        {
            if (chargeSound)
            {
                chargeSound.Stop();
            }
            OnFinishChargingFuel?.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (jets != null)
        {
            jets.OnJetsWork -= OnJetsWork;
            jets.OnChargeEnebled -= OnChargeEnebled;
            jets.OnChargeDisabled -= OnChargeDisabled;
        }
    }
}
