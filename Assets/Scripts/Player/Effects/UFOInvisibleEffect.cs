using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Компонент контроля эффекта усиления невидимость
/// </summary>
public class UFOInvisibleEffect : UFOEffect
{
    private MeshRenderer ufoMeshRender = null;
    private Material ufoInitialMaterial = null;
    private Material ufoInvisibleMaterial = null;

    private void Awake()
    {
        ufoMeshRender = GetComponent<MeshRenderer>();
        ufoInitialMaterial = ufoMeshRender.material;
    }

    public void InitEffects(GameObject effect, Material ufoInvisibleMaterial)
    {
        this.effect = effect;
        this.ufoInvisibleMaterial = ufoInvisibleMaterial;
    }

    public override void UpdateState(bool state)
    {
        base.UpdateState(state);
        ufoMeshRender.material = (state)? ufoInvisibleMaterial: ufoInitialMaterial;
    }

}
