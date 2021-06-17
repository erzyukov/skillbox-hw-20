using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Структура для хранения объекта диалога
/// </summary>
[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue Object", order = 51)]
public class DialogueObject : ScriptableObject
{
	/// <summary>
	/// Имя говорящего
	/// </summary>
	public string speakerName;

	/// <summary>
	/// Тип диалога
	/// </summary>
	public DialogType type;

	/// <summary>
	/// Список предложений диалога
	/// </summary>
	[Header("Dialogue")]
	public DialogueSentence[] sentences;

	/// <summary>
	/// Ссылка на следующий диалог (не обязательно)
	/// </summary>
	[Header("Link to next dialogue (optional)")]
	public DialogueObject nextDialogue;
}

[System.Serializable]
public struct DialogueSentence
{
	/// <summary>
	/// Текст предложения
	/// </summary>
	[TextArea(3, 10)]
	public string text;

	/// <summary>
	/// Изображение для предложения (не обязательно)
	/// </summary>
	[Header("Image for sentence (optional)")]
	public Sprite image;
}

/// <summary>
/// Типы диалога
/// Default - по умолчанию (сценарий)
/// Lesson  - Обучение
/// </summary>
public enum DialogType { Default, Lesson }