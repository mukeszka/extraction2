using UnityEngine;

public static class LocalPlayerId
{
    private const string Key = "LocalPlayerId";

    public static string GetOrCreate()
    {
        if (PlayerPrefs.HasKey(Key))
        {
            return PlayerPrefs.GetString(Key);
        }

        string newId = System.Guid.NewGuid().ToString();
        PlayerPrefs.SetString(Key, newId);
        PlayerPrefs.Save();
        return newId;
    }
}