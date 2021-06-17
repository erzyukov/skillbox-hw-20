using System.Collections;
using UnityEngine;

/// <summary>
/// Компонент управления игрой
/// </summary>
[RequireComponent(typeof(LevelManager))]
public class GameManager : MonoBehaviour
{
    public StateType State { get { return state; } }

    public static GameManager instance = null;

    /// <summary>
    /// Компонент управления сценами
    /// </summary>
    public ScenesManager scenes;
    public LevelManager levelManager;
    public SavesManager saves;
    public MessageManager messages;

    [Space]
    [SerializeField] private Transform startPoint = default;
    [SerializeField] private GameObject player = default;

    [Space]
    [SerializeField] private StateType state;

    /// <summary>
    /// Типы состояния игры
    /// Game            - Игра
    /// Pause           - Пауза
    /// Menu            - Меню
    /// GameOver        - Поражение
    /// LevelComplete   - Победа
    /// Dialogue        - Диалог
    /// </summary>
    public enum StateType { Game, Pause, Menu, GameOver, LevelComplete, Dialogue };

    private StateType previousState;

    private void Awake()
    {
        if (instance == this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Time.timeScale = 1;
    }

    private void Start()
    {
        if (player != null && startPoint != default)
        {
            player.transform.position = startPoint.position;
            saves.CheckPoint = startPoint.position;
        }
    }

    void Update()
    {
        if (!IsPauseAllow())
        {
            return;
        }

        // для отладки - перезагрузка сцены
        if (Input.GetKeyDown(KeyCode.R))
        {
            scenes.RestatrLevel();
        }
    }

    private void LateUpdate()
    {
        // если статус меняется на LevelComplete - то переходим на следующий уровень
        if (state == StateType.LevelComplete)
        {
            scenes.LoadNextLevel();
        }
    }

    /// <summary>
    /// Проверяет разрешина ли пауза
    /// </summary>
    /// <returns></returns>
    public bool IsPauseAllow()
    {
        if (state == StateType.Dialogue || state == StateType.GameOver)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Проверяет разрешино ли движение
    /// </summary>
    /// <returns></returns>
    public bool IsActionAllow()
    {
        if (state == StateType.Game)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Перейти в состояние Пауза (StateType.Pause)
    /// </summary>
    public void PauseGame()
    {
        if (state != StateType.Pause)
        {
            previousState = state;
            state = StateType.Pause;
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Продолжить игру после паузы
    /// </summary>
    public void ResumeGame()
    {
        state = previousState;
        Time.timeScale = 1;
    }

    /// <summary>
    /// Перейти в состояние Диалог (StateType.Dialogue)
    /// </summary>
    public void SetDialogueState()
    {
        state = StateType.Dialogue;
    }

    /// <summary>
    /// Вернуться из состояния Диалог в указанное состояние (по умолчанию StateType.Game)
    /// </summary>
    /// <param name="state">Состояние игры</param>
    public void FinishDialogue(StateType state = StateType.Game)
    {
        this.state = state;
    }

    /// <summary>
    /// Перейти в состояние Уровень пройден (StateType.LevelComplete)
    /// </summary>
    public void SetLevelCompleteState()
    {
        state = StateType.LevelComplete;
    }

    /// <summary>
    /// Обработка смерти игрока
    /// </summary>
    public void PlayerDeathHandler()
    {
        messages.Defeat();
        state = StateType.GameOver;
    }

    /// <summary>
    /// Начинает игру с последней сохраненной точки
    /// </summary>
    public void StartGameFromCheckPoint()
    {
        player.GetComponent<PlayerController>().ResetPlayer(saves.CheckPoint);
        levelManager.ResetLevel();
        state = StateType.Game;
    }


    private void OnDestroy()
    {
        instance = null;
    }
}
