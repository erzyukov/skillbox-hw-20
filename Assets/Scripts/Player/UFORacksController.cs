using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс обработки состояния взлета и приземления
/// </summary>
public class UFORacksController : MonoBehaviour
{
    /// <summary>
    /// Класс управления двигателем
    /// </summary>
    [SerializeField] private UFOJetsController jets = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("LandingPlatform"))
        {
            jets.SetLanded(true);
            jets.SetChargerConnected(true);
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            GameManager.instance.messages.Victory();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("LandingPlatform"))
        {
            jets.SetLanded(false);
            jets.SetChargerConnected(false);
        }
    }


}
