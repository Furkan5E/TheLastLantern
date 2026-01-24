using UnityEngine;

public class Properties : MonoBehaviour
{
    int health = 5;
    int maxHealth = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
