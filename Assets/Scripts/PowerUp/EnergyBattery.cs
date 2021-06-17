using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Усилитель батарея энергии. При активации добавляет цели топлива.
/// </summary>
public class EnergyBattery : IPowerUp
{
    public GameObject Target { get; set; }
    public UFOEffect Effect { get; set; }

    public PowerUpBehaviour Behaviour { get; set; }

    public bool Enabled { get; set; }

    private UFOFuelContorller targetFC;
    private float energyAmount;

    public EnergyBattery(float energyAmount)
    {
        Behaviour = PowerUpBehaviour.Disposable;
        Enabled = false;
        this.energyAmount = energyAmount;
    }

    public void PickUp(GameObject target, UFOEffect effect = null)
    {
        Target = target;
        targetFC = target.GetComponent<UFOFuelContorller>();
    }

    public void Enable()
    {
        targetFC.AddFuel(energyAmount);
    }

    public void Disable()
    {

    }


}
