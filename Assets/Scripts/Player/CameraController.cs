using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    [SerializeField] private float maxDeltaY = 0;
    [SerializeField] private float maxDeltaX = 0;

    private void Start()
    {
        if (target)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
    }

    private void Update()
    {
        if (target)
        {
            float moveX = transform.position.x;
            if (target.position.x > transform.position.x + maxDeltaX)
            {
                moveX = target.position.x - maxDeltaX;
            }
            else if (target.position.x < transform.position.x - maxDeltaX)
            {
                moveX = target.position.x + maxDeltaX;
            }

            float moveY = transform.position.y;
            if (target.position.y > transform.position.y + maxDeltaY)
            {
                moveY = target.position.y - maxDeltaY;
            }
            else if (target.position.y < transform.position.y - maxDeltaY)
            {
                moveY = target.position.y + maxDeltaY;
            }

            transform.position = new Vector3(moveX, moveY, transform.position.z);
        }
    }

}
