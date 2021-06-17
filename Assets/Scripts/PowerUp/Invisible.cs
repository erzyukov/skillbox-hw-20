using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Усиление невидимость
/// </summary>
public class Invisible : IPowerUp
{
    public GameObject Target { get; set; }
    public UFOEffect Effect { get; set; }

    public PowerUpBehaviour Behaviour { get; set; }

    public bool Enabled { get; set; }

    private UFOFuelContorller targetFC;
    private UFOJetsController targetJC;
    private float energyConsumption;
    private float jetsPower;

    public Invisible(float energyConsumption, float JetsPower)
    {
        Behaviour = PowerUpBehaviour.Usable;
        Enabled = false;
        this.energyConsumption = energyConsumption;
        this.jetsPower = JetsPower;
    }

    public void PickUp(GameObject target, UFOEffect effect)
    {
        Target = target;
        Effect = effect;
        targetFC = target.GetComponent<UFOFuelContorller>();
        targetJC = target.GetComponent<UFOJetsController>();
        targetFC.OnOutOfFuel += OnOutOfFuel;
    }

    public void Enable()
    {
        if (!targetFC.OutOfFuel)
        {
            Enabled = true;
            Effect.UpdateState(true);
            targetJC.JetsPower = jetsPower;
            targetFC.AddConsumtion(energyConsumption);
        }
    }

    public void Disable()
    {
        Enabled = false;
        Effect.UpdateState(false);
        targetJC.ResetJetsPower();
        targetFC.RemoveConsumtion(energyConsumption);
    }

    private void OnOutOfFuel()
    {
        Disable();
    }
}
