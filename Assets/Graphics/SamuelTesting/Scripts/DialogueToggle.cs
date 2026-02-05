using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueToggle : MonoBehaviour
{
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private string dialogueId;
    private bool playerInRange;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (!playerInRange)
            return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Dialogue dialogue = dialogueObject.GetComponent<Dialogue>();
            if (dialogue != null)
            {
                dialogue.SetDialogueId(dialogueId);
            }
            dialogueObject.SetActive(true);
        }
    }
}
