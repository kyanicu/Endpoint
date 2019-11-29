using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static PlayerData loadedData;
        
    /// <summary>
    /// Retrieves saveable data and serializes it to a binary file
    /// </summary>
    /// <param name="p"></param>
    public static void SavePlayer (Player p)
    {
        int saveFileID = GameManager.SaveFileID;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = $"{GameManager.FILE_PATH}{saveFileID}.sav";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        PlayerData data = new PlayerData(p);
        formatter.Serialize(stream, data);
        stream.Close();

        //Check if file already exists and we'll need to overwrite
        /*if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            PlayerData data = new PlayerData(p);
            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            PlayerData data = new PlayerData(p);
            formatter.Serialize(stream, data);
            stream.Close();
        }*/
    }

    /// <summary>
    /// Deserializes save data from a binary file given a file ID
    /// </summary>
    /// <param name="p"></param>
    public static PlayerData LoadPlayer(int SaveFileID)
    {
        string path = $"{GameManager.FILE_PATH}{SaveFileID}.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
