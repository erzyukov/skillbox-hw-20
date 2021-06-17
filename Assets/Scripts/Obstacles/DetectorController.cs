using UnityEngine;

/// <summary>
/// Класс обнаружения противника с помощью лучей лазера
/// Используется для передачи данных о противнике в класс MachineGun, с помощью событий
/// </summary>
[RequireComponent(typeof(MachineGun))]
public class DetectorController : MonoBehaviour
{
    /// <summary>
    /// Тип детектора
    /// </summary>
    public DetectorType type = DetectorType.Default;

    /// <summary>
    /// Ссылка на компонент MachineGun
    /// </summary>
    [SerializeField] private MachineGun gun = default;

    [Header("Lasers settings")]
    /// <summary>
    /// Объекты которые будут излучать луч лазера
    /// </summary>
    [SerializeField] private GameObject[] laserObjects = null;
    /// <summary>
    /// Материал для луча лазера
    /// </summary>
    [HideInInspector] public Material laserBeamMaterial = null;
    /// <summary>
    /// Максимальная длина луча лазера
    /// </summary>
    [SerializeField] private float maxBeamLength = 30;

    /// <summary>
    /// Тип детектора
    /// Default     - обычный
    /// TrueSight   - видит невидимые объекты
    /// </summary>
    public enum DetectorType { Default, TrueSight }

    /// <summary>
    /// Состояние детектора
    /// Off             - выключен
    /// Activating      - активация
    /// On              - включен
    /// EnemyDetected   - обнаружен противник
    /// </summary>
    private enum DetectorState {Off, Activating, On, EnemyDetected};

    private Laser[] lasers;
    private DetectorState state;

    private MoveByPointsController[] laserMoveControllers;

    private void Awake()
    {
        state = DetectorState.Off;
        int count = laserObjects.Length;
        // инициализируем лазеры
        lasers = new Laser[count];
        laserMoveControllers = new MoveByPointsController[count];
        for (int i = 0; i < count; i++)
        {
            lasers[i] = new Laser(laserObjects[i], laserBeamMaterial, 0.25f);
            laserMoveControllers[i] = laserObjects[i].GetComponent<MoveByPointsController>();
        }
    }

    private void Start()
    {
        gun.OnSystemActivate += OnSystemActivate;
        gun.OnSystemDeactivate += OnSystemDeactivate;
    }

    void Update()
    {
        //Debug.Log(state);
        if (state != DetectorState.Off) {
            int count = laserObjects.Length;

            if (state == DetectorState.Activating)
            {
                // если детектор в состоянии активации - задаем лучам длинну и переводим в состояние "включен"
                for (int i = 0; i < count; i++)
                {
                    lasers[i].UpdatePosition(-maxBeamLength);
                }
                state = DetectorState.On;
            }
            else
            {

                for (int i = 0; i < count; i++)
                {
                    if (laserMoveControllers[i])
                    {
                        lasers[i].UpdatePosition(-maxBeamLength);
                    }
                }

                bool detected = false;
                for (int i = 0; i < count; i++)
                {
                    // проверяем каждым лазером - не косается ли он игрока
                    if (Physics.Raycast(laserObjects[i].transform.position, -laserObjects[i].transform.forward * 30, out RaycastHit hit, maxBeamLength))
                    {
                        if (hit.collider && hit.collider.CompareTag("Player"))
                        {
                            UFOInvisibleEffect invisibleController = hit.collider.gameObject.GetComponent<UFOInvisibleEffect>();
                            // если лазер типа TrueSight
                            // или цель не имеет компоненты невидимости
                            // или невидимость выключена
                            // - значит цель обнаружена
                            if (type == DetectorType.TrueSight || invisibleController == null || !invisibleController.Enabled)
                            {
                                lasers[i].UpdatePosition(-Vector3.Distance(hit.point, laserObjects[i].transform.position));
                                detected = true;
                            }
                        }
                    }
                }
                // если обноружен противник и система не в статусе "обнаружен противник", переходим в статус обнаружен
                if (detected && state != DetectorState.EnemyDetected)
                {
                    EnemyDetected();
                }
                // а если противник не обнаружен и система была в статусе "обнаружен противник", то переходим в статус активации
                else if (!detected && state == DetectorState.EnemyDetected)
                {
                    EnemyMissing();
                }
            }
        }
    }

    /// <summary>
    /// Активация детектора
    /// </summary>
    public void ActivateDetector()
    {
        state = DetectorState.Activating;
        int count = laserObjects.Length;
        for (int i = 0; i < count; i++)
        {
            if (laserMoveControllers[i])
            {
                laserMoveControllers[i].UpdateState(true);
            }
            lasers[i].On();
        }
    }

    /// <summary>
    /// Деактивация детектора
    /// </summary>
    public void DeactivateDetector()
    {
        state = DetectorState.Off;
        int count = laserObjects.Length;
        for (int i = 0; i < count; i++)
        {
            if (laserMoveControllers[i])
            {
                laserMoveControllers[i].UpdateState(false);
            }
            lasers[i].Off();
        }
    }

    private void EnemyMissing()
    {
        state = DetectorState.Activating;
        gun.TargetMissingHandle();
    }

    private void EnemyDetected()
    {
        state = DetectorState.EnemyDetected;
        gun.TargetDetectedHandle();
    }

    private void OnSystemActivate()
    {
        ActivateDetector();
    }

    private void OnSystemDeactivate()
    {
        DeactivateDetector();
    }

    private void OnDestroy()
    {
        if (gun != null)
        {
            gun.OnSystemActivate -= OnSystemActivate;
            gun.OnSystemDeactivate -= OnSystemDeactivate;
        }
    }

}
