using UnityEngine;

/// <summary>
/// Компонент управление позицией для компонента движения по точкам
/// </summary>
public class MoveByPointsController : MoveableByPoints
{
    private bool isAllowToMoove = false;

    public void UpdateState(bool state)
    {
        isAllowToMoove = state;
    }

    public override void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public override bool IsAllowToMove()
    {
        return isAllowToMoove;
    }
}
