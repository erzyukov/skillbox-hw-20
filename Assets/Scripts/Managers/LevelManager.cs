using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Компонент управления объектами сцены
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject powerUpContainer = default;

    private PowerUpItem[] powerUpItems;

    private void Start()
    {
        powerUpItems = powerUpContainer.GetComponentsInChildren<PowerUpItem>();
    }

    /// <summary>
    /// Возвращает всем необходимым объектам изначальное состояние при сбросе уровня (без перезагрузки сценария)
    /// </summary>
    public void ResetLevel()
    {
        foreach(PowerUpItem item in powerUpItems)
        {
            item.ResetItemObject();
        }
    }
}
