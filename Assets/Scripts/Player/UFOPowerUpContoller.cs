using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс управляющий усилениями
/// </summary>
public class UFOPowerUpContoller : MonoBehaviour
{
    public delegate void PickUpAction(PowerUpItem.Type type);
    public event PickUpAction OnPickedUp;
    public delegate void UseAction();
    public event UseAction OnUse;
    public event UseAction OnTurnOn;
    public event UseAction OnTurnOff;

    [Header("UFO Power Up Effects")]
    [SerializeField] private GameObject effectForceFeild = null;
    [SerializeField] private GameObject effectStabilize = null;
    [SerializeField] private GameObject effectLantern = null;
    [SerializeField] private GameObject effectInvisible = null;
    [SerializeField] private Material ufoInvisibleMaterial = null;

    [Space]
    [SerializeField] private AudioSource pickUpSound = default;
    [SerializeField] private AudioSource turnOnSound = default;
    [SerializeField] private AudioSource turnOffSound = default;

    private IPowerUp currentPowerUp = null;
    private UFOEffect currtneEffect = null;
    //private PowerUpItem.Type currentPowerUpType;

    /// <summary>
    /// Использовать усиление (повторное использование отключает)
    /// </summary>
    public void Use()
    {
        Debug.Log("Use !!!!");
        if (currentPowerUp != null && currentPowerUp.Behaviour != PowerUpBehaviour.Permanent)
        {
            if (currentPowerUp.Enabled == false)
            {
                currentPowerUp.Enable();
                // если усилитель одноразовый, то после использования удаляем его
                if (currentPowerUp.Behaviour == PowerUpBehaviour.Disposable)
                {
                    currentPowerUp = null;
                    OnUse?.Invoke();
                }
                else if (turnOnSound)
                {
                    OnTurnOn?.Invoke();
                    turnOnSound.Play();
                }
            }
            else
            {
                currentPowerUp.Disable();
                OnTurnOff?.Invoke();
                if (turnOffSound)
                {
                    turnOffSound.Play();
                }
            }
        }
    }

    public bool IsForceFieldActive()
    {
        if (currentPowerUp == null || currentPowerUp.GetType() != typeof(ForceField))
        {
            return false;
        }
        return currentPowerUp.Enabled;
    }

    public void ResetPowerUp()
    {
        if (currentPowerUp != null)
        {
            currentPowerUp.Disable();
            currentPowerUp = null;
        }
        if (currtneEffect != null)
        {
            currtneEffect.UpdateState(false);
            Destroy(currtneEffect);
            currtneEffect = null;
        }
        OnUse?.Invoke();
    }

    /// <summary>
    /// Подобрать усиление
    /// </summary>
    /// <param name="powerUpObject">объект усиления</param>
    private void PickUp(GameObject powerUpObject)
    {
        if (pickUpSound)
        {
            pickUpSound.Play();
        }

        PowerUpItem powerUpItem = powerUpObject.GetComponent<PowerUpItem>();
        IPowerUp powerUp = powerUpItem.PowerUp;

        // добавляем компонент эфекта усиления на объект игрока
        UFOEffect effect = AttachEffectComponent(powerUpItem.type);

        // если уселитель не самоактивирующийся, то заменяем им имеющийся
        if (powerUp.Behaviour != PowerUpBehaviour.SelfActivating)
        {
            OnPickedUp?.Invoke(powerUpItem.type);

            if (currentPowerUp != null)
            {
                currentPowerUp.Disable();
                currentPowerUp = null;
            }
            if (currtneEffect != null)
            {
                currtneEffect.UpdateState(false);
                Destroy(currtneEffect);
                currtneEffect = null;
            }

            currtneEffect = effect;
            currentPowerUp = powerUp;
        }
        
        if (powerUp.Behaviour == PowerUpBehaviour.Permanent)
        {
            OnTurnOn?.Invoke();
        }

        // поднимаем усиление
        powerUp.PickUp(gameObject, effect);

        powerUpItem.TakeItemHandler();
    }

    /// <summary>
    /// Добавляет на объект игрока компонент эффекта усиления. Возвращает ссылку на этот компонент.
    /// </summary>
    /// <param name="type">Тип усиления</param>
    /// <returns>Ссылка на компонент усиления</returns>
    private UFOEffect AttachEffectComponent(PowerUpItem.Type type)
    {
        switch (type)
        {
            case PowerUpItem.Type.ForceFeld:
                UFOForceFieldEffect ffe = gameObject.AddComponent<UFOForceFieldEffect>();
                ffe.InitEffects(effectForceFeild);
                return ffe;
            case PowerUpItem.Type.Stabilize:
                UFOStabilizeEffect se = gameObject.AddComponent<UFOStabilizeEffect>();
                se.InitEffects(effectStabilize);
                return se;
            case PowerUpItem.Type.Lantern:
                UFOLanternEffect le = gameObject.AddComponent<UFOLanternEffect>();
                le.InitEffects(effectLantern);
                return le;
            case PowerUpItem.Type.Invisible:
                UFOInvisibleEffect ie = gameObject.AddComponent<UFOInvisibleEffect>();
                ie.InitEffects(effectInvisible, ufoInvisibleMaterial);
                return ie;
            default:
                return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            PickUp(other.gameObject);
        }
    }

}
