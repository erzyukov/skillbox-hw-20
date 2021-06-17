using UnityEngine;

/// <summary>
/// Компонент управления окном паузы
/// </summary>
public class PauseWindow : Window
{
    /// <summary>
    /// Ссылка на объект окна настроек
    /// </summary>
    [SerializeField] private OptionsWindow optionWindow = default;

    /// <summary>
    /// Ссылка на объект окна помощи
    /// </summary>
    [SerializeField] private HelpWindow helpWindow = default;

    void Update()
    {
        if (!GameManager.instance.IsPauseAllow())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.State != GameManager.StateType.Pause)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    /// <summary>
    /// Открывает окно паузы и включает паузу
    /// </summary>
    public override void Open()
    {
        base.Open();
        GameManager.instance.PauseGame();
    }

    /// <summary>
    /// Закрывает окно паузы и выключает паузу
    /// Вместе с окном паузы, закрываются все зависимые окна
    /// </summary>
    public override void Close()
    {
        base.Close();
        optionWindow.Close();
        helpWindow.Close();
        GameManager.instance.ResumeGame();
    }

    /// <summary>
    /// Обработка нажатия кнопки помощи
    /// </summary>
    public void HelpButtonHandler()
    {
        optionWindow.Close();
        if (helpWindow)
        {
            helpWindow.Open();
        }
    }

    /// <summary>
    /// Обработка нажатия кнопки настройки
    /// </summary>
    public void OptionsButtonHandler()
    {
        optionWindow.Open();
    }

    /// <summary>
    /// Обработка нажатия кнопки рестарт
    /// </summary>
    public void RestartButtonHandler()
    {
        GameManager.instance.scenes.RestatrLevel();
    }

    /// <summary>
    /// Обработка нажатия кнопки чекпоинт
    /// </summary>
    public void CheckpointButtonHandler()
    {
        GameManager.instance.StartGameFromCheckPoint();
        Close();
    }

    /// <summary>
    /// Обработка нажатия кнопки главное меню
    /// </summary>
    public void MainMenuButtonHandler()
    {
        GameManager.instance.scenes.LoadMainMenu();
    }

    /// <summary>
    /// Обработка нажатия кнопки выход
    /// </summary>
    public void QuitButtonHandler()
    {
        GameManager.instance.scenes.QuitGame();
    }

}
