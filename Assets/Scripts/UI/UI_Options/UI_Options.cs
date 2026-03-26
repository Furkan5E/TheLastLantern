using UnityEngine;

public class UI_Options : UIScreen
{
    public void OnAudioPressed()
    {
        ui.PushScreen(ui.audioUI.gameObject);
    }
    public void OnVideoPressed()
    {
        ui.PushScreen(ui.videoUI.gameObject);
    }
    public void OnGamePressed()
    {
        ui.PushScreen(ui.gameUI.gameObject);
    }
}