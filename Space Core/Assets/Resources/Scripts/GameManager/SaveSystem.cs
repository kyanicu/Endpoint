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
    public static void SavePlayer (Character p)
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
<<<<<<< HEAD
            return null;
        }
    }

    /// <summary>
    /// Retrieves settings data and serializes it to a binary file
    /// </summary>
    /// <param name="settings"></param>
    public static void SaveSettings(SettingsSerlializer settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        //Retrieve path of file to load
        string path = $"{Application.dataPath}/Settings.ser";

        //Open or create new file at path and save data
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        formatter.Serialize(stream, settings);
        stream.Close();
    }

    /// <summary>
    /// Deserializes settings data from its binary file
    /// </summary>
    /// <param name="SaveFileID"></param>
    public static SettingsSerlializer LoadSettings()
    {
        //Retrieve path of settings to load
        string path = $"{Application.dataPath}/Settings.ser";

        //Check if file at that path actually exists
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsSerlializer data = formatter.Deserialize(stream) as SettingsSerlializer;
            stream.Close();

            return data;
        }
        //If file at specified path doesn't exist, log an error
        else
        {
            Debug.LogWarning("No settings found in " + path);
=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
            return null;
        }
    }

    /// <summary>
    /// Deletes saved settings whenever they are restored to default settings
    /// </summary>
    public static void DeleteSavedSettings()
    {//Retrieve path of settings to load
        string path = $"{Application.dataPath}/Settings.ser";

        //Check if file at that path actually exists
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
