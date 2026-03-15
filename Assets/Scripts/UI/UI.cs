using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UI : MonoBehaviour
{
    [SerializeField] private bool isMainMenuScene;
    [SerializeField] private RectTransform leftIndicator;
    [SerializeField] private RectTransform rightIndicator;
    [SerializeField] private float indicatorOffsetX = 80f;
    private Action<InputAction.CallbackContext> onOptionsPerformed;

    public UI_Pause pauseUI { get; private set; }
    public UI_Options optionsUI { get; private set; }
    public UI_Audio audioUI { get; private set; }
    public UI_Video videoUI { get; private set; }
    public UI_Game gameUI { get; private set; }
    public UI_MainMenu mainMenuUI { get; private set; }

    private Stack<GameObject> uiStack = new Stack<GameObject>();
    private PlayerInputSet input;

    private void Awake()
    {
        pauseUI = GetComponentInChildren<UI_Pause>(true);
        optionsUI = GetComponentInChildren<UI_Options>(true);
        audioUI = GetComponentInChildren<UI_Audio>(true);
        videoUI = GetComponentInChildren<UI_Video>(true);
        gameUI = GetComponentInChildren<UI_Game>(true);
        mainMenuUI = GetComponentInChildren<UI_MainMenu>(true);

        HideIndicators();
    }

    private void Start()
    {
        if (isMainMenuScene && mainMenuUI != null)
        {
            PushScreen(mainMenuUI.gameObject);
        }
    }

    public void SetupControlsUI(PlayerInputSet inputSet)
    {
        input = inputSet;
        onOptionsPerformed = ctx =>
        {
            if (uiStack.Count > (isMainMenuScene ? 1 : 0))
                GoBack();
            else if (!isMainMenuScene && pauseUI != null)
                PushScreen(pauseUI.gameObject);
        };
        input.UI.OptionsUI.performed += onOptionsPerformed;
    }

    private void OnDestroy()
    {
        if (input != null && onOptionsPerformed != null)
        {
            input.UI.OptionsUI.performed -= onOptionsPerformed;
        }
    }

    public void MoveIndicators(RectTransform target)
    {
        leftIndicator.gameObject.SetActive(true);
        rightIndicator.gameObject.SetActive(true);

        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);
        Vector3 leftWorld  = (corners[0] + corners[1]) / 2f;
        Vector3 rightWorld = (corners[2] + corners[3]) / 2f;

        float scaledOffset = indicatorOffsetX * target.lossyScale.x;
        leftIndicator.position  = leftWorld - (target.right * scaledOffset);
        rightIndicator.position = rightWorld + (target.right * scaledOffset);
    }

    public void HideIndicators()
    {
        leftIndicator.gameObject.SetActive(false);
        rightIndicator.gameObject.SetActive(false);
    }

    public void PushScreen(GameObject screen)
    {
        HideIndicators();
        if (uiStack.Count > 0)
            uiStack.Peek().SetActive(false);

        uiStack.Push(screen);
        screen.SetActive(true);

        if (!isMainMenuScene && uiStack.Count == 1)
        {
            Time.timeScale = 0;
            input?.Player.Disable();
        }
    }

    public void GoBack()
    {
        if (uiStack.Count <= (isMainMenuScene ? 1 : 0)) return;

        HideIndicators();
        uiStack.Pop().SetActive(false);

        if (uiStack.Count > 0)
            uiStack.Peek().SetActive(true);
        
        if (!isMainMenuScene && uiStack.Count == 0)
        {
            Time.timeScale = 1;
            input?.Player.Enable();
        }
    }

    public void OnQuitPressed()
    {
        if (isMainMenuScene)
        {
            Application.Quit();
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        else
        {
            while (uiStack.Count > 0)
                uiStack.Pop().SetActive(false);

            uiStack.Clear();
            Time.timeScale = 1;
            input?.Player.Enable();  
            SaveManager.Instance?.SaveGame();
            SceneManager.LoadScene("MainMenu");
        }
    }
}