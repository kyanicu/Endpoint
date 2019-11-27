using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float[] MaxValues = { 0, 0, 0, 0, 0 };
    public static List<GameObject> Enemies;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        LoadMaxStats();

        //If DB hasn't been initialized yet, do that
        if (!LoadDataBaseEntries.AllLogsLoaded)
        {
            LoadDataBaseEntries.LoadAllDataEntries();
            LoadDataBaseEntries.AllLogsLoaded = true;
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
        HUDController.instance.UpdateHUD(Player.instance);
    }
    #endregion

    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                // fallback, might not be necessary.
                if (_instance == null)
                    _instance = new GameObject(typeof(GameManager).Name).AddComponent<GameManager>();

                // This breaks scene reloading
                // DontDestroyOnLoad(m_Instance.gameObject);
            }
            return _instance;
        }
    }
}
