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
        //Retrieve most up to date FileID
        int saveFileID = GameManager.SaveFileID;
        BinaryFormatter formatter = new BinaryFormatter();

        //Retrieve path of file to load
        string path = $"{GameManager.FILE_PATH}{saveFileID}.sav";

        //Open or create new file at path and save data
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        PlayerData data = new PlayerData(p);
        formatter.Serialize(stream, data);
        stream.Close();

        #region Overwriting Files (not sure if needed)
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
        #endregion
    }

    /// <summary>
    /// Deserializes save data from a binary file given a file ID
    /// </summary>
    /// <param name="SaveFileID"></param>
    public static PlayerData LoadPlayer(int SaveFileID)
    {
        //Retrieve path of file to load
        string path = $"{GameManager.FILE_PATH}{SaveFileID}.sav";

        //Check if file at that path actually exists
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        //If file at specified path doesn't exist, log an error
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }
}
