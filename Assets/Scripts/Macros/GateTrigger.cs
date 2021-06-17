using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Открывает заданный шлюз, по срабатыванию триггера
/// </summary>
public class GateTrigger : MonoBehaviour
{
    /// <summary>
    /// Ссылка на компонент управления шлюзом
    /// </summary>
    [SerializeField] private GateController gc = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerRacks"))
        {
            gc.OpenGate();
        }
    }

}
