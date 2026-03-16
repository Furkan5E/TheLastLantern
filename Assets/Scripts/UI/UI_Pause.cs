using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Pause : UIScreen
{
    public void OnContinuePressed()
    {
        ui.GoBack();
    }
    public void OnOptionsPressed()
    {
        ui.PushScreen(ui.optionsUI.gameObject);
    }
    public void OnQuitPressed()
    {
        ui.OnQuitPressed();
    }
}