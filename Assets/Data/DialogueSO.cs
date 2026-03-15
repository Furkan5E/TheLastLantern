using UnityEngine;

[System.Serializable]
public class DialogueLineData
{
    public string speaker;
    [TextArea(2, 5)] public string text;
}

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string dialogueId;
    public DialogueLineData[] lines;
}
