using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Компонент главного меню
/// </summary>
public class MainMenuScene : MonoBehaviour
{
    /// <summary>
    /// Прятать кнопку выхода
    /// </summary>
    [SerializeField] private bool isHideQuitButton = default;

    /// <summary>
    /// Ссылка на объект кнопки выхода
    /// </summary>
    [SerializeField] private GameObject quitButton = default;

    /// <summary>
    /// Ссылка на объект кнопки продолжения игры
    /// </summary>
    [SerializeField] private Button continueButton = default;

    /// <summary>
    /// Ссылка на объект окна настроек
    /// </summary>
    [SerializeField] private OptionsWindow optionWindow = default;

    private void Awake()
    {
        // скрываем кнопку выхода, если она нам не нужна
        if (quitButton && isHideQuitButton)
        {
            quitButton.SetActive(false);
        }
    }

    private void Start()
    {
        if (GameManager.instance.saves.LastSceneId != 0)
        {
            continueButton.interactable = true;
        }
    }

    /// <summary>
    /// Обработка нажатия кнопки Старт
    /// </summary>
    public void StartButtonHandler()
    {
        GameManager.instance.scenes.LoadNextLevel();
    }

    /// <summary>
    /// Обработка нажатия кнопки Настройки
    /// </summary>
    public void ContinueButtonHandler()
    {
        GameManager.instance.scenes.LoadLastScene();
    }

    /// <summary>
    /// Обработка нажатия кнопки Настройки
    /// </summary>
    public void OptionButtonHandler()
    {
        optionWindow.Open();
    }

    /// <summary>
    /// Обработка нажатия кнопки Выход
    /// </summary>
    public void QuitButtonHandler()
    {
        GameManager.instance.scenes.QuitGame();
    }

    /// <summary>
    /// Обработка нажатия кнопки Титры
    /// </summary>
    public void CreditsButtonHandler()
    {
        GameManager.instance.scenes.LoadCredits();
    }

}
