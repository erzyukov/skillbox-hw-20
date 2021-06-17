using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Room
{
    private const string LEFT_WALL_PREFAB_PATH = "Level/Room/LeftWall/";
    private const string RIGHT_WALL_PREFAB_PATH = "Level/Room/RightWall/";
    private const string FLOOR_PREFAB_PATH = "Level/Room/Floor/";
    private const string CEIL_PREFAB_PATH = "Level/Room/Ceil/";
    private const string LIGHTING_PREFAB_PATH = "Level/Room/Lighting/";

    public enum WallType { None, Solid, Gate }
    public enum LightingType { Off, On, Middle, Dark }

    public enum PositionType { None, LeftEdge, RightEdge, TopEdge, LeftTop, RightTop }

    public static void UpdateFloor(WallType type, GameObject room)
    {
        RoomController rc = room.GetComponent<RoomController>();
        if (rc != null)
        {
            UpdateFloor(type, rc);
        }
    }

    public static void UpdateFloor(WallType type, RoomController room)
    {
        room.floorType = type;
        GameObject prefab = Resources.Load<GameObject>(FLOOR_PREFAB_PATH + type);
        UpdateRoomPrefab(prefab, ref room.floor, room.transform, "Floor" + type);
    }

    public static void UpdateLighting(LightingType type, GameObject room)
    {
        RoomController rc = room.GetComponent<RoomController>();
        if (rc != null)
        {
            UpdateLighting(type, rc);
        }
    }

    public static void UpdateLighting(LightingType type, RoomController room)
    {
        room.lightingType = type;
        GameObject prefab = Resources.Load<GameObject>(LIGHTING_PREFAB_PATH + type);
        UpdateRoomPrefab(prefab, ref room.lighing, room.transform, "Lighting" + type);
    }

    public static void UpdateLeftWall(WallType type, GameObject room)
    {
        RoomController rc = room.GetComponent<RoomController>();
        if (rc != null)
        {
            UpdateLeftWall(type, rc);
        }
    }

    public static void UpdateLeftWall(WallType type, RoomController room)
    {
        room.leftWallType = type;
        GameObject prefab = Resources.Load<GameObject>(LEFT_WALL_PREFAB_PATH + type);
        UpdateRoomPrefab(prefab, ref room.leftWall, room.transform, "LeftWall" + type);
    }

    public static void UpdateRightWall(WallType type, GameObject room)
    {
        RoomController rc = room.GetComponent<RoomController>();
        if (rc != null)
        {
            UpdateRightWall(type, rc);
        }
    }

    public static void UpdateRightWall(WallType type, RoomController room)
    {
        room.rightWallType = type;
        GameObject prefab = Resources.Load<GameObject>(RIGHT_WALL_PREFAB_PATH + type);
        UpdateRoomPrefab(prefab, ref room.rightWall, room.transform, "RightWall" + type);
    }

    public static void UpdateCeil(WallType type, GameObject room)
    {
        RoomController rc = room.GetComponent<RoomController>();
        if (rc != null)
        {
            UpdateCeil(type, rc);
        }
    }

    public static void UpdateCeil(WallType type, RoomController room)
    {
        room.ceilType = type;
        GameObject prefab = Resources.Load<GameObject>(CEIL_PREFAB_PATH + type);
        UpdateRoomPrefab(prefab, ref room.ceil, room.transform, "Ceil" + type);
    }


    private static void UpdateRoomPrefab(GameObject prefab, ref GameObject rcPartObject, Transform room, string objectName = null)
    {
        if (rcPartObject != null)
        {
            GameObject.DestroyImmediate(rcPartObject);
            rcPartObject = null;
        }

        if (prefab != null)
        {
            rcPartObject = GameObject.Instantiate(prefab, room.position, Quaternion.identity, room);
            if (objectName != null)
            {
                rcPartObject.name = objectName;
            }
        }
    }


}
