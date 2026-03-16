using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;
    [SerializeField] private string fileName = "save.json";
    [SerializeField] private bool encryptData = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        allSaveables = FindISaveables();

        yield return new WaitForSeconds(0.01f);
        LoadGame();
    }

    private void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if(gameData == null)
        {
            Debug.Log("No save data found");
            gameData = new GameData();
            return;
        }

        foreach (var saveable in allSaveables)
            saveable.LoadData(gameData);
    }

    public void SaveGame()
    {
        if (allSaveables == null || dataHandler == null)
            return;

        foreach(var saveable in allSaveables)
            saveable.SaveData(ref gameData);

        dataHandler.SaveData(gameData);
    }

    [ContextMenu("*** Delete save data ***")]
    public void DeleteSave()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveable> FindISaveables()
    {
        return
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .ToList();
    }
}