using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
     public int fireflies;
     public TextMeshProUGUI fireflyText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //reference the coin text and update it to the current coin count
        fireflyText.text = "Fireflies: " + fireflies.ToString();
    }
}
