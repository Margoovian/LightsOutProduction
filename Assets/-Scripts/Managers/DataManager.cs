using UnityEngine;
using System.IO;

public class DataManager
{
    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            instance ??= new DataManager();
            return instance;
        }
    }

    /// <summary>
    /// Saves all Data from "Settings" class into the Json format
    /// </summary>
    /// <param name="settings"></param>
    public void SaveAllData(BaseSettings settings, string filePath)
    {
        string data = JsonUtility.ToJson(settings, true);
        File.WriteAllText(filePath, data);
    }

    /// <summary>
    /// Loads all "Settings" data in the Json format
    /// (This may not be used! Remove if it doesn't go utilized!)
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="savedData"></param>
    public void Load(BaseSettings settings, string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, settings);
    }

    /// <summary>
    /// Gets the data from the "Settings" Json data using a jsonString
    /// </summary>
    /// <param name="jsonString"></param>
    /// <returns></returns>
    public BaseSettings GetData(string jsonString)
    {
        return JsonUtility.FromJson<BaseSettings>(jsonString);
    }

    /// <summary>
    /// This may not be a functional method, please update it!
    /// </summary>
    public void ResetData(string jsonString, BaseSettings settings)
    {
        JsonUtility.FromJsonOverwrite(jsonString, settings);
    }
}
