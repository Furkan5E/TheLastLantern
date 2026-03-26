using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : UIScreen
{
    [Header("Main Menu Settings")]
    [SerializeField] private string sceneToLoad = "GameScene";

    public void OnPlayPressed()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnOptionsPressed()
    {
        if (ui != null && ui.optionsUI != null)
        {
            ui.PushScreen(ui.optionsUI.gameObject);
        }
    }

    public void OnQuitPressed()
    {
        if (ui != null)
        {
            ui.OnQuitPressed();
        }
    }
}