using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueToggle : MonoBehaviour
{
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private DialogueSO dialogueData;
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
            if (dialogueObject == null)
            {
                Debug.LogError("Dialogue object is not assigned.");
                return;
            }

            Dialogue dialogue = dialogueObject.GetComponent<Dialogue>();
            if (dialogue != null)
            {
                dialogue.SetDialogue(dialogueData);
            }
            dialogueObject.SetActive(true);
        }
    }
}
