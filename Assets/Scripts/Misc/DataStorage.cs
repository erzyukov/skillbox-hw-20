using System.IO;
using UnityEngine;

/// <summary>
/// Хранилище данных
/// Сохраняет/загружает данные в виде json строки в указанный файл
/// </summary>
public static class DataStorage
{
    /// <summary>
    /// Сохраняет данные объекта
    /// если не указывать имя файла, то имя образуется из названия типа объекта
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <param name="data">Объект</param>
    /// <param name="fileName">Имя файла (не обязательно)</param>
    public static void SaveData<T>(T data, string fileName = "")
    {
        string file = Application.persistentDataPath + ((fileName != "")? fileName : typeof(T) + ".json");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(file, json);
    }

    /// <summary>
    /// Загружает данные в объект
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <param name="fileName">Имя файла (не обязательно)</param>
    /// <returns>Объект с загруженными данными</returns>
    public static T LoadData<T>(string fileName = "")
    {
        string file = Application.persistentDataPath + ((fileName != "") ? fileName : typeof(T) + ".json");
        if (File.Exists(file))
        {
            return JsonUtility.FromJson<T>(File.ReadAllText(file));
        }
        return default;
    }

}
