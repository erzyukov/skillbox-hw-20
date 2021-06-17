using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    /// <summary>
    /// Если отмечено - запоминает сцену, чтобы с нее можно было продолжить
    /// </summary>
    [Header("Запоминает сцену, для загрузи из главного меню")]
    [SerializeField] private bool isSaveScene = false;

    private void Start()
    {
        if (isSaveScene)
        {
            GameManager.instance.saves.LastSceneId = SceneManager.GetActiveScene().buildIndex;
        }
    }

    /// <summary>
    /// Перезагрузить текущий уровень
    /// </summary>
    public void RestatrLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Загрузить следующий уровень
    /// </summary>
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Перейти на сцену главного меню
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Перейти на сцену с титрами
    /// </summary>
    public void LoadCredits()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    /// <summary>
    /// Загружает сцену, которая была открыта в последний раз
    /// </summary>
    public void LoadLastScene()
    {
        if (GameManager.instance.saves.LastSceneId != 0)
        {
            // Загрузку начинаем с предыдущей сцены - там должен быть прелоадер
            SceneManager.LoadScene(GameManager.instance.saves.LastSceneId - 1);
        }
    }

    /// <summary>
    /// Выход из игры
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
