using System.IO;
using UnityEngine;

public static class PlayerDataService
{
    private static string FolderPath => Path.Combine(Application.persistentDataPath, "PlayerData");

    private static string GetFilePath(string playerId)
    {
        return Path.Combine(FolderPath, $"{playerId}.json");
    }

    public static PlayerData Load(string playerId)
    {
        string path = GetFilePath(playerId);

        if (!File.Exists(path))
        {
            PlayerData fresh = new PlayerData { playerId = playerId };
            Save(fresh);
            return fresh;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<PlayerData>(json);
    }

    public static void Save(PlayerData data)
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetFilePath(data.playerId), json);
    }
}