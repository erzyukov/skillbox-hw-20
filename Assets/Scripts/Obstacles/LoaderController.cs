using UnityEngine;

/// <summary>
/// Компонент управления погрузчиком
/// </summary>
public class LoaderController : MoveableByPoints
{
    public Transform LongitudinalRail { get { return longitudinalRail; } }
    public Transform TransverseRail { get { return transverseRail; } }
    public Transform Slider { get { return slider; } }
    public Transform Rope { get { return rope; } }
    public Transform Hook { get { return hook; } }
    public float MaxHeight { get { return maxHeight; } }

    /// <summary>
    /// Позиция груза по X в процентах от 0 до 1
    /// </summary>
    [Range(0f, 1f)]
    [SerializeField] private float xPosition = 0;
    /// <summary>
    /// Позиция груза по Y в процентах от 0 до 1
    /// </summary>
    [Range(0f, 1f)]
    [SerializeField] private float yPosition = 0;
    /// <summary>
    /// Позиция груза по Z в процентах от 0 до 1
    /// </summary>
    [Range(0f, 1f)]
    [SerializeField] private float zPosition = 0;

    [Space]
    /// <summary>
    /// Максимальная вертикальная дистанция перемещения зацепа
    /// </summary>
    [SerializeField] private float maxHeight = 21;

    [Space]
    /// <summary>
    /// Продольная рельса
    /// </summary>
    [SerializeField] private Transform longitudinalRail = default;
    //[SerializeField] private Renderer lr = default;
    /// <summary>
    /// Поперечная рельса
    /// </summary>
    [SerializeField] private Transform transverseRail = default;
    /// <summary>
    /// Передвижной блок поперечной рельсы
    /// </summary>
    [SerializeField] private Transform slider = default;
    /// <summary>
    /// Трос
    /// </summary>
    [SerializeField] private Transform rope = default;
    /// <summary>
    /// Зацеп
    /// </summary>
    [SerializeField] private Transform hook = default;

    [Space]
    [SerializeField] private AudioSource movementSound = default;

    /// <summary>
    /// Длина продольной рельсы
    /// </summary>
    private float longitudinalRailLength;
    /// <summary>
    /// Длина поперечной рельсы
    /// </summary>
    private float transverseRailLength;
    /// <summary>
    /// Ширина поперечной рельсы
    /// </summary>
    private float transverseRailWidth;
    /// <summary>
    /// Ширина передвижного блока поперечной рельсы
    /// </summary>
    private float sliderWidth;
    /// <summary>
    /// Изначальная длина троса
    /// </summary>
    private float ropeLength;

    /// <summary>
    /// Начальная минимальная позиция зацепа
    /// </summary>
    private float minYHookPosition;

    /// <summary>
    /// Центр продольной рельсы
    /// </summary>
    private Vector3 longitudinalRailCenter;
    /// <summary>
    /// Центр поперечной рельсы
    /// </summary>
    private Vector3 transverseRailCenter;

    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    private void Awake()
    {
        // определяем начальные значения необходимые для вычисления конечного положения зацепа
        Renderer lr = longitudinalRail.GetComponent<Renderer>();
        Renderer tr = transverseRail.GetComponent<Renderer>();
        longitudinalRailLength = lr.bounds.size.x;
        transverseRailLength = tr.bounds.size.z;
        transverseRailWidth = tr.bounds.size.x;
        longitudinalRailCenter = lr.bounds.center;
        transverseRailCenter = tr.bounds.center;

        Renderer sr = slider.GetComponent<Renderer>();
        sliderWidth = sr.bounds.size.z;

        Renderer rr = rope.GetComponent<Renderer>();
        ropeLength = rr.bounds.size.y / rope.localScale.y;

        minYHookPosition = -ropeLength; // hook.localPosition.y;

        minX = longitudinalRailCenter.x - longitudinalRailLength / 2 + transverseRailWidth;
        maxX = longitudinalRailCenter.x + longitudinalRailLength / 2 - transverseRailWidth;
        minZ = transverseRailCenter.z - transverseRailLength / 2 + sliderWidth;
        maxZ = transverseRailCenter.z + transverseRailLength / 2 - sliderWidth;

        UpdateHookPosition();
    }

    private void FixedUpdate()
    {
        UpdateHookPosition();
    }

    /// <summary>
    /// Устанавливает заданную глобальную позицию, переводя ее в процентное выражение
    /// </summary>
    /// <param name="position">Глобальная позиция</param>
    public override void SetPosition(Vector3 position)
    {
        xPosition = (position.x - minX) / (maxX - minX);
        zPosition = (position.z - minZ) / (maxZ - minZ);

        float globalYMin = rope.position.y - ropeLength;
        yPosition = (position.y - globalYMin) / (- maxHeight + ropeLength);
    }

    public override void StartMovementHandler()
    {
        if (movementSound)
        {
            movementSound.Play();
        }
    }

    public override bool IsAllowToMove()
    {
        return true;
    }

    /// <summary>
    /// Обновляет положение зацепа
    /// </summary>
    private void UpdateHookPosition()
    {
        // определяем положение поперечной рельсы в зависимости от выставленного положения xPosition
        Vector3 trp = transverseRail.position;
        transverseRail.position = new Vector3((maxX - minX) * xPosition + minX, trp.y, trp.z);

        // определяем положение блока поперечной рельсы в зависимости от выставленного положения zPosition
        Vector3 sp = slider.position;
        slider.position = new Vector3(sp.x, sp.y, (maxZ - minZ) * zPosition + minZ);

        // определяем положение зацепа в зависимости от выставленного положения yPosition
        Vector3 hp = hook.localPosition;
        hook.localPosition = new Vector3(hp.x, ((- maxHeight - minYHookPosition) * yPosition) + minYHookPosition, hp.z);

        // определяем масштаб троса в зависимости от положения зацепа
        float currentDistance = Mathf.Abs(hp.y - minYHookPosition);
        rope.localScale = new Vector3(1, (currentDistance + ropeLength) / ropeLength, 1);
    }


}
