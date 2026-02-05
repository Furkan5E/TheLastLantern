using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI speakerComponent;

    [Header("Dialogue Settings")]
    public float textSpeed = 0.05f;

    private string dialogueId;

    [Header("Data")]
    public TextAsset dialogueJson;

    private string[] lines;
    private string[] speakers;
    private int index;
    private bool hasStarted;

    void Start()
    {
        hasStarted = true;
    }

    public void SetDialogueId(string id)
    {
        dialogueId = id;
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

    void LoadDialogue(string id)
    {
        if (dialogueJson == null)
        {
            Debug.LogError("Dialogue JSON not assigned.");
            return;
        }

        DialogueDatabase db =
            JsonUtility.FromJson<DialogueDatabase>(dialogueJson.text);

        if (db == null || db.dialogues == null)
        {
            Debug.LogError("Failed to parse dialogue JSON.");
            return;
        }

        DialogueData dialogue =
            Array.Find(db.dialogues, d => d.id == id);

        if (dialogue == null)
        {
            Debug.LogError($"Dialogue with id '{id}' not found.");
            return;
        }

        lines = new string[dialogue.lines.Length];
        speakers = new string[dialogue.lines.Length];
        for (int i = 0; i < dialogue.lines.Length; i++)
        {
            lines[i] = dialogue.lines[i].text;
            speakers[i] = dialogue.lines[i].speaker;
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
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
        if (string.IsNullOrEmpty(dialogueId))
            return;

        LoadDialogue(dialogueId);

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

    // ─────────────────────────────
    // Nested JSON data structures
    // ─────────────────────────────

    [Serializable]
    private class DialogueDatabase
    {
        public DialogueData[] dialogues;
    }

    [Serializable]
    private class DialogueData
    {
        public string id;
        public DialogueLine[] lines;
    }

    [Serializable]
    private class DialogueLine
    {
        public string speaker;
        public string text;
    }
}
