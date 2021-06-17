using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Компонент настройки громкости звука
/// </summary>
public class OptionsController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer = default;
    [Space]
    [SerializeField] private Slider masterSlider = default;
    [SerializeField] private Toggle masterToggle = default;
    [Space]
    [SerializeField] private Text masterValue = default;
    [SerializeField] private Text musicValue = default;
    [SerializeField] private Text sfxValue = default;
    [Space]
    [SerializeField] private float minValue = -40;
    [SerializeField] private float maxValue = 10;
    [Space]
    [SerializeField] private Toggle skipTipsToggle = default;

    private void Start()
    {
        skipTipsToggle.isOn = (PlayerPrefs.GetInt("isSkipTips") == 1);

        // Устанавливаем начальные значения
        UpdateMasterSlider();
        UpdateMuteState();
        UpdateGroupValue(ref masterValue, "masterSoundVolume");
        UpdateGroupValue(ref musicValue, "musicSoundVolume");
        UpdateGroupValue(ref sfxValue, "sfxSoundVolume");
    }

    /// <summary>
    /// Установка мута звука
    /// </summary>
    /// <param name="state">состояние</param>
    public void MuteMaster(bool state)
    {
        if (state)
        {
            // берем текущее значение звука и запоминаем его в настройках игрока
            audioMixer.GetFloat("masterSoundVolume", out float premutedVolume);
            PlayerPrefs.SetFloat("premutedVolume", premutedVolume);
            // выставляем минимальный звук
            masterSlider.value = minValue;
            SetMasterVolume(-80);
            ChangeContorllersState(false);
        }
        else
        {
            masterSlider.value = PlayerPrefs.GetFloat("premutedVolume");
            SetMasterVolume(masterSlider.value);
            ChangeContorllersState(true);
        }

    }

    /// <summary>
    /// Устанавливает значение звука группе Мастер
    /// </summary>
    /// <param name="value">Значение</param>
    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("masterSoundVolume", value);
        UpdateGroupValue(ref masterValue, "masterSoundVolume");
    }

    /// <summary>
    /// Устанавливает значение звука группе Музыка
    /// </summary>
    /// <param name="value">Значение</param>
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("musicSoundVolume", value);
        UpdateGroupValue(ref musicValue, "musicSoundVolume");
    }

    /// <summary>
    /// Устанавливает значение звука группе Спецэффекты
    /// </summary>
    /// <param name="value">Значение</param>
    public void SetSfxVolume(float value)
    {
        audioMixer.SetFloat("sfxSoundVolume", value);
        UpdateGroupValue(ref sfxValue, "sfxSoundVolume");
    }

    /// <summary>
    /// Обновляет числовое значение в указанном поле для указанной группы
    /// </summary>
    /// <param name="field">Текстовое поле</param>
    /// <param name="groupName">Текстовое название группы</param>
    private void UpdateGroupValue(ref Text field, string groupName)
    {
        audioMixer.GetFloat(groupName, out float value);
        value = Mathf.Clamp(value, minValue, maxValue);
        float percent = (value - minValue) * 100 / (maxValue - minValue);
        field.text = Mathf.RoundToInt(percent).ToString();
    }

    /// <summary>
    /// Обновляет положение слайдера группы Мастер
    /// </summary>
    private void UpdateMasterSlider()
    {
        audioMixer.GetFloat("masterSoundVolume", out float value);
        value = Mathf.Clamp(value, minValue, maxValue);
        masterSlider.value = value;
    }

    /// <summary>
    /// Обновляет состояние переключателя заглушения звука
    /// </summary>
    private void UpdateMuteState()
    {
        if (masterSlider.value == minValue)
        {
            masterToggle.SetIsOnWithoutNotify(true);
            ChangeContorllersState(false);
        }
    }

    /// <summary>
    /// Изменяет состояние активности слайдеров
    /// </summary>
    /// <param name="state">Состояние</param>
    private void ChangeContorllersState(bool state)
    {
        Slider[] sliders = gameObject.GetComponentsInChildren<Slider>();
        foreach (Slider slider in sliders)
        {
            slider.enabled = state;
        }
    }
}
