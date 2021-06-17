using UnityEngine;
using UnityEditor;
using System.Configuration;

[CustomEditor(typeof(DetectorController))]
public class DetectorEditor : Editor
{
    SerializedProperty type;

    protected virtual void OnEnable()
    {
        type = serializedObject.FindProperty("type");
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        DetectorController item = (DetectorController)target;

        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            // подставляем материал в зависимости от выбраного типа детектора
            item.type = (DetectorController.DetectorType)type.enumValueIndex;
            switch (item.type)
            {
                case DetectorController.DetectorType.Default:
                    item.laserBeamMaterial = Resources.Load<Material>("Materials/Obstacles/RedLaser");
                    break;
                case DetectorController.DetectorType.TrueSight:
                    item.laserBeamMaterial = Resources.Load<Material>("Materials/Obstacles/BlueLaser");
                    break;
                default:
                    break;
            }
        }

        this.serializedObject.ApplyModifiedProperties();
    }


}
