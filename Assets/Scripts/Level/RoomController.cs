using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class RoomController : MonoBehaviour
{
    [HideInInspector] public Room.WallType leftWallType = Room.WallType.None;
    [HideInInspector] public Room.WallType rightWallType = Room.WallType.None;
    [HideInInspector] public Room.WallType ceilType = Room.WallType.None;
    [HideInInspector] public Room.WallType floorType = Room.WallType.None;
    [HideInInspector] public Room.LightingType lightingType = Room.LightingType.Off;

    [HideInInspector] public GameObject leftWall = null;
    [HideInInspector] public GameObject rightWall = null;
    [HideInInspector] public GameObject floor = null;
    [HideInInspector] public GameObject ceil = null;
    [HideInInspector] public GameObject lighing = null;

    [HideInInspector] public GameObject leftNeighbour = null;
    [HideInInspector] public GameObject rightNeighbour = null;
    [HideInInspector] public GameObject bottomNeighbour = null;
    [HideInInspector] public GameObject topNeighbour = null;

    public StageController stage;
}

