using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Усиление стабилизация. 
/// </summary>
public class Stabilize : IPowerUp
{
    public GameObject Target { get; set; }
    public UFOEffect Effect { get; set; }

    public PowerUpBehaviour Behaviour { get; set; }

    public bool Enabled { get; set; }

    private float jetsPower;
    private UFOJetsController targetJC;

    public Stabilize(float JetsPower)
    {
        Behaviour = PowerUpBehaviour.Permanent;
        Enabled = false;
        this.jetsPower = JetsPower;
    }

    public void PickUp(GameObject target, UFOEffect effect)
    {
        Target = target;
        Effect = effect;
        targetJC = target.GetComponent<UFOJetsController>();
        Enable();
    }

    public void Enable()
    {
        Effect.UpdateState(true);
        targetJC.JetsPower = jetsPower;
        targetJC.StabilizeEnabled = true;
    }

    public void Disable()
    {
        Effect.UpdateState(false);
        targetJC.ResetJetsPower();
        targetJC.StabilizeEnabled = false;
    }

}
