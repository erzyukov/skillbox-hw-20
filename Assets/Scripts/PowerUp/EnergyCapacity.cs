using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Усилитель вместимости энергии. При взятии - повышает максимальную вместимость топлива.
/// </summary>
public class EnergyCapacity : IPowerUp
{

    public GameObject Target { get; set; }
    public UFOEffect Effect { get; set; }

    public PowerUpBehaviour Behaviour { get; set; }

    public bool Enabled { get; set; }

    private UFOFuelContorller targetFC;
    private float energyAmount;

    public EnergyCapacity(float energyAmount)
    {
        Behaviour = PowerUpBehaviour.SelfActivating;
        Enabled = false;
        this.energyAmount = energyAmount;
    }

    public void PickUp(GameObject target, UFOEffect effect = null)
    {
        Target = target;
        targetFC = target.GetComponent<UFOFuelContorller>();
        targetFC.RaiseCapacity(energyAmount);
    }

    public void Enable()
    {
    }

    public void Disable()
    {
    }



}
