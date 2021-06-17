using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Устанавливает заданное максимальное количество топлива у игрока по срабатыванию триггера
/// </summary>
public class SetFuelCapacity : MonoBehaviour
{
    [SerializeField] private float FuelAmount = 0;
    [SerializeField] private UFOFuelContorller fc = null;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerRacks"))
        {
            fc.SetCurrentMaxCapacity(FuelAmount);
            Destroy(this);
        }
    }

}
