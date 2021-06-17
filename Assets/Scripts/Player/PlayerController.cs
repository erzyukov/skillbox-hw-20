using UnityEngine;

/// <summary>
/// Контроллер управления игроком
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Скрипт управления двигателями
    /// </summary>
    [SerializeField] private UFOJetsController jets = null;

    /// <summary>
    /// Скрипт управления топливом
    /// </summary>
    [SerializeField] private UFOFuelContorller fuel = null;

    /// <summary>
    /// Скрипт управления умениями
    /// </summary>
    [SerializeField] private UFOPowerUpContoller powerUp = null;

    /// <summary>
    /// Скрипт управления уничтожением
    /// </summary>
    [SerializeField] private UFODestroyHandler destroy = null;

    /// <summary>
    /// Компонент управления жизнью
    /// </summary>
    [SerializeField] private UFOLifeController life = null;

    private Vector3 initPosition;
    private Quaternion initRotation;

    private void Start()
    {
        initPosition = transform.position;
        initRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        //Debug.Log(jets.ToString());
        if (GameManager.instance.IsActionAllow())
        {
            jets.Move(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        }
    }

    private void Update()
    {
        // использование усиления
        if (GameManager.instance.IsActionAllow() && Input.GetKeyDown(KeyCode.Space))
        {
            powerUp.Use();
        }
    }

    /// <summary>
    /// Сбрасывает состояние игрока, без перезагрузки уровня
    /// </summary>
    /// <param name="checkPoint">Точка сохранения</param>
    public void ResetPlayer(Vector3 checkPoint = default)
    {
        StopAllRigidbody();
        // сбрасываем положение и вращение игрока
        transform.position = (checkPoint != default)? checkPoint: initPosition;
        transform.rotation = initRotation;
        // сбрасываем усиления
        powerUp.ResetPowerUp();
        // пополняем топливо
        fuel.FillUp();
        gameObject.SetActive(true);
        destroy.ResetState();
        life.ResetState();
    }

    /// <summary>
    /// Останавливает все Rigidbody на игроке
    /// </summary>
    private void StopAllRigidbody()
    {
        Rigidbody[] rbs = gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
            rb.isKinematic = false;
        }
    }

}
