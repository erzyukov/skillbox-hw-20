using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс управляющий двигателями коробля, является базовым классом корабля
/// </summary>
public class UFOJetsController : MonoBehaviour
{
    public enum JetSide { None, Left, Right };

    public delegate void JetsAction(float power, JetSide side = JetSide.None);
    public event JetsAction OnJetsWork;

    public delegate void ChargeAction();
    public event ChargeAction OnChargeEnebled;
    public event ChargeAction OnChargeDisabled;

    public bool StabilizeEnabled { get { return isStabilizeEnabled; } set { isStabilizeEnabled = value; } }
    public float JetsPower { get { return currentJetsPower; } set { currentJetsPower = value; } }

    /// <summary>
    /// Левый двигатель
    /// </summary>
    [SerializeField] private Rigidbody leftJets = null;
    /// <summary>
    /// Правый двигатель
    /// </summary>
    [SerializeField] private Rigidbody rightJets = null;

    [Space]
    /// <summary>
    /// Эффекты для двигателей
    /// </summary>
    [SerializeField] private ParticleSystem leftFrontJetsFire = null;
    [SerializeField] private ParticleSystem leftRearJetsFire = null;
    [SerializeField] private ParticleSystem rightFrontJetsFire = null;
    [SerializeField] private ParticleSystem rightRearJetsFire = null;

    [Space]
    /// <summary>
    /// Мощьность двигателя
    /// </summary>
    [SerializeField] private float maxJetsPower = 550;

    [Space]
    /// <summary>
    /// Множетель мощьности при задании движения вверх
    /// </summary>
    [Range(0, 1f)]
    [SerializeField] private float upPowerPercent = .8f;
    /// <summary>
    /// Множетель мощьности при отсутствии направления движения по вертикали
    /// </summary>
    [Range(0, 1f)]
    [SerializeField] private float idlePowerPercent = .4f;
    /// <summary>
    /// Множетель мощьности при задании движения вниз
    /// </summary>
    [Range(0, 1f)]
    [SerializeField] private float downPowerPercent = .2f;
    /// <summary>
    /// Множетель мощьности при задании крена
    /// </summary>
    [Range(0, .2f)]
    [SerializeField] private float tiltPowerPercent = .04f;

    [Space]
    /// <summary>
    /// Режим стабилизации коробля (вкл/выкл)
    /// </summary>
    [SerializeField] private bool isStabilizeEnabled = false;
    /// <summary>
    /// Угол, выходя за который корабль может запустить стабилизацию
    /// </summary>
    [SerializeField] private float stabilizeStartAngle = 0.05f;
    /// <summary>
    /// Множитель мощьности, которая задается для обратного крена при запуске стабилизации
    /// </summary>
    [SerializeField] private float stabilizeStartPower = 0.015f;

    [Space]
    /// <summary>
    /// Ссылка на компонент Rigidbody
    /// </summary>
    [SerializeField] private Rigidbody objectRigidbody = default;

    private float currentJetsPower;

    private enum StabilizeState { Right, Left, None};
    private StabilizeState currentStabilizeState = StabilizeState.None;

    private bool isLanded;
    private bool isOutOfFuel;

    float rightPowerPercent = 0;
    float leftPowerPercent = 0;

    void Start()
    {
        ResetJetsPower();
        isLanded = true;
        SetLeftJetsFire(0);
        SetRightJetsFire(0);
    }

    public void Move(float dir, float tilt)
    {
        rightPowerPercent = 0;
        leftPowerPercent = 0;

        if (!isLanded)
        {
            rightPowerPercent = idlePowerPercent;
            leftPowerPercent = idlePowerPercent;
        }

        if (dir > 0)
        {
            rightPowerPercent = upPowerPercent;
            leftPowerPercent = upPowerPercent;
        }
        else if (dir < 0)
        {
            rightPowerPercent = downPowerPercent;
            leftPowerPercent = downPowerPercent;
        }

        if (tilt > 0)
        {
            leftPowerPercent += tiltPowerPercent;
        }
        else if (tilt < 0)
        {
            rightPowerPercent += tiltPowerPercent;
        }
        else
        {
            if (!isLanded && isStabilizeEnabled)
            {
                Stabilize();
                MoveDamping();
            }
        }

        SetLeftJetsPower(leftPowerPercent);
        SetRightJetsPower(rightPowerPercent);
    }

    public void SetLanded(bool state)
    {
        isLanded = state;
        ResetAllJets();
    }

    public void SetOutOfFuel(bool state)
    {
        isOutOfFuel = state;
    }

    public void SetChargerConnected(bool state)
    {
        if (state && OnChargeEnebled != null)
        {
            OnChargeEnebled();
        }
        else if (!state && OnChargeDisabled != null)
        {
            OnChargeDisabled();
        }
    }

    public void ResetJetsPower()
    {
        currentJetsPower = maxJetsPower;
    }

    public Vector3 GetSpeedVector()
    {
        return objectRigidbody.velocity;
    }

    public float GetSpeedValue()
    {
        return objectRigidbody.velocity.magnitude;
    }

    /// <summary>
    /// Замедляет скорость корабля если у него отсутствует вращение и у rb отсутствует уголовая скорость.
    /// Если скорость будет меньше 1, то корабль остановится
    /// </summary>
    private void MoveDamping()
    {
        if (transform.rotation.z == 0 && objectRigidbody.angularVelocity == Vector3.zero)
        {
            if (objectRigidbody.velocity.x > 1)
            {
                objectRigidbody.velocity = new Vector3(objectRigidbody.velocity.x - Time.deltaTime*2, objectRigidbody.velocity.y, 0);
            }
            else if (objectRigidbody.velocity.x < -1)
            {
                objectRigidbody.velocity = new Vector3(objectRigidbody.velocity.x + Time.deltaTime*2, objectRigidbody.velocity.y, 0);
            }
            else
            {
                objectRigidbody.velocity = new Vector3(0, objectRigidbody.velocity.y, 0);
            }
        }
    }

    /// <summary>
    /// Стабилизирует вращение корабля. Если угол наклона по оси z больше stabilizeStartAngle.
    /// Если корабль находится в режиме стабилизации, то для обратной компенсации задает силу в обратную сторону.
    /// Если угол становится небольшим, то совсем останавливает вращение.
    /// </summary>
    private void Stabilize()
    {

        if (transform.rotation.z > stabilizeStartAngle)
        {
            currentStabilizeState = StabilizeState.Left;
            leftPowerPercent += Mathf.Clamp(transform.rotation.z / 6, stabilizeStartPower, transform.rotation.z);
        }
        else if (transform.rotation.z < -stabilizeStartAngle)
        {
            currentStabilizeState = StabilizeState.Right;
            rightPowerPercent += Mathf.Clamp(transform.rotation.z / 6, stabilizeStartPower, transform.rotation.z);
        }
        else if (transform.rotation.z > 0 && transform.rotation.z < stabilizeStartAngle && currentStabilizeState == StabilizeState.Left)
        {
            rightPowerPercent += stabilizeStartPower * 15;
            currentStabilizeState = StabilizeState.None;
        }
        else if (transform.rotation.z < 0 && transform.rotation.z > -stabilizeStartAngle && currentStabilizeState == StabilizeState.Right)
        {
            leftPowerPercent += stabilizeStartPower * 15;
            currentStabilizeState = StabilizeState.None;
        }
        else if (
            transform.rotation.z < stabilizeStartAngle / 5
            && transform.rotation.z > -stabilizeStartAngle / 5
            && objectRigidbody.angularVelocity.z < 0.4f
        )
        {
            objectRigidbody.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Сбрасывает скорость всех двигателей
    /// </summary>
    private void ResetAllJets()
    {
        leftJets.velocity = Vector3.zero;
        rightJets.velocity = Vector3.zero;
    }

    /// <summary>
    /// Задает силу левому двигателю
    /// </summary>
    /// <param name="powerPercent">Процент от максимального (от 0 до 1)</param>
    private void SetLeftJetsPower(float powerPercent)
    {
        if (isOutOfFuel)
        {
            SetLeftJetsFire(0);
            return;
        }

        SetLeftJetsFire(powerPercent);
        float power = currentJetsPower * powerPercent;
        OnJetsWork?.Invoke(power, JetSide.Left);
        leftJets.AddRelativeForce(Vector3.up * power);
    }

    /// <summary>
    /// Задает силу правому двигателю
    /// </summary>
    /// <param name="powerPercent">Процент от максимального (от 0 до 1)</param>
    private void SetRightJetsPower(float powerPercent)
    {
        if (isOutOfFuel)
        {
            SetRightJetsFire(0);
            return;
        }

        SetRightJetsFire(powerPercent);
        float power = currentJetsPower * powerPercent;
        OnJetsWork?.Invoke(power, JetSide.Right);
        rightJets.AddRelativeForce(Vector3.up * power);
    }

    private void SetLeftJetsFire(float percent)
    {
        SetJetEffectRate(leftFrontJetsFire, percent);
        SetJetEffectRate(leftRearJetsFire, percent);
    }

    private void SetRightJetsFire(float percent)
    {
        SetJetEffectRate(rightFrontJetsFire, percent);
        SetJetEffectRate(rightRearJetsFire, percent);
    }

    private void SetJetEffectRate(ParticleSystem jet, float rate)
    {
        var main = jet.main;
        main.startSpeedMultiplier = rate * 12f + 3;
        var emission = jet.emission;
        emission.rateOverTimeMultiplier = rate * 100 + 50;
    }


    public override string ToString()
    {
        return "LV: " + leftJets.velocity + " - RV: " + rightJets.velocity;
    }

}
