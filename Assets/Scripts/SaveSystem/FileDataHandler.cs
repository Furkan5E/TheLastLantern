using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        fullPath = Path.Combine(dataDirPath, dataFileName);
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToSave = JsonUtility.ToJson(gameData, true);


            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {            
                using(StreamWriter write = new StreamWriter(stream))
                {
                    write.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        GameData loadData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.Log("Error when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadData;
    }

    public void Delete()
    {
        if(File.Exists(fullPath))
            File.Delete(fullPath);
    }

}