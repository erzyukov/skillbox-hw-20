using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(RoomController))]
public class RoomEditor : Editor
{
    SerializedProperty leftWallType;
    SerializedProperty rightWallType;
    SerializedProperty ceilType;
    SerializedProperty floorType;
    SerializedProperty lightingType;

    SerializedProperty leftNeighbour;
    SerializedProperty rightNeighbour;
    SerializedProperty bottomNeighbour;
    SerializedProperty topNeighbour;

    RoomController rc;

    protected virtual void OnEnable()
    {
        leftWallType = serializedObject.FindProperty("leftWallType");
        rightWallType = serializedObject.FindProperty("rightWallType");
        ceilType = serializedObject.FindProperty("ceilType");
        floorType = serializedObject.FindProperty("floorType");
        lightingType = serializedObject.FindProperty("lightingType");
        leftNeighbour = serializedObject.FindProperty("leftNeighbour");
        rightNeighbour = serializedObject.FindProperty("rightNeighbour");
        bottomNeighbour = serializedObject.FindProperty("bottomNeighbour");
        topNeighbour = serializedObject.FindProperty("topNeighbour");
        rc = (RoomController)target;
        if (rc.stage == null)
        {
            rc.stage = rc.GetComponentInParent<StageController>();
            if (rc.stage == null)
            {
                Debug.LogWarning("Can't found parent StageController object!");
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(leftWallType);
        EditorGUILayout.PropertyField(rightWallType);
        EditorGUILayout.PropertyField(ceilType);
        EditorGUILayout.PropertyField(floorType);
        EditorGUILayout.PropertyField(lightingType);

        if (EditorGUI.EndChangeCheck())
        {
            // подставляем выбранную часть комнаты, в зависимости от выбранного типа части
            // если у комнаты есть сосед рядом с выбраной частью, то убираем ту часть, которой она соединяется

            if (rc.leftWallType != (Room.WallType)leftWallType.enumValueIndex)
            {
                Room.UpdateLeftWall((Room.WallType)leftWallType.enumValueIndex, rc);
                Room.UpdateRightWall(Room.WallType.None, (GameObject)leftNeighbour.objectReferenceValue);
            }
            if (rc.rightWallType != (Room.WallType)rightWallType.enumValueIndex)
            {
                Room.UpdateRightWall((Room.WallType)rightWallType.enumValueIndex, rc);
                Room.UpdateLeftWall(Room.WallType.None, (GameObject) rightNeighbour.objectReferenceValue);
            }
            if (rc.floorType != (Room.WallType)floorType.enumValueIndex)
            {
                Room.UpdateFloor((Room.WallType)floorType.enumValueIndex, rc);
                Room.UpdateCeil(Room.WallType.None, (GameObject)bottomNeighbour.objectReferenceValue);
            }
            if (rc.ceilType != (Room.WallType)ceilType.enumValueIndex)
            {
                Room.UpdateCeil((Room.WallType)ceilType.enumValueIndex, rc);
                Room.UpdateFloor(Room.WallType.None, (GameObject)topNeighbour.objectReferenceValue);
            }
            if (rc.lightingType != (Room.LightingType) lightingType.enumValueIndex)
            {
                Room.UpdateLighting((Room.LightingType) lightingType.enumValueIndex, rc);
            }
        }

        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }

}
