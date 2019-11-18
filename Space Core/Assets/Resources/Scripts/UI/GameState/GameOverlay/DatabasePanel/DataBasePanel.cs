using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataBasePanel : MonoBehaviour
{
    public Dictionary<string, DataEntry> Logs = new Dictionary<string, DataEntry>();

    /// <summary>
    /// DataEntry holds all database panel info and must be unlocked to be displayed in the panel
    /// </summary>
    [System.Serializable]
    public struct DataEntry
    {
        [Tooltip("The name of the data entry log")]
        public string LogName;

        [Tooltip("The type entry log data")]
        public string LogCategory;

        [Tooltip("The image associated with the data entry log")]
        public Image LogImage;
        
        [Tooltip("Returns whether or not the log has been unlocked")]
        public bool Visible;

        [Tooltip("All entries associated with this log")]
        public Dictionary<string, Tuple<string, bool>> LogEntries;        

        /// <summary>
        /// Data entry constructor. All entries start as locked
        /// </summary>
        /// <param name="name"></param>
        /// <param name="info"></param>
        /// <param name="img"></param>
        /// <param name="cat"></param>
        public DataEntry(string name, Dictionary<string, Tuple<string, bool>> info, Image img, string cat)
        {
            LogName = name;
            LogImage = img;
            LogEntries = info;
            LogCategory = cat;
            Visible = false;
        }
    }

    void Awake()
    {
        //Get the directionary holding the category folders
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Text/Lore");
        string[] categoryFolders = Directory.GetDirectories(dir.FullName);

        //Foreach category folder, pull individual log folders
        for (int i = 0; i < categoryFolders.Length; i++)
        {
            DirectoryInfo catDir = new DirectoryInfo(categoryFolders[i]);
            string[] logFolders = Directory.GetDirectories(catDir.FullName);

            //for each log folder, pull out all of their data entries
            foreach (string logPath in logFolders)
            {
                DirectoryInfo logDir = new DirectoryInfo(logPath);
                FileInfo[] Files = logDir.GetFiles("*.txt");

                string logName = pullDirectoryEndPoint(logPath);

                //Load all text assets from resources
                TextAsset[] dataFolders = Resources.LoadAll<TextAsset>("Text/Lore/" + catDir.Name + "/" + logName);

                //Loop through each text file in log folder

                for (int ii = 0; ii < dataFolders.Length; ii++)
                {
                    //Format filename appropriately 
                    string logFileType = pullDirectoryEndPoint(Files[ii].Name);
                    logFileType = logFileType.Substring(0, logFileType.Length - 4);

                    //Compile the data entry
                    DataEntry d = RetrieveDataEntryFromText(dataFolders[ii], logName, catDir.Name, logFileType);

                    //If it is a new entry, add it to logs
                    if (!Logs.ContainsKey(logName))
                    {
                        Logs.Add(logName, d);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Converts a text file into a data entry by retrieving log name, info,
    /// and pulling image from resources with given log name
    /// </summary>
    /// <param name="ft"></param>
    /// <returns></returns>
    public DataEntry RetrieveDataEntryFromText(TextAsset txt, string logName, string category, string fileType)
    {
        DataEntry entry;

        //Check if data already exists for file name before making new entry
        if (Logs.ContainsKey(logName))
        {
            entry = Logs[logName];
        }
        //If no entry exists compile data under new name
        else
        {
            //Compile new Log 
            entry = new DataEntry();

            //Load the respective image from resources/images using the pulled name
            try
            {
                Image img = Resources.Load<Image>("Images/DataEntryImages/" + logName);
                entry.LogImage = img;
            }
            catch (Exception e)
            {
                Debug.LogWarning("No image with name " + logName + " found");
                Image img = Resources.Load<Image>("Images/DataEntryImages/GREENHOUSE 10 - 1");
                entry.LogImage = img;
            }
            entry.LogCategory = category;
            entry.LogName = logName;
            entry.LogEntries = new Dictionary<string, Tuple<string, bool>>();
        }

        //Retrieve Log File Type
        string logFileType = fileType;

        //Add the log data into the entry
        entry.LogEntries.Add(logFileType, new Tuple<string, bool>(txt.text, false));

        //Return loaded entry
        return entry;
    }

    /// <summary>
    /// Pulls the end most item from a directory path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private string pullDirectoryEndPoint(string path)
    {
        int pos = path.LastIndexOf("\\") + 1;
        return path.Substring(pos, path.Length - pos);
    }
}
