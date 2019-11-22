﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public static class LoadDataBaseEntries
{
    public static Dictionary<string, DataEntry> Logs = new Dictionary<string, DataEntry>();
    public static bool AllLogsLoaded = false;

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
        public Sprite LogImage;
        
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
        public DataEntry(string name, Dictionary<string, Tuple<string, bool>> info, Sprite img, string cat)
        {
            LogName = name;
            LogImage = img;
            LogEntries = info;
            LogCategory = cat;
            Visible = false;
        }
    }

    /// <summary>
    /// One time process of compiling all database entries from lore text/image files
    /// </summary>
    public static void LoadAllDataEntries()
    {
        //If logs have already been loaded, don't load them again
        if (AllLogsLoaded) return;

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
                        d.Visible = false;
                        Logs.Add(logName, d);
                    }
                }
            }
        }

        //Unlock stuff for testing purposes
        UnlockDataEntry("GREENHOUSE 10-1", "NEWS ARTICLE");
        UnlockDataEntry("GREENHOUSE 10-1", "OBSERVATION");
    }

    /// <summary>
    /// Converts a text file into a data entry by retrieving log name, info,
    /// and pulling image from resources with given log name
    /// </summary>
    /// <param name="ft"></param>
    /// <returns></returns>
    public static DataEntry RetrieveDataEntryFromText(TextAsset txt, string logName, string category, string fileType)
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

            entry.Visible = false;

            //Load the respective image from resources/images using the pulled name
            Sprite img = Resources.Load<Sprite>("Images/DataEntryImages/" + logName);
            if (img == null)
            {
                Debug.LogWarning("No image with name " + logName + " found");
                img = Resources.Load<Sprite>("Images/DataEntryImages/GREENHOUSE 10-2");
                entry.LogImage = img;
            }
            entry.LogImage = img;
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
    private static string pullDirectoryEndPoint(string path)
    {
        int pos = path.LastIndexOf("\\") + 1;
        return path.Substring(pos, path.Length - pos);
    }

    /// <summary>
    /// Sets visibility of an entry's article to true
    /// </summary>
    /// <param name="entryName"></param>
    /// <param name="entryArticle"></param>
    public static void UnlockDataEntry(string entryName, string entryArticle)
    {
        //Retrieve the entry
        DataEntry d = Logs[entryName];

        //Set the article's visibility to true
        Tuple<string, bool> newInfo = d.LogEntries[entryArticle];
        newInfo = new Tuple<string, bool>(newInfo.Item1, true);
        d.LogEntries[entryArticle] = newInfo;

        //If entry visibility hasn't been enabled yet, do that
        if (!d.Visible)
        {
            d.Visible = true;
        }

        Logs[entryName] = d;
    }
}