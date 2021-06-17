using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Компонент контроля эффекта усиления фонарик
/// </summary>
public class UFOLanternEffect : UFOEffect
{

    void Update()
    {
        if (isEnabled)
        {
            Plane plane = new Plane(Vector3.back, transform.position);
            Vector3 worldPosition;
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                worldPosition = ray.GetPoint(distance);

                Vector3 target = worldPosition - transform.position;

                float angle = Vector3.Angle(Vector3.right, target);
                if (target.y < 0)
                    angle *= -1;

                effect.transform.rotation = Quaternion.Euler(-angle, 90, 0);
            }
        }
    }

}
