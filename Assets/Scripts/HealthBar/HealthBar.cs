using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Entity_Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        //holds the maximum amount of health the player has at the begining of the game
        // its equals to the current health at the start of the game
        totalhealthBar.fillAmount = playerHealth.currentHp /10;
    }

    private void Update()
    {
        // shows how much the player has left of thir health bar
        currenthealthBar.fillAmount = playerHealth.currentHp /10;
    }
}
