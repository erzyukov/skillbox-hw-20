using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesManager : MonoBehaviour
{
    public Vector3 CheckPoint { 
        get 
        { 
            return saves.checkPoint; 
        } 
        set
        {
            saves.checkPoint = value;
            Save();
        }
    }

    public int LastSceneId
    {
        get
        {
            return saves.LastSceneId;
        }
        set
        {
            saves.LastSceneId = value;
            Save();
        }
    }

    private GameSaves saves;

    private void Awake()
    {
        saves = DataStorage.LoadData<GameSaves>();
        if (saves == default)
        {
            saves = new GameSaves();
        }
    }

    /// <summary>
    /// Сохраняет текущее состояние игры
    /// </summary>
    private void Save()
    {
        DataStorage.SaveData<GameSaves>(saves);
    }

}
