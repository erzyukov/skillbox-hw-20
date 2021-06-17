using System;
using UnityEngine;

/// <summary>
/// Компонент расчета положения объекта между точками перемещения
/// </summary>
public class MoveByPoints : MonoBehaviour
{
    /// <summary>
    /// Контроллер перемещения
    /// </summary>
    [SerializeField] private MoveableByPoints controller = default;

    [Space]
    /// <summary>
    /// Точки по которым должен двигаться груз
    /// </summary>
    [Header("Первой точкой должен быть объект перемещения")]
    [SerializeField] private Transform[] wayPointTransforms = default;
    /// <summary>
    /// Скорость передвижения груза
    /// </summary>
    [SerializeField] private float speed = 100;

    [Space]
    /// <summary>
    /// Движение по кругу
    /// </summary>
    [Header("Перемещения по кругу")]
    [SerializeField] private bool isCircleMovement = false;
    /// <summary>
    /// Движение в обратную сторону
    /// </summary>
    [Header("Только для перемещения по кругу")]
    [SerializeField] private bool isReverseMovement = false;
    /// <summary>
    /// Кривая для плавной смены скорости в зависимости от дистанции
    /// </summary>
    [SerializeField] private AnimationCurve smoothChangeDirection = default;

    private float startTime;
    private float distance;

    //private Vector3 startPosition;
    //private Vector3 endPosition;
    private Vector3[] wayPoints;

    private Vector3 currentStartPoint;
    private Vector3 currentEndPoint;

    private int pointsCount = 0;
    private int nextPointIndex = 1;

    private float fractionDistance;

    private void Awake()
    {
        if (controller == default)
        {
            Destroy(this);
        }
        wayPoints = new Vector3[wayPointTransforms.Length];
        for (int i = 0; i < wayPointTransforms.Length; i++)
        {
            wayPoints[i] = wayPointTransforms[i].position;
        }
        if (isReverseMovement && isCircleMovement)
        {
            nextPointIndex = 0;
            Array.Reverse(wayPoints);
        }
    }

    private void Start()
    {
        pointsCount = wayPoints.Length;
        if (pointsCount > 0)
        {
            controller.StartMovementHandler();
            //startPosition = wayPoints[0].position;
            //endPosition = wayPoints[pointsCount - 1].position;

            int firstPointIndex = (nextPointIndex - 1 < 0) ? pointsCount - 1: nextPointIndex - 1;

            currentStartPoint = wayPoints[firstPointIndex];
            currentEndPoint = wayPoints[nextPointIndex];

            startTime = Time.time;

            distance = Vector3.Distance(currentStartPoint, currentEndPoint);
        }
    }

    private void FixedUpdate()
    {
        if (!controller.IsAllowToMove())
        {
            startTime += Time.fixedDeltaTime;
            return;
        }

        if (pointsCount > 0 && GameManager.instance.IsActionAllow())
        {
            float smoothSpeedFactor = smoothChangeDirection.Evaluate(fractionDistance);
            float smoothSpeed = (smoothSpeedFactor != 0) ? speed * smoothSpeedFactor : speed;
            // пройденая дистанция
            float distCovered = (Time.time - startTime) * Time.fixedDeltaTime * smoothSpeed;
            // доля пройденного пути
            fractionDistance = distCovered / distance;

            // плавно перемещаем передавая данные контроллеру
            Vector3 position = Vector3.Lerp(currentStartPoint, currentEndPoint, fractionDistance);
            controller.SetPosition(position);

            // если доля больше 1, значит достигли необходимой точки
            if (Mathf.Abs(fractionDistance) >= 1)
            {
                calcNextPoint();
            }
        }
    }

    /// <summary>
    /// Рассчитываем следующую точку к которой будет двигаться лифт
    /// </summary>
    private void calcNextPoint()
    {
        nextPointIndex++;
        startTime = Time.time;
        currentStartPoint = currentEndPoint;

        // если текущая крайняя точка совпадала с конечной позицией останавливаемся и меняем направление
        if (nextPointIndex >= pointsCount)
        {
            if (isCircleMovement)
            {
                nextPointIndex = 0;
            }
            else
            {
                nextPointIndex = 1;
                Array.Reverse(wayPoints);
            }
        }

        currentEndPoint = wayPoints[nextPointIndex];
        distance = Vector3.Distance(currentStartPoint, currentEndPoint);
    }

    private void OnDrawGizmos()
    {
        if (wayPoints != null && wayPoints.Length > 0)
        {
            foreach (Vector3 point in wayPoints)
            {
                if (point != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(point, .5f);
                }
            }
        }
        else
        {
            if (wayPointTransforms != null && wayPointTransforms.Length != 0)
            {
                foreach (Transform point in wayPointTransforms)
                {
                    if (point != null)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(point.position, .5f);
                    }
                }
            }
        }
    }


}
