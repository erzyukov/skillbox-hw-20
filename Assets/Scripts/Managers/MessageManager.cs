using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private Window defeatWindow = default;
    [SerializeField] private Window victoryWindow = default;

    /// <summary>
    /// Запускает процесс окончания уровня
    /// </summary>
    public void Victory()
    {
        StartCoroutine(ShowVictoryCoroutine(0.5f));
    }

    IEnumerator ShowVictoryCoroutine(float delay)
    {
        // Если игра в состоянии отображения диалога, то ожидаем пока мы из него не выйдем
        while (GameManager.instance.State == GameManager.StateType.Dialogue)
        {
            yield return new WaitForSeconds(delay);
        }

        victoryWindow.Open();
    }

    public void Defeat()
    {
        defeatWindow.Open();
    }
}
