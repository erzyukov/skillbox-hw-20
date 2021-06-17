using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс обработки датчика с подвижной шкалой
/// В объекте должно быть два изображения со шкалой, пивот этих шкал должен быть в центре изображения 
/// и этот центр по вертикали должен совпадать с центром самого датчика.
/// </summary>
public class UIScaleController : MonoBehaviour
{
    /// <summary>
    /// Изображения шкалы (обязательно два)
    /// </summary>
    [SerializeField] private RectTransform[] scales = new RectTransform[SCALES_COUNT];

    [Space]
    /// <summary>
    /// Количество легенд отметок на шкале (тех что надо пометить числами)
    /// </summary>
    [SerializeField] private int legendCountOnScale = 3;
    /// <summary>
    /// Шаг значений на шкале
    /// </summary>
    [SerializeField] private int gradeStep = 5;
    /// <summary>
    /// Размер шага на шкале в юнитах (по Y)
    /// </summary>
    [SerializeField] private float gradeStepSize = 40;
    /// <summary>
    /// Только положительные значения
    /// </summary>
    [SerializeField] private bool isPositive = false;

    private const int SCALES_COUNT = 2;
    private float scaleHeight;
    private float currentValue = 0;

    private void Awake()
    {
        scaleHeight = scales[0].rect.height;
    }

    /// <summary>
    /// Выставляет начальное положение шкал датчика
    /// </summary>
    /// <param name="startValue">Начальное значение</param>
    public void InitScales(float startValue)
    {
        for (int i = 0; i < SCALES_COUNT; i++)
        {
            float yPos = i * scaleHeight + gradeStepSize / gradeStep * startValue;
            scales[i].anchoredPosition = new Vector2(scales[i].anchoredPosition.x, yPos);
            UpdateScaleLegends(-yPos * gradeStep / gradeStepSize, scales[i].gameObject);
        }
    }

    /// <summary>
    /// Устанавливает шкалу по указанному значению,
    /// если шкала перескакивает - обновляет промежуточные показатели на шкале
    /// </summary>
    /// <param name="value">Значение параметра</param>
    public void SetValue(float value)
    {
        if (isPositive && value < 0)
        {
            value = 0;
        }
        
        float offset = gradeStepSize / gradeStep * (currentValue - value);
        currentValue = value;

        for (int i = 0; i < SCALES_COUNT; i++)
        {
            float newYPosition = scales[i].anchoredPosition.y + offset;

            if (newYPosition <= -scaleHeight)
            {
                newYPosition += scaleHeight * 2;
                UpdateScaleLegends(Mathf.RoundToInt(value) + gradeStep * legendCountOnScale, scales[i].gameObject);
            }
            else if (newYPosition >= scaleHeight)
            {
                newYPosition -= scaleHeight * 2;
                UpdateScaleLegends(Mathf.RoundToInt(value) - gradeStep * legendCountOnScale, scales[i].gameObject);
            }

            scales[i].anchoredPosition = new Vector2(scales[i].anchoredPosition.x, newYPosition);
        }
    }

    private void OnValidate()
    {
        if (scales.Length != SCALES_COUNT)
        {
            Debug.LogWarning("Don't change the 'scales' field's array size!");
            Array.Resize(ref scales, SCALES_COUNT);
        }
    }

    private void UpdateScaleLegends(float currntValue, GameObject scaleObject)
    {
        int[] values = GetMainLegends(currntValue);

        Text[] scaleLegens = scaleObject.GetComponentsInChildren<Text>();
        for (int i = 0; i < legendCountOnScale; i++)
        {
            scaleLegens[i].text = (isPositive && values[i] < 0)? "": values[i].ToString();
        }
    }

    private int[] GetMainLegends(float value)
    {
        int[] result = new int[legendCountOnScale];

        int offset = (int) value / gradeStep;
        offset -= legendCountOnScale / 2;
        for (int i = 0; i < legendCountOnScale; i++)
        {
            result[i] = (offset + i) * gradeStep;
        }

        return result;
    }

}
