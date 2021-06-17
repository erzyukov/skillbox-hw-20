using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс управления приборной доской
/// поддерживаются следующие параметры: максимальная вместимость топливо, количество текущего топлива, направление полета,
/// уровень, текущая подача мощьности левого и правого двигателя, скорость, высота, 
/// шкала вертикальной скорости, шкала скорости, шкала высоты,
/// индикаторы низкого уровня топлива и зарядки, отображение иконки текущего уселителя
/// </summary>
public class UIDashboardController : MonoBehaviour
{
    [SerializeField] private Text fuelMaxCapacitySensor = null;
    [SerializeField] private Text fuelPercentSensor = null;
    [SerializeField] private Slider energySensor = null;
    /*
    [SerializeField] private Transform directionSensorArrow = null;
    [SerializeField] private Transform levelSensor = null;
    [SerializeField] private Slider leftJetPowerSensor = null;
    [SerializeField] private Slider rightJetPowerSensor = null;
    [SerializeField] private Text speedSensor = null;
    [SerializeField] private Text heightSensor = null;
    [SerializeField] private UIScaleController rateOfClimbScaleSensor = null;
    [SerializeField] private UIScaleController speedScaleSensor = null;
    [SerializeField] private UIScaleController heightScaleSensor = null;
    */
    [SerializeField] private UIPowerUpController uiPowerUpSensor = null;


    [Space]
    [SerializeField] private Image lowEnergyIcon = null;
    [SerializeField] private Image chargingIcon = null;

    [Space]
    //[SerializeField] private UFOJetsController targetJC = null;
    [SerializeField] private UFOFuelContorller targetFC = null;
    [SerializeField] private UFOPowerUpContoller targetPUC = null;
    //[SerializeField] private Transform targetPosition = null;

    private void Start()
    {
        //targetJC.OnJetsWork += OnJetsWork;
        targetFC.OnLowFuel += OnLowFuel;
        targetFC.OnEnoughFuel += OnEnoughFuel;
        targetFC.OnStartChargingFuel += OnStartChargingFuel;
        targetFC.OnFinishChargingFuel += OnFinishChargingFuel;
        targetFC.OnMaxCapacityChange += OnMaxCapacityChange;
        targetPUC.OnPickedUp += OnPickedUpPowerUp;
        targetPUC.OnUse += OnUsePowerUp;
        targetPUC.OnTurnOn += OnTurnOnPowerUp;
        targetPUC.OnTurnOff += OnTurnOffPowerUp;

        lowEnergyIcon.gameObject.SetActive(false);
        chargingIcon.gameObject.SetActive(false);

        UpdateMaxFuelCapacitySensor(targetFC.MaxCapacity);

        /*
        rateOfClimbScaleSensor.InitScales(targetJC.GetSpeedVector().y);
        speedScaleSensor.InitScales(targetJC.GetSpeedValue());
        heightScaleSensor.InitScales(targetPosition.position.y);
        */
    }

    private void FixedUpdate()
    {
        UpdateEnergySensor(targetFC.CapacityPercent);
        /*
        UpdateDirectionSensor(targetJC.GetSpeedVector());
        UpdateSpeedSensor(targetJC.GetSpeedValue());
        UpdateHeightSensor(targetPosition.position.y);
        UpdateLevelSensor(targetPosition.up);
        UpdateRateOfClimbSencor(targetJC.GetSpeedVector().y);
        UpdateSpeedScaleSencor(targetJC.GetSpeedValue());
        UpdateHeightScaleSencor(targetPosition.position.y);
        */
    }

    private void UpdateMaxFuelCapacitySensor(float capacity)
    {
        fuelMaxCapacitySensor.text = capacity.ToString();
    }

    private void UpdateEnergySensor(float value)
    {
        energySensor.value = value;
        fuelPercentSensor.text = Mathf.RoundToInt(value * 100).ToString() + " %";
    }

    /*
    private void UpdateLevelSensor(Vector3 targetNormal)
    {
        float angle = 0;
        if (targetNormal != Vector3.up)
        {
            angle = Vector3.Angle(Vector3.up, targetNormal);

            if (targetNormal.x > 0)
                angle *= -1;
        }

        levelSensor.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    private void UpdateDirectionSensor(Vector3 speed)
    {
        float angle = -90;
        if (speed != Vector3.zero)
        {
            angle = Vector3.Angle(Vector3.right, speed);
            if (speed.y < 0)
                angle *= -1;
        }

        directionSensorArrow.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void UpdateSpeedSensor(float speed)
    {
        speedSensor.text = Mathf.RoundToInt(speed).ToString();
    }

    private void UpdateHeightSensor(float height)
    {
        heightSensor.text = Mathf.RoundToInt(height).ToString();
    }

    private void UpdateRateOfClimbSencor(float verticalSpeed)
    {
        rateOfClimbScaleSensor.SetValue(verticalSpeed);
    }

    private void UpdateSpeedScaleSencor(float speed)
    {
        speedScaleSensor.SetValue(speed);
    }

    private void UpdateHeightScaleSencor(float height)
    {
        heightScaleSensor.SetValue(height);
    }

    private void OnJetsWork(float power, UFOJetsController.JetSide side)
    {
        float percent = power / targetJC.JetsPower;

        switch (side)
        {
            case UFOJetsController.JetSide.Left:
                leftJetPowerSensor.value = percent;
                break;
            case UFOJetsController.JetSide.Right:
                rightJetPowerSensor.value = percent;
                break;
        }
    }
    */
    private void OnLowFuel()
    {
        lowEnergyIcon.gameObject.SetActive(true);
    }

    private void OnEnoughFuel()
    {
        lowEnergyIcon.gameObject.SetActive(false);
    }

    private void OnStartChargingFuel()
    {
        chargingIcon.gameObject.SetActive(true);
    }

    private void OnFinishChargingFuel()
    {
        chargingIcon.gameObject.SetActive(false);
    }

    private void OnMaxCapacityChange(float capacity)
    {
        UpdateMaxFuelCapacitySensor(capacity);
    }

    private void OnPickedUpPowerUp(PowerUpItem.Type type)
    {
        uiPowerUpSensor.SetIcon(type);
    }

    private void OnUsePowerUp()
    {
        uiPowerUpSensor.ResetIcon();
    }

    private void OnTurnOnPowerUp()
    {
        uiPowerUpSensor.TurnOn();
    }
    private void OnTurnOffPowerUp()
    {
        uiPowerUpSensor.TurnOff();
    }

    private void OnDestroy()
    {
        /*
        if (targetJC != null)
        {
            targetJC.OnJetsWork -= OnJetsWork;
        }
        */
        if (targetFC != null)
        {
            targetFC.OnLowFuel -= OnLowFuel;
            targetFC.OnEnoughFuel -= OnEnoughFuel;
            targetFC.OnStartChargingFuel -= OnStartChargingFuel;
            targetFC.OnFinishChargingFuel -= OnFinishChargingFuel;
            targetFC.OnMaxCapacityChange -= OnMaxCapacityChange;
        }
        if (targetPUC != null)
        {
            targetPUC.OnPickedUp -= OnPickedUpPowerUp;
            targetPUC.OnUse -= OnUsePowerUp;
            targetPUC.OnTurnOn -= OnTurnOnPowerUp;
            targetPUC.OnTurnOff -= OnTurnOffPowerUp;
        }
    }
}
