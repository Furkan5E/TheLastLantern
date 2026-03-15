using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class DialogueSOImporter
{
    private const string JsonPath = "Assets/Data/Dialogue/dialogue.json";
    private const string OutputFolder = "Assets/Data/Dialogue";

    [MenuItem("Tools/Dialogue/Import JSON To DialogueSO")]
    public static void ImportFromJson()
    {
        TextAsset jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(JsonPath);
        if (jsonAsset == null)
        {
            Debug.LogError($"Could not find JSON file at '{JsonPath}'.");
            return;
        }

        DialogueDatabaseJson db = JsonUtility.FromJson<DialogueDatabaseJson>(jsonAsset.text);
        if (db == null || db.dialogues == null || db.dialogues.Length == 0)
        {
            Debug.LogError("No dialogues found in JSON. Check the file format.");
            return;
        }

        EnsureFolderExists(OutputFolder);

        int created = 0;
        int updated = 0;

        foreach (DialogueJsonData dialogue in db.dialogues)
        {
            if (string.IsNullOrWhiteSpace(dialogue.id))
            {
                Debug.LogWarning("Skipped dialogue with empty id.");
                continue;
            }

            string safeName = MakeSafeAssetName(dialogue.id);
            string assetPath = $"{OutputFolder}/{safeName}.asset";

            DialogueSO dialogueAsset = AssetDatabase.LoadAssetAtPath<DialogueSO>(assetPath);
            bool isNew = dialogueAsset == null;

            if (isNew)
            {
                dialogueAsset = ScriptableObject.CreateInstance<DialogueSO>();
                AssetDatabase.CreateAsset(dialogueAsset, assetPath);
                created++;
            }
            else
            {
                updated++;
            }

            dialogueAsset.dialogueId = dialogue.id;
            dialogueAsset.lines = ConvertLines(dialogue.lines);
            EditorUtility.SetDirty(dialogueAsset);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Dialogue import complete. Created: {created}, Updated: {updated}, Total in JSON: {db.dialogues.Length}.");
    }

    private static DialogueLineData[] ConvertLines(DialogueJsonLine[] sourceLines)
    {
        if (sourceLines == null || sourceLines.Length == 0)
            return Array.Empty<DialogueLineData>();

        DialogueLineData[] result = new DialogueLineData[sourceLines.Length];
        for (int i = 0; i < sourceLines.Length; i++)
        {
            DialogueJsonLine src = sourceLines[i];
            result[i] = new DialogueLineData
            {
                speaker = src.speaker,
                text = src.text
            };
        }

        return result;
    }

    private static string MakeSafeAssetName(string value)
    {
        string fileName = value;
        foreach (char invalid in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalid, '_');
        }

        return fileName;
    }

    private static void EnsureFolderExists(string targetFolder)
    {
        if (AssetDatabase.IsValidFolder(targetFolder))
            return;

        string[] parts = targetFolder.Split('/');
        string current = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string next = $"{current}/{parts[i]}";
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, parts[i]);
            }

            current = next;
        }
    }

    [Serializable]
    private class DialogueDatabaseJson
    {
        public DialogueJsonData[] dialogues;
    }

    [Serializable]
    private class DialogueJsonData
    {
        public string id;
        public DialogueJsonLine[] lines;
    }

    [Serializable]
    private class DialogueJsonLine
    {
        public string speaker;
        public string text;
    }
}
