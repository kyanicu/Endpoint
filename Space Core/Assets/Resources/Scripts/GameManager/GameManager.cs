using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float[] MaxValues = { 0, 0, 0, 0, 0 };
    public static List<GameObject> Enemies;
    public static string Sector;
    public static int SaveFileID = 0;
    public static string FILE_PATH;
    public static bool Initialized;

    private static GameManager _instance;
    public static GameManager instance { get { return _instance; } }

    public static Scenes currentScene;

    public enum Scenes
    {
        MainMenu,
        CentralProcessing,
        ShipmentFacility,
        scene3,
        scene4,
        scene5,
        scene6,
        scene7,
        scene8,
        scene9,
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        FILE_PATH = $"{Application.dataPath}/Save Files/Endpoint";
        Application.targetFrameRate = 60;
        LoadMaxStats();

        //If DB hasn't been initialized yet, do that
        if (!Initialized)
        {
            Initialized = true;
            currentScene = (Scenes) SceneManager.GetActiveScene().buildIndex;
            Sector = "CENTRAL PROCESSING";
            LoadDataBaseEntries.LoadAllDataEntries();
            LoadObjectives.LoadAllObjectives();
            LoadDialogue.LoadDialogueItems();
        }
        //Create a directory for save files if one doesn't exist
        if(!Directory.Exists(Application.dataPath + "/Save Files"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Save Files");
        }

        //Retrieve updated new SaveFileID
        string path = $"{FILE_PATH}{SaveFileID}.sav";
        while (File.Exists(path))
        {
            SaveFileID++;
            path = $"{FILE_PATH}{SaveFileID}.sav";
        }
        Enemies = null;
    }

    public void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        if (Enemies == null)
        {
            Enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        }
        Enemies.RemoveAll(enemy => enemy == null || enemy.CompareTag("Player"));
    }

    #region Total Range Stats
    public enum MaxStat
    {
        Damage,
        FireRate,
        ReloadTime,
        Range,
        BulletVeloc
    };

    /// <summary>
    /// One time call to load max values for all weapon stats, used when calculating diagnostic bar fill
    /// </summary>
    public static void LoadMaxStats()
    {
        List<WeaponGenerationInfo> allWeaponsList = new List<WeaponGenerationInfo>();
        allWeaponsList.Add(new Jakkaru());
        allWeaponsList.Add(new Matsya());
        allWeaponsList.Add(new SnipeyBoi());

        for (int i = 0; i < allWeaponsList.Count; i++)
        {
            //Load current weapon stats at list index i
            float[] loadedStats = allWeaponsList[i].PassMaxValues();

            //Loop through each weapon in the list to get the highest possible stats
            for (int x = 0; x <= (int)MaxStat.BulletVeloc; x++)
            {
                //Compare the curent to the max
                if (MaxValues[x] < loadedStats[x])
                {
                    //Overwrite list with new max
                    MaxValues[x] = loadedStats[x];
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// Pulls the end most item from a directory path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string PullDirectoryEndPoint(string path)
    {
        int pos = path.LastIndexOf("\\") + 1;
        return path.Substring(pos, path.Length - pos);
    }
}
