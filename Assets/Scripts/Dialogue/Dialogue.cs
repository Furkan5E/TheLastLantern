using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI speakerComponent;

    [Header("Dialogue Settings")]
    public float textSpeed = 0.05f;

    private DialogueSO dialogueData;

    private string[] lines;
    private string[] speakers;
    private int index;

    public void SetDialogue(DialogueSO data)
    {
        dialogueData = data;
    }

    void Update()
    {
        if (lines == null || lines.Length == 0)
            return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && gameObject.activeSelf)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void LoadDialogue(DialogueSO data)
    {
        lines = null;
        speakers = null;

        if (data == null)
        {
            Debug.LogError("Dialogue data not assigned.");
            return;
        }

        if (data.lines == null || data.lines.Length == 0)
        {
            Debug.LogError($"Dialogue '{data.name}' has no lines.");
            return;
        }

        lines = new string[data.lines.Length];
        speakers = new string[data.lines.Length];
        for (int i = 0; i < data.lines.Length; i++)
        {
            lines[i] = data.lines[i].text;
            speakers[i] = data.lines[i].speaker;
        }
    }

    IEnumerator TypeLine()
    {
        speakerComponent.text = speakers[index];
        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (dialogueData == null)
            return;

        LoadDialogue(dialogueData);

        if (lines == null || lines.Length == 0)
        {
            Debug.LogError("No dialogue lines loaded.");
            return;
        }

        StopAllCoroutines();
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }
}
