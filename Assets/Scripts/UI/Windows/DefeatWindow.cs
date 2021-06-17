using UnityEngine;

/// <summary>
/// Компонент управления окном поражения
/// </summary>
public class DefeatWindow : Window
{

    /// <summary>
    /// Закрывает окно и загружает главное меню
    /// </summary>
    public override void Close()
    {
        base.Close();
        GameManager.instance.scenes.LoadMainMenu();
    }

    /// <summary>
    /// Обработка нажатия кнопки попробовать еще
    /// </summary>
    public void TryAgainButtonHandler()
    {
        base.Close();
        GameManager.instance.StartGameFromCheckPoint();
    }

}
