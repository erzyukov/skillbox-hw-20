using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Компонент установки чекпоинта при срабатывании триггера
/// </summary>
public class SetCheckPoint : MonoBehaviour
{
    [SerializeField] private Transform checkPoint = default;
    [SerializeField] private Material activeMaterial = default;
    [SerializeField] private string lampTag = "PlatformLamp";

    private Material defaultMaterial = null;
    private bool isActive = false;
    private Renderer[] lamps;

    private void Awake()
    {
        List<Renderer> list = new List<Renderer>(gameObject.GetComponentsInChildren<Renderer>());
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (!list[i].CompareTag(lampTag))
            {
                list.RemoveAt(i);
            }
        }
        lamps = list.ToArray();
        if (lamps[0] != null)
        {
            defaultMaterial = lamps[0].material;
        }
    }

    /// <summary>
    /// Активация чекпоита
    /// </summary>
    public void Activate()
    {
        if (!isActive)
        {
            DeactivatePrevious();
            GameManager.instance.saves.CheckPoint = checkPoint.position;
            isActive = true;
            foreach(Renderer lamp in lamps)
            {
                lamp.material = activeMaterial;
            }
        }
    }

    /// <summary>
    /// Деактивация платформы
    /// </summary>
    public void Deactivate()
    {
        if (isActive)
        {
            isActive = false;
            foreach (Renderer lamp in lamps)
            {
                lamp.material = defaultMaterial;
            }
        }
    }

    /// <summary>
    /// Деактивирует все платформы
    /// </summary>
    private void DeactivatePrevious()
    {
        SetCheckPoint[] points = GetComponents<SetCheckPoint>();
        foreach (SetCheckPoint point in points)
        {
            point.Deactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerRacks") && checkPoint != default)
        {
            Activate();
        }
    }


}
