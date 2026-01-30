using UnityEngine;

public interface ISaveable
{
    public void loadData(GameData data);
    public void SaveData(ref GameData data);
}