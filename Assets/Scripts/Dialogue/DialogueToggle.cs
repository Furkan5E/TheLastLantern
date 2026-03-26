using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueToggle : MonoBehaviour
{
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private DialogueSO dialogueData;
    [SerializeField] private GameObject hint0;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Player player;
    [SerializeField] private string interactActionName = "Interact";

    private bool playerInRange;
    private InputAction interactAction;
    private bool isSubscribed;

    private void Awake()
    {
        if (hint0 == null)
        {
            Transform qm0 = transform.Find("hint_0");
            if (qm0 != null)
                hint0 = qm0.gameObject;
        }

        SetHintVisible(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            SetHintVisible(true);

            if (playerInput == null)
                playerInput = other.GetComponentInParent<PlayerInput>();

            if (player == null)
                player = other.GetComponentInParent<Player>();

            ResolveAndSubscribeInteractAction();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            SetHintVisible(false);
        }
    }

    private void OnEnable()
    {
        SetHintVisible(false);
        ResolveAndSubscribeInteractAction();
    }

    private void OnDisable()
    {
        SetHintVisible(false);
        UnsubscribeInteractAction();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (!playerInRange)
            return;

        if (dialogueObject == null)
        {
            Debug.LogError("Dialogue object is not assigned.");
            return;
        }

        if (dialogueObject.activeSelf)
            return;

        Dialogue dialogue = dialogueObject.GetComponent<Dialogue>();
        if (dialogue != null)
        {
            dialogue.SetDialogue(dialogueData);
            dialogue.SetInteractAction(interactAction);
        }

        dialogueObject.SetActive(true);
    }

    private void ResolveAndSubscribeInteractAction()
    {
        if (interactAction == null)
            interactAction = ResolveInteractAction();

        if (interactAction == null || isSubscribed)
            return;

        interactAction.performed += OnInteractPerformed;
        isSubscribed = true;
    }

    private void UnsubscribeInteractAction()
    {
        if (interactAction == null || !isSubscribed)
            return;

        interactAction.performed -= OnInteractPerformed;
        isSubscribed = false;
    }

    private InputAction ResolveInteractAction()
    {
        if (playerInput != null && playerInput.actions != null)
        {
            InputAction actionFromPlayerInput = playerInput.actions.FindAction(interactActionName, false);
            if (actionFromPlayerInput != null)
                return actionFromPlayerInput;
        }

        if (player != null && player.input != null)
            return player.input.Player.Interact;

        Player foundPlayer = FindFirstObjectByType<Player>();
        if (foundPlayer != null && foundPlayer.input != null)
        {
            player = foundPlayer;
            return player.input.Player.Interact;
        }

        return null;
    }

    private void SetHintVisible(bool isVisible)
    {
        if (hint0 != null)
            hint0.SetActive(isVisible);
    }
}
