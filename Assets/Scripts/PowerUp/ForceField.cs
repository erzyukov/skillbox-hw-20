using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Усиление - силовое поле. Создает вокруг цели силовое поле. Которое дает неуезвимость от заданных повреждений. При активации потребляет топливо.
/// </summary>
public class ForceField : IPowerUp
{
    public PowerUpBehaviour Behaviour { get; set; }
    public GameObject Target { get; set; }
    public bool Enabled { get; set; }
    public UFOEffect Effect { get; set; }

    private UFOLifeController targetCC;
    private UFOFuelContorller targetFC;
    private float energyConsumption;

    public ForceField(float energyConsumption)
    {
        Behaviour = PowerUpBehaviour.Usable;
        Enabled = false;
        this.energyConsumption = energyConsumption;
    }

    public void PickUp(GameObject target, UFOEffect effect)
    {
        Target = target;
        Effect = effect;
        targetCC = target.GetComponent<UFOLifeController>();
        targetFC = target.GetComponent<UFOFuelContorller>();
        targetFC.OnOutOfFuel += OnOutOfFuel;
    }

    public void Enable()
    {
        if (!targetFC.OutOfFuel)
        {
            Enabled = true;
            Effect.UpdateState(true);
            targetCC.Immortal = true;
            targetFC.AddConsumtion(energyConsumption);
        }
    }

    public void Disable()
    {
        Enabled = false;
        Effect.UpdateState(false);
        targetCC.Immortal = false;
        targetFC.RemoveConsumtion(energyConsumption);
    }

    private void OnOutOfFuel()
    {
        Disable();
    }

}
