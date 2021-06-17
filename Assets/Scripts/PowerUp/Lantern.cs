using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Усиление фонарь
/// </summary>
public class Lantern : IPowerUp
{
    public GameObject Target { get; set; }
    public UFOEffect Effect { get; set; }

    public PowerUpBehaviour Behaviour { get; set; }

    public bool Enabled { get; set; }

    private UFOFuelContorller targetFC;
    private float energyConsumption;

    public Lantern(float energyConsumption)
    {
        Behaviour = PowerUpBehaviour.Usable;
        Enabled = false;
        this.energyConsumption = energyConsumption;
    }

    public void PickUp(GameObject target, UFOEffect effect)
    {
        Target = target;
        Effect = effect;
        targetFC = target.GetComponent<UFOFuelContorller>();
        targetFC.OnOutOfFuel += OnOutOfFuel;
    }

    public void Enable()
    {
        if (!targetFC.OutOfFuel)
        {
            Enabled = true;
            Effect.UpdateState(true);
            targetFC.AddConsumtion(energyConsumption);
        }
    }

    public void Disable()
    {
        Enabled = false;
        Effect.UpdateState(false);
        targetFC.RemoveConsumtion(energyConsumption);
    }

    private void OnOutOfFuel()
    {
        Disable();
    }

}
