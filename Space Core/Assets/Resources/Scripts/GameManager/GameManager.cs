using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Dictionary<string, float[]> MaxStats;
    public static List<GameObject> EnemyControllers;
    public static string Sector;
    public static int SaveFileID = 0;
    public static string FILE_PATH;
    public static bool Initialized;
    public static float Timer;
    public static int PlayerLevel = 0;
    public static Dictionary<string, OneTimeEventTags> OneTimeEvents = new Dictionary<string, OneTimeEventTags>();

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

    public enum OneTimeEventTags
    {
        Destroy,
        Console,
        HazardSwitch
    }
    private void Awake()
    {
        if (_instance == null || _instance != this)
        {
            _instance = this;
        }
        FILE_PATH = $"{Application.dataPath}/Save Files/Endpoint";
        Application.targetFrameRate = 60; 
        LoadMaxStats();

        //If DB hasn't been initialized yet, do that
        if (!Initialized)
        {
            InputManager.instance.currentState = InputManager.InputState.MAIN_MENU;
            Initialized = true;
            currentScene = (Scenes)SceneManager.GetActiveScene().buildIndex;
            Sector = "CENTRAL PROCESSING";
            Timer = 0;

            //Refresh progress lists
            LoadDataBaseEntries.LoadAllDataEntries();
            LoadObjectives.LoadAllObjectives();
            LoadDialogue.LoadDialogueItems();
        }
        //Create a directory for save files if one doesn't exist
        if (!Directory.Exists(Application.dataPath + "/Save Files"))
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
        LoadMaxStats();
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyControllers = null;

        if (OneTimeEvents.Count > 0)
        {
            //Iterate through each One Time Event that has already occurred
            foreach (KeyValuePair<string, OneTimeEventTags> ote in OneTimeEvents)
            {
                //Locate the event in the scene given its name
                GameObject Event = GameObject.Find(ote.Key);

                //Check if the game object exists in the current scene
                if (Event != null)
                {
                    switch (ote.Value)
                    {
                        case OneTimeEventTags.Destroy:
                            Destroy(Event.gameObject);
                            break;
                        case OneTimeEventTags.Console:
                            Console console = Event.GetComponent<Console>();
                            console.AlreadyPressed = true;
                            break;
                        case OneTimeEventTags.HazardSwitch:
                            HazardSwitch hazardSwitch = Event.GetComponent<HazardSwitch>();
                            hazardSwitch.AlreadyPressed = true;
                            break;
                    }
                }
            }
        }
    }

    public void Update()
    {
        Timer += Time.deltaTime;
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        if (EnemyControllers == null)
        {
            EnemyControllers = GameObject.FindGameObjectsWithTag("EnemyController").ToList();
        }
        EnemyControllers.RemoveAll(enemy => enemy == null || enemy.CompareTag("Player"));
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
        MaxStats = new Dictionary<string, float[]>();
        List<WeaponGenerationInfo> allWeaponsList = new List<WeaponGenerationInfo>();
        allWeaponsList.Add(new Jakkaru());
        allWeaponsList.Add(new Matsya());
        allWeaponsList.Add(new SnipeyBoi());

        for (int i = 0; i < allWeaponsList.Count; i++)
        {
            //Load current weapon stats at list index i
            float[] loadedStats = allWeaponsList[i].PassMaxValues();
            string weaponName = allWeaponsList[i].name;

            //Add them to the dictionary keeping track of each weapon's max stats
            MaxStats.Add(weaponName, loadedStats);
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

    /// <summary>
    /// Returns a formatted string representing player's time in game
    /// </summary>
    /// <returns></returns>
    public static string RetrievePlayTime(float playerTime)
    {
        int seconds = (int)playerTime % 60;
        string sec = seconds > 9 ? seconds + "" : "0" + seconds;
        int minutes = (int)playerTime / 60;
        string min = minutes > 9 ? minutes + "" : "0" + minutes;
        int hours = (int)playerTime / 3600;
        string hr = hours > 9 ? hours + "" : "0" + hours;
        return $"{hr}:{min}:{sec}";
    }
}
