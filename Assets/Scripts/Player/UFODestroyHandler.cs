using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс обработки уничтожения корабля
/// </summary>
public class UFODestroyHandler : MonoBehaviour
{
    /// <summary>
    /// Объект содержащий в себе части корабля, которые будут разлетаться при взрыве
    /// </summary>
    [SerializeField] private GameObject partsContainer = null;
    /// <summary>
    /// Система частиц отвечающая за эффект взрыва
    /// </summary>
    [SerializeField] private ParticleSystem particles = null;

    [Space]
    [Header("Explosion Power")]
    /// <summary>
    /// Сила взрыва
    /// </summary>
    [SerializeField] private float power = 300;
    /// <summary>
    /// Вертикальная сила взрыва
    /// </summary>
    [SerializeField] private float upPower = 10;

    Vector3 initContainerPosition;
    Rigidbody[] parts;
    Vector3[] initPartPositions;

    private void Awake()
    {
        parts = partsContainer.GetComponentsInChildren<Rigidbody>();
        initPartPositions = new Vector3[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            initPartPositions[i] = parts[i].transform.localPosition;
        }
        initContainerPosition = partsContainer.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Explode();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Crash();
        }
        
    }

    /// <summary>
    /// Взрыв коробля эффектом частиц и большим разлетом частей коробля
    /// </summary>
    public void Explode()
    {
        Destroy(true, power, upPower);
    }

    /// <summary>
    /// Разбивает корабль на части
    /// </summary>
    public void Crash()
    {
        Destroy(false, power/5, upPower/5);
    }

    /// <summary>
    /// Сбрасывает объект в изначальное состояние
    /// </summary>
    public void ResetState()
    {
        partsContainer.SetActive(false);
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].isKinematic = false;
            parts[i].transform.localPosition = initPartPositions[i];
        }
        partsContainer.transform.position = initContainerPosition;
    }

    /// <summary>
    /// Уничтожает корабль с различными параметрами эффекта
    /// </summary>
    /// <param name="isExplode">Отображать взрыв</param>
    /// <param name="power">Сила взрыва</param>
    /// <param name="upPower">Вертикальная сила взрыва</param>
    private void Destroy(bool isExplode, float power, float upPower)
    {
        // перемещаем заготовку в место с игроком
        partsContainer.transform.position = transform.position;
        partsContainer.transform.rotation = transform.rotation;
        Vector3 parentVelocity = transform.GetComponent<Rigidbody>().velocity;

        // игрока убираем и вместо него ставим заготовку
        gameObject.SetActive(false);
        partsContainer.SetActive(true);

        // раскидываем части взрывом, предварительно придавая им скорость игрока
        foreach (Rigidbody part in parts)
        {
            part.isKinematic = false;
            part.velocity = parentVelocity;
            part.AddExplosionForce(power, transform.position, 5, upPower, ForceMode.Impulse);
        }

        if (isExplode)
        {
            particles.Play();
        }
    }
}
