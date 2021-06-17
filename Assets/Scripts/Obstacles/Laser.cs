using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс отображения луча лазера
/// </summary>
public class Laser
{
    private LineRenderer beam;
    private Transform laser;

    public Laser(GameObject laser, Material material, float width)
    {
        // Инициируем луч лазера
        this.laser = laser.transform;
        beam = laser.AddComponent(typeof(LineRenderer)) as LineRenderer;
        beam.enabled = false;
        beam.material = material;
        beam.startWidth = width;
        beam.generateLightingData = true;

        beam.SetPosition(0, laser.transform.position);
        beam.SetPosition(1, laser.transform.position);
    }

    public void On()
    {
        beam.enabled = true;
    }

    public void Off()
    {
        beam.enabled = false;
    }

    /// <summary>
    /// Выставляет длину луча лазера
    /// </summary>
    /// <param name="length">Длина</param>
    public void UpdatePosition(float length)
    {
        beam.SetPosition(0, laser.transform.position);
        Vector3 beamTargetPoint = laser.position + laser.forward * length;
        beam.SetPosition(1, beamTargetPoint);
    }

}