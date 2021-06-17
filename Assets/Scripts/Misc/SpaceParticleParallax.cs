using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceParticleParallax : MonoBehaviour
{
    [SerializeField] Transform target = default;
    [SerializeField] float maxPositionOffset = 10;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        lookAtTarget();
    }

    private void Update()
    {
        float xOffset = -(Input.mousePosition.x - (Screen.width / 2)) * maxPositionOffset / (Screen.width / 2);
        float yOffset = -(Input.mousePosition.y - (Screen.height / 2)) * maxPositionOffset / (Screen.height / 2);

        transform.position = startPosition + new Vector3(2 * xOffset, yOffset);

        lookAtTarget();
    }

    private void lookAtTarget()
    {
        transform.LookAt(target);
    }

}
