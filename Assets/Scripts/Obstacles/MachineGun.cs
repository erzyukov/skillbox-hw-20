using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс управления пушками
/// </summary>
public class MachineGun : MonoBehaviour
{
    /// <summary>
    /// Рама оружия - будет двигаться в горизонтальной плоскости
    /// </summary>
    [SerializeField] private Transform gunFrame = default;
    /// <summary>
    /// Тело оружия - будет двигаться в вертикальной плоскости
    /// </summary>
    [SerializeField] private Transform gunBody = default;
    /// <summary>
    /// Скорость прицеливания (поворота) ствола
    /// </summary>
    [SerializeField] private float aimSpeed = 75;

    [Space]
    /// <summary>
    /// Ссылка на компонент Animator
    /// </summary>
    [SerializeField] private Animator animator = default;

    [Space]
    [SerializeField] private AudioSource movementSound = default;

    /// <summary>
    /// Система частиц выстрела
    /// </summary>
    [Space]
    [SerializeField] private ParticleSystem[] shot = default;

    [Space]
    [SerializeField] private AudioSource shotAudio = default;

    [Space]
    [SerializeField] public DamageType damageType;

    private GameObject target;

    /// <summary>
    /// Типы состояния оружия
    /// Idle            - бездействие
    /// Arising         - появление
    /// Aim             - прицеливание
    /// Active          - активность (слежение за игроком)
    /// Deactivating    - деактивация (переход в положение перед прицеливанием)
    /// Vainshing       - исчезновение
    /// EnemyDetected   - обнаружен противник (в этом состоянии оружие стреляет по цели)
    /// </summary>
    private enum GanState { Idle, Arising, Aim, Active, Deactivating, Vainshing, EnemyDetected};
    private GanState state;

    public delegate void ActivationAction();
    public event ActivationAction OnSystemActivate;
    public event ActivationAction OnSystemDeactivate;

    private void Awake()
    {
        state = GanState.Idle;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void FixedUpdate()
    {
        // если цель есть, но она не активна (игрок погиб) - переходим в состояние деактивации
        if (target != null && !target.activeSelf && state != GanState.Deactivating)
        {
            SetDeactivatingState();
        }

        if (target != null)
        {
            bool isGunFrameRotateFinish;
            bool isGunBodyRotateFinish;

            Quaternion lookAngle = Quaternion.LookRotation(gunFrame.position - target.transform.position);
            Vector3 v = lookAngle.eulerAngles;
            // отдельно получаем углы поворота для рамы и для тела оружия
            Quaternion frameLookAngle = Quaternion.Euler(0, v.y, 0);
            Quaternion bodyLookAngle = Quaternion.Euler(v.x, v.y, 0);

            switch (state)
            {
                case GanState.Active:
                    // следим за игроком
                    gunFrame.rotation = frameLookAngle;
                    gunBody.rotation = bodyLookAngle;
                    break;
                case GanState.Aim:
                    // поворачиваем пушку в сторону игрока
                    isGunFrameRotateFinish = RotateGunPart(gunFrame, frameLookAngle);
                    isGunBodyRotateFinish = RotateGunPart(gunBody, bodyLookAngle);
                    if (isGunFrameRotateFinish && isGunBodyRotateFinish)
                    {
                        SetActiveState();
                    }
                    break;
                case GanState.Deactivating:
                    //возвращаем ствол в положение перед прицеливанием (чтобы анимация исчезновения отрабатывала как надо)
                    lookAngle = Quaternion.LookRotation(Vector3.forward);
                    isGunFrameRotateFinish = RotateGunPart(gunFrame, lookAngle);
                    isGunBodyRotateFinish = RotateGunPart(gunBody, lookAngle);
                    if (isGunFrameRotateFinish && isGunBodyRotateFinish)
                    {
                        SetVainshingState();
                    }
                    break;
            }
        }
    }

    private bool RotateGunPart(Transform part, Quaternion lookAngle)
    {
        part.rotation = Quaternion.RotateTowards(part.rotation, lookAngle, Time.fixedDeltaTime * aimSpeed);
        float angle = Quaternion.Angle(lookAngle, part.rotation);
        return (angle <= 0) ? true : false;
    }

    /// <summary>
    /// Обработка обнаружения противника
    /// </summary>
    public void TargetDetectedHandle()
    {
        state = GanState.EnemyDetected;
        animator.SetBool("isShoting", true);
    }

    /// <summary>
    /// Сделать выстрел
    /// </summary>
    public void Shot(int barrel = 0)
    {
        if (target != null)
        {
            shotAudio.Play();
            shot[barrel].Play();

            // если у цели есть система контроля жизней - передаем повреждение
            UFOLifeController player = target.GetComponent<UFOLifeController>();
            player?.GetDamage(damageType);
        }
    }

    /// <summary>
    /// Обработка потери цели
    /// </summary>
    public void TargetMissingHandle()
    {
        state = GanState.Active;
        animator.SetBool("isShoting", false);
    }

    /// <summary>
    /// Переход в состояние Idle
    /// </summary>
    public void SetIdleState()
    {
        state = GanState.Idle;
    }

    /// <summary>
    /// Переход в состояние Aim
    /// </summary>
    public void SetAimState()
    {
        state = GanState.Aim;
    }

    /// <summary>
    /// Переход в состояние Active
    /// </summary>
    public void SetActiveState()
    {
        state = GanState.Active;
    }

    /// <summary>
    /// Переход в состояние Arising
    /// </summary>
    public void SetArisingState(GameObject target)
    {
        if (movementSound)
        {
            movementSound.Play();
        }
        state = GanState.Arising;
        animator.SetTrigger("Arise");
        this.target = target;
        OnSystemActivate?.Invoke();
    }

    /// <summary>
    /// Переход в состояние Deactivating
    /// </summary>
    public void SetDeactivatingState()
    {
        if (movementSound)
        {
            movementSound.Play();
        }
        OnSystemDeactivate?.Invoke();
        state = GanState.Deactivating;
    }

    /// <summary>
    /// Переход в состояние Vainshing
    /// </summary>
    public void SetVainshingState()
    {
        state = GanState.Vainshing;
        animator.SetBool("isShoting", false);
        animator.SetTrigger("Vanish");
        target = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && state != GanState.Arising)
        {
            SetArisingState(other.gameObject);
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject);
            //target = other.gameObject;
        }
    }
    */

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && state != GanState.Deactivating)
        {
            SetDeactivatingState();
        }
    }

}
