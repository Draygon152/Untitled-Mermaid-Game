using UnityEngine;

public static class SaveDataUtility
{
    private const string HAS_SAVE_DATA = "HAS_SAVE_DATA";
    public const string MASTER_VOLUME = "MASTER_VOLUME";
    public const string MUSIC_VOLUME = "MUSIC_VOLUME";
    public const string SFX_VOLUME = "SFX_VOLUME";
    public const string CURSOR_LOCK_MODE = "CURSOR_LOCK_MODE";



#region Data Saving Functions
    public static void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);

        if (!HasSaveData())
        {
            MarkDataSaved();
        }
    }

    public static void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);

        if (!HasSaveData())
        {
            MarkDataSaved();
        }
    }

    public static void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);

        if (!HasSaveData())
        {
            MarkDataSaved();
        }
    }

    public static void SaveBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);

        if (!HasSaveData())
        {
            MarkDataSaved();
        }
    }

    // Saves generic data to PlayerPrefs in json format
    public static void Save<T>(string key, T value)
    {
        string jsonDataString = JsonUtility.ToJson(value);
        SaveString(key, jsonDataString);

        if (!HasSaveData())
        {
            MarkDataSaved();
        }
    }
#endregion

#region Data Loading Functions
    public static int LoadInt(string key, int defaultVal = 0)
    {
        return PlayerPrefs.GetInt(key, defaultVal);
    }

    public static float LoadFloat(string key, float defaultVal = 0f)
    {
        return PlayerPrefs.GetFloat(key, defaultVal);
    }

    public static string LoadString(string key, string defaultVal = "")
    {
        return PlayerPrefs.GetString(key, defaultVal);
    }

    public static bool LoadBool(string key, int defaultVal = 0)
    {
        return PlayerPrefs.GetInt(key, defaultVal) == 1;
    }

    public static T Load<T>(string key)
    {
        string jsonDataString = LoadString(key);
        return JsonUtility.FromJson<T>(jsonDataString);
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
#endregion

#region Data Clearing Functions
    public static void ClearAllGameData()
    {
        Debug.Log("ALL GAME DATA CLEARED");

        // Add clearing functions here

        PlayerPrefs.DeleteKey(HAS_SAVE_DATA);
    }
#endregion

#region Utility Functions
    private static void MarkDataSaved()
    {
        SaveBool(HAS_SAVE_DATA, true);
    }

    public static bool HasSaveData()
    {
        return PlayerPrefs.HasKey(HAS_SAVE_DATA) && LoadBool(HAS_SAVE_DATA);
    }
#endregion
}