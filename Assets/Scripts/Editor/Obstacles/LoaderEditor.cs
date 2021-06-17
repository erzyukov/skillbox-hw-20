using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoaderController))]
public class LoaderEditor : Editor
{
    SerializedProperty xPosition;
    SerializedProperty yPosition;
    SerializedProperty zPosition;

    private LoaderController lc;
    private float longitudinalRailLength;
    private float transverseRailLength;
    private float transverseRailWidth;
    private float sliderWidth;
    private float ropeLength;
    private float minYHookPosition;
    private Vector3 longitudinalRailCenter;
    private Vector3 transverseRailCenter;

    protected virtual void OnEnable()
    {
        xPosition = serializedObject.FindProperty("xPosition");
        yPosition = serializedObject.FindProperty("yPosition");
        zPosition = serializedObject.FindProperty("zPosition");

        lc = (LoaderController)target;
        Renderer lr = lc.LongitudinalRail.GetComponent<Renderer>();
        Renderer tr = lc.TransverseRail.GetComponent<Renderer>();
        longitudinalRailLength = lr.bounds.size.x;
        transverseRailLength = tr.bounds.size.z;
        transverseRailWidth = tr.bounds.size.x;
        longitudinalRailCenter = lr.bounds.center;
        transverseRailCenter = tr.bounds.center;

        Renderer sr = lc.Slider.GetComponent<Renderer>();
        sliderWidth = sr.bounds.size.z;

        Renderer rr = lc.Rope.GetComponent<Renderer>();
        ropeLength = rr.bounds.size.y / lc.Rope.localScale.y;

        minYHookPosition = -ropeLength;// lc.Hook.localPosition.y;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            SetXPosition();
            SetZPosition();
            SetYPosition();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void SetXPosition()
    {
        // определяем положение поперечной рельсы в зависимости от выставленного положения xPosition
        float minX = longitudinalRailCenter.x - longitudinalRailLength / 2 + transverseRailWidth;
        float maxX = longitudinalRailCenter.x + longitudinalRailLength / 2 - transverseRailWidth;
        Vector3 trp = lc.TransverseRail.position;
        lc.TransverseRail.position = new Vector3((maxX - minX) * xPosition.floatValue + minX, trp.y, trp.z);
    }

    private void SetZPosition()
    {
        // определяем положение блока поперечной рельсы в зависимости от выставленного положения zPosition
        float minZ = transverseRailCenter.z - transverseRailLength / 2 + sliderWidth;
        float maxZ = transverseRailCenter.z + transverseRailLength / 2 - sliderWidth;
        Vector3 sp = lc.Slider.position;
        lc.Slider.position = new Vector3(sp.x, sp.y, (maxZ - minZ) * zPosition.floatValue + minZ);
    }

    private void SetYPosition()
    {
        // определяем положение зацепа в зависимости от выставленного положения yPosition
        Vector3 hp = lc.Hook.localPosition;
        lc.Hook.localPosition = new Vector3(hp.x, ((-lc.MaxHeight - minYHookPosition) * yPosition.floatValue) + minYHookPosition, hp.z);

        // определяем масштаб троса в зависимости от положения зацепа
        float currentDistance = Mathf.Abs(hp.y - minYHookPosition);
        lc.Rope.localScale = new Vector3(1, (currentDistance + ropeLength) / ropeLength, 1);
    }

}
