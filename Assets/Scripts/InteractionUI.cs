using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    private PlayerController playerController;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        promptPanel.SetActive(false);
    }

    void Update()
    {
        Interactable interactable = playerController.GetCurrentInteractable();
        if (interactable != null && interactable.IsInteractable())
        {
            promptPanel.SetActive(true);
            promptText.text = interactable.interactionPrompt;
        }
        else
        {
            promptPanel.SetActive(false);
        }
    }
}