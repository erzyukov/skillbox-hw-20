using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveableByPoints : MonoBehaviour
{
    virtual public void SetPosition(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    virtual public bool IsAllowToMove()
    {
        throw new System.NotImplementedException();
    }

    virtual public void StartMovementHandler()
    {

    }
}
