using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour, ISaveable
{
    public static CurrencyManager Instance { get; private set; }

    public int Fireflies { get; private set; }
    public event Action<int> OnFirefliesChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Add(int amount)
    {
        Fireflies += amount;
        OnFirefliesChanged?.Invoke(Fireflies);
    }

    public void LoadData(GameData data)
    {
        Fireflies = data.fireflies;
        OnFirefliesChanged?.Invoke(Fireflies);
    }

    public void SaveData(ref GameData data)
    {
        data.fireflies = Fireflies;
    }
}