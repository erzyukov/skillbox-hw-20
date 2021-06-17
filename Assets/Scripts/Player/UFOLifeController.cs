using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс управления жизнью игрока
/// </summary>
public class UFOLifeController : MonoBehaviour
{
    [SerializeField] private UFODestroyHandler explosion = null;
    /// <summary>
    /// Количество затрачиваемой энергии (в единицах мощьности) при попадании тяжелого оружия
    /// </summary>
    [SerializeField] private float heavyWeaponDamageEnergyPower = 900000;
    /// <summary>
    /// Количество затрачиваемой энергии (в единицах мощьности) при попадании легкого оружия
    /// </summary>
    [SerializeField] private float lightWeaponDamageEnergyPower = 100000;
    [Space]
    /// <summary>
    /// Ссылка на компонент UFOFuelContorller
    /// </summary>
    [SerializeField] private UFOFuelContorller fuel = default;
    /// <summary>
    /// Является ли корабль бессмертным
    /// </summary>
    public bool Immortal { get { return isImmortal; } set { isImmortal = value; } }

    private bool isImmortal;
    private bool isDead = false;

    /// <summary>
    /// Функция должна вызываться когда игрок получает повреждения
    /// Тип повреждений влияет на поведение усилений
    /// </summary>
    /// <param name="type">Тип повреждения</param>
    public void GetDamage(DamageType type)
    {
        if (isDead)
        {
            return;
        }

        // если на игроке висит щит, то мы тратим энергию
        UFOPowerUpContoller pu = GetComponent<UFOPowerUpContoller>();
        if (fuel != null && pu != null && pu.IsForceFieldActive())
        {
            switch (type)
            {
                case DamageType.LightWeapon:
                    fuel.RemoveFuel(lightWeaponDamageEnergyPower);
                    return;
                case DamageType.HeavyWeapon:
                    fuel.RemoveFuel(heavyWeaponDamageEnergyPower);
                    return;
                default:
                    break;
            }
        }

        // если кончилось топливо - то взрыва не будет
        if (fuel != null && fuel.OutOfFuel)
        {
            explosion.Crash();
        }
        else
        {
            explosion.Explode();
        }

        isDead = true;
        Invoke("OnPlayerDeath", 3f);
    }

    public void ResetState()
    {
        isDead = false;
    }
    private void OnPlayerDeath()
    {
        GameManager.instance.PlayerDeathHandler();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stage")) // && !isImmortal
        {
            GetDamage(DamageType.Сollision);
            //SceneManager.LoadScene("Level-1");
        }
    }


}

/// <summary>
/// Возможные типы повреждений
/// LightWeapon - Легкое оружие
/// HeavyWeapon - Тяжелое оружие
/// Сollision   - Столкновения
/// </summary>
public enum DamageType { LightWeapon, HeavyWeapon, Сollision }
