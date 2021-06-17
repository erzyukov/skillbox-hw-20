using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Компонент сцены титры
/// </summary>
public class CreditsScene : MonoBehaviour
{
    /// <summary>
    /// Обработка нажатия кнопки Главное меню
    /// </summary>
    public void MainMenuButtonHandler()
    {
        GameManager.instance.scenes.LoadMainMenu();
    }

}
