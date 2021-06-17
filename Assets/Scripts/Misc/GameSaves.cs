using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaves
{
    public bool isSkipTips = false;
    public Vector3 checkPoint = Vector3.zero;
    public int LastSceneId = 0;
}
