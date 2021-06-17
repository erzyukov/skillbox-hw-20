using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PowerUpItem))]
public class PowerUpEditor : Editor
{
    SerializedProperty energyConsumption;
    SerializedProperty energyAmount;
//    SerializedProperty targetEffect;
    SerializedProperty jetsPower;

    protected virtual void OnEnable()
    {
        energyConsumption = this.serializedObject.FindProperty("energyConsumption");
        energyAmount = this.serializedObject.FindProperty("energyAmount");
//        targetEffect = this.serializedObject.FindProperty("targetEffect");
        jetsPower = this.serializedObject.FindProperty("jetsPower");

        PowerUpItem item = (PowerUpItem)target;
        GameObject obj = item.gameObject;
        obj.tag = "PowerUp";
        obj.GetComponent<Collider>().isTrigger = true;
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        PowerUpItem item = (PowerUpItem)target;

        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            // если есть родитель префаб, значит распаковываем наш объект
            GameObject parent =  PrefabUtility.GetCorrespondingObjectFromSource(item.gameObject);
            if (parent != null)
            {
                PrefabUtility.UnpackPrefabInstance(item.gameObject, PrefabUnpackMode.Completely, InteractionMode.UserAction);
            }

            // удаляем все дочерние объекты
            foreach (Transform child in item.transform)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
            

            Renderer render = item.GetComponent<Renderer>();
            if (render != null)
            {
                switch (item.type)
                {
                    case PowerUpItem.Type.ForceFeld:
                        UpdateItem(item, "Shield");
                        break;
                    case PowerUpItem.Type.EnergyBattery:
                        UpdateItem(item, "EnergyBattery");
                        break;
                    case PowerUpItem.Type.EnergyCapacity:
                        UpdateItem(item, "EnergyCapacity");
                        break;
                    case PowerUpItem.Type.Stabilize:
                        UpdateItem(item, "Stabilize");
                        break;
                    case PowerUpItem.Type.Lantern:
                        UpdateItem(item, "Lantern");
                        break;
                    case PowerUpItem.Type.Invisible:
                        UpdateItem(item, "Invisible");
                        break;
                    default:
                        break;
                }
            }
        }

        switch (item.type)
        {
            case PowerUpItem.Type.ForceFeld:
                EditorGUILayout.PropertyField(energyConsumption);
//                EditorGUILayout.ObjectField(targetEffect, typeof(GameObject));
                break;
            case PowerUpItem.Type.EnergyBattery:
            case PowerUpItem.Type.EnergyCapacity:
                EditorGUILayout.PropertyField(energyAmount);
                break;
            case PowerUpItem.Type.Stabilize:
//                EditorGUILayout.ObjectField(targetEffect, typeof(GameObject));
                EditorGUILayout.PropertyField(jetsPower);
                break;
            case PowerUpItem.Type.Lantern:
                EditorGUILayout.PropertyField(energyConsumption);
                break;
            case PowerUpItem.Type.Invisible:
                EditorGUILayout.PropertyField(energyConsumption);
                EditorGUILayout.PropertyField(jetsPower);
                break;
        }

        this.serializedObject.ApplyModifiedProperties();
    }

    private void UpdateItem(PowerUpItem item, string type)
    {
        Renderer render = item.GetComponent<Renderer>();
        render.material = Resources.Load<Material>("Materials/PowerUpItem/" + type);
        Instantiate(Resources.Load<GameObject>("Prefabs/PowerUpItem/" + type), item.transform);
    }
}