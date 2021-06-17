using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Компонент управлеиня звуком двигателей
/// Для левого и правого двигателя устанавливается отдельный звук
/// </summary>
public class UFOJetSoundController : MonoBehaviour
{
    /// <summary>
    /// Звук для левого двигателя
    /// </summary>
    [SerializeField] private AudioSource leftJetAudio = default;
    /// <summary>
    /// Звук для правого двигателя
    /// </summary>
    [SerializeField] private AudioSource rightJetAudio = default;
    /// <summary>
    /// Компонент управления двигателями
    /// </summary>
    [SerializeField] private UFOJetsController jets = default;
    [Space]
    [SerializeField] private float minVolume = 0.1f;
    [SerializeField] private float maxVolume = 0.3f;
    [Space]
    [SerializeField] private float minPitch = 1f;
    [SerializeField] private float maxPitch = 2f;

    private float currentPersent = 0;
    private float lastChangedPersent = 0;
    private float lerpTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        leftJetAudio.volume = 0;
        rightJetAudio.volume = 0;
        jets.OnJetsWork += OnJetsWork;
    }

    private void OnJetsWork(float power, UFOJetsController.JetSide side)
    {
        // при обновлении значения мощьности - плавно меняем звук
        float percent = power / jets.JetsPower;
        if (lastChangedPersent != percent)
        {
            lastChangedPersent = percent;
            lerpTime = 0;
        }

        currentPersent = Mathf.Lerp(currentPersent, percent, lerpTime);
        lerpTime += Time.unscaledDeltaTime;

        switch (side)
        {
            case UFOJetsController.JetSide.Left:
                SetVolume(ref leftJetAudio, currentPersent);
                break;
            case UFOJetsController.JetSide.Right:
                SetVolume(ref rightJetAudio, currentPersent);
                break;
        }
    }

    /// <summary>
    /// Устанавливает процент громкости и питча (в зависимости от допустимых минимального и максимального значения)
    /// </summary>
    /// <param name="audio">Аудио ресурс</param>
    /// <param name="percent">Процент</param>
    private void SetVolume(ref AudioSource audio, float percent)
    {
        float volume = (maxVolume - minVolume) * percent;
        float pitch = (maxPitch - minPitch) * percent;
        audio.volume = minVolume + volume;
        audio.pitch = minPitch + pitch;
    }

    private void OnDestroy()
    {
        if (jets != null)
        {
            jets.OnJetsWork -= OnJetsWork;
        }
    }
}
