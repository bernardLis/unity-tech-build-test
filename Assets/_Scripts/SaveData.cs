using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=uD7y4T4PVk0\
// C:\Users\blis\AppData\LocalLow\DefaultCompany\Tactics Bu
[System.Serializable]
public class SaveData
{
    // global data
    public int Count;

    public string ToJson() { return JsonUtility.ToJson(this); }

    public void LoadFromJson(string jsonString) { JsonUtility.FromJsonOverwrite(jsonString, this); }
}

public interface ISavable
{
    void PopulateSaveData(SaveData saveData);
    void LoadFromSaveData(SaveData saveData);
}


