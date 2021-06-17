using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.PlayerLoop;

public class StageWindow : EditorWindow
{
    //string myString = "Hello, World!";

    private StageController selectedStage = null;
    private bool isGenerated = false;
    private int roomCount = 0;

    [MenuItem("Window/Stage Editor")]
    public static void ShowWindow()
    {
        GetWindow<StageWindow>("Stage Editor");
    }

    protected virtual void OnEnable()
    {
        //stage = this.serializedObject.FindProperty("stage");
    }

    private void OnSelectionChange()
    {
        StageController sc = GetStage();
        if (sc != null)
        {
            selectedStage = sc;
            UpdateSelectedStageInfo();
        }
        else
        {
            selectedStage = null;
        }
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Для работы с уровнем - выделите объект с сомпонентом StageController", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUI.BeginDisabledGroup(selectedStage == null);

        EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(isGenerated);
            if (GUILayout.Button("Сгенерировать комнаты", GUILayout.Height(50)))
            {
                GenerateStage();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!isGenerated);
            if (GUILayout.Button("Удалить комнаты", GUILayout.Height(50)))
            {
                ClearStage();
            }
            EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        if (selectedStage != null)
        {
            GUILayout.Space(10);
            if (isGenerated)
            {
                GUILayout.Label("Уровень сгенерирован");
                GUILayout.Label("Количество комнат: " + roomCount);
            }
            else
            {
                GUILayout.Label("Уровень не сгенерирован");
            }
        }


        EditorGUI.EndDisabledGroup();
    }

    private void UpdateSelectedStageInfo()
    {
        SerializedObject serializedObject = new UnityEditor.SerializedObject(selectedStage);
        isGenerated = serializedObject.FindProperty("isGenerated").boolValue;
        roomCount = serializedObject.FindProperty("rooms").arraySize;
    }

    /// <summary>
    /// Очищаем уровень от комнат
    /// </summary>
    private void ClearStage()
    {
        SerializedObject serializedObject = new UnityEditor.SerializedObject(selectedStage);
        SerializedProperty isGenerated = serializedObject.FindProperty("isGenerated");
        SerializedProperty scRooms = serializedObject.FindProperty("rooms");

        if (isGenerated.boolValue)
        {
            serializedObject.Update();

            for (int i = 0; i < scRooms.arraySize; i++)
            {
                GameObject.DestroyImmediate(scRooms.GetArrayElementAtIndex(i).objectReferenceValue);
            }
            scRooms.ClearArray();
            isGenerated.boolValue = false;

            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            Debug.LogWarning("Удалять нечего!");
        }
    }

    /// <summary>
    /// Генерируем уровень в зависимости от его настроек
    /// </summary>
    private void GenerateStage()
    {
        SerializedObject serializedObject = new UnityEditor.SerializedObject(selectedStage);
        SerializedProperty isGenerated = serializedObject.FindProperty("isGenerated");
        SerializedProperty scRooms = serializedObject.FindProperty("rooms");
        int roomWidth = serializedObject.FindProperty("roomWidth").intValue;
        int roomHeight = serializedObject.FindProperty("roomHeight").intValue;

        int rows = serializedObject.FindProperty("rows").intValue;
        int cols = serializedObject.FindProperty("cols").intValue;
        if (!isGenerated.boolValue)
        {
            serializedObject.Update();

            scRooms.ClearArray();

            // создаем комнаты
            GameObject[,] rooms = new GameObject[rows, cols];
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    Vector3 position = new Vector3(j * roomWidth, i * roomHeight, 0);
                    Room.PositionType type = DefineRoomPositionType(i, j, rows, cols);
                    rooms[i, j] = CreateRoom(selectedStage.transform, position, "-"+i+"-"+j, type);
                }
            }

            // проставляем соседей для комнат
            SetRoomNeighbour(ref rooms, rows, cols);

            //записываем данные о комнатах в переменную уровня
            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    scRooms.InsertArrayElementAtIndex(index);
                    scRooms.GetArrayElementAtIndex(index).objectReferenceValue = rooms[i, j];
                    index++;
                }
            }

            isGenerated.boolValue = true;
            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            Debug.LogWarning("Уровень уже сгенерирован!");
        }
    }

    private Room.PositionType DefineRoomPositionType(int i, int j, int rows, int cols)
    {
        if (j == 0 && i == rows - 1)
        {
            return Room.PositionType.LeftTop;
        }
        else if (j == cols - 1 && i == rows - 1)
        {
            return Room.PositionType.RightTop;
        }
        else if (i == rows - 1)
        {
            return Room.PositionType.TopEdge;
        }
        else if (j == 0)
        {
            return Room.PositionType.LeftEdge;
        }
        else if (j == cols - 1)
        {
            return Room.PositionType.RightEdge;
        }


        return Room.PositionType.None;
    }

    private void SetRoomNeighbour(ref GameObject[,] rooms, int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                RoomController rc = rooms[i, j].GetComponent<RoomController>();
                if (i != 0)
                {
                    rc.bottomNeighbour = rooms[i - 1, j];
                }
                if (i != rows - 1)
                {
                    rc.topNeighbour = rooms[i + 1, j];
                }
                if (j != 0)
                {
                    rc.leftNeighbour = rooms[i, j - 1];
                }
                if (j != cols - 1)
                {
                    rc.rightNeighbour = rooms[i, j + 1];
                }
            }
        }
    }

    /// <summary>
    /// Создает объект комнаты
    /// </summary>
    /// <param name="parent">Родитель</param>
    /// <param name="nameSufix">Суфикс для названия объекта</param>
    /// <returns>Объект комнаты</returns>
    private GameObject CreateRoom(Transform parent, Vector3 position, string nameSufix = "", Room.PositionType positionType = Room.PositionType.None)
    {
        GameObject room = new GameObject("Room" + nameSufix, typeof(RoomController));
        room.transform.parent = parent;
        room.transform.position = position;
        room.transform.rotation = Quaternion.identity;

        Room.UpdateFloor(Room.WallType.Solid, room);
        Room.UpdateLighting(Room.LightingType.Middle, room);

        switch (positionType)
        {
            case Room.PositionType.LeftEdge:
                Room.UpdateLeftWall(Room.WallType.Solid, room);
                break;
            case Room.PositionType.RightEdge:
                Room.UpdateRightWall(Room.WallType.Solid, room);
                break;
            case Room.PositionType.TopEdge:
                Room.UpdateCeil(Room.WallType.Solid, room);
                break;
            case Room.PositionType.LeftTop:
                Room.UpdateCeil(Room.WallType.Solid, room);
                Room.UpdateLeftWall(Room.WallType.Solid, room);
                break;
            case Room.PositionType.RightTop:
                Room.UpdateCeil(Room.WallType.Solid, room);
                Room.UpdateRightWall(Room.WallType.Solid, room);
                break;
        }

        return room;
    }

    /// <summary>
    /// Возвращает первый из выделенных уровней
    /// </summary>
    /// <returns></returns>
    private StageController GetStage()
    {
        StageController sc;
        foreach (GameObject obj in Selection.gameObjects)
        {
            sc = obj.GetComponent<StageController>();
            if (sc != null)
            {
                return sc;
            }
        }
        return null;
    }



    private RoomController[] GetSelectedRoom()
    {
        // use list man! :)
        RoomController[] result = { };

        int count = 0;
        foreach (GameObject obj in Selection.gameObjects)
        {
            RoomController rc = obj.GetComponent<RoomController>();
            if (rc != null)
            {
                count++;
                Array.Resize(ref result, count);
                result[count - 1] = rc;
            }
        }

        return result;
    }

}
