/// <summary>
/// Компонент управления окном поражения
/// </summary>
public class VictoryWindow : Window
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
    /// Обработка нажатия кнопки следующий уровень
    /// </summary>
    public void NextLevelButtonHandler()
    {
        Close();
        GameManager.instance.SetLevelCompleteState();
    }

}
