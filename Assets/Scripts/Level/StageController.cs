using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс настройки уровня, настройки используются для генератора уровня
/// </summary>
public class StageController : MonoBehaviour
{
    [Header("Room Settings")]
    public int roomWidth = 30;
    public int roomHeight = 30;
    public int roomDepth = 30;
    [Space]
    [Header("Stage Settings")]
    public int rows = 3;
    public int cols = 6;

    [HideInInspector] public bool isGenerated = false;

    [HideInInspector] public GameObject[] rooms = { };

}
