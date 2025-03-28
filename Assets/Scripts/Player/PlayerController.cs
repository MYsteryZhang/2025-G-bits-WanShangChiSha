using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private float interactDistance = 10f;

    private Camera playerCamera;
    private Interactable currentInteractable;
    private void Start()
    {
        playerCamera = Camera.main;
    }
    private void Update()
    {
        CheckInteractable();
        HandleInteractionInput();
    }

    public Interactable GetCurrentInteractable()
    {
        return currentInteractable;
    }
    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.OnInteract(gameObject);
        }
    }

    private void CheckInteractable()
    {
        //视口坐标是标准化的、相对于摄像机的坐标。摄像机左下角为 (0,0)，右上角为 (1,1)。
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            Interactable interactable = hit.transform.GetComponent<Interactable>();
            if (interactable != null && interactable.isInteractable)
            {
                currentInteractable = interactable;
                return;
            }
        }
        currentInteractable = null;
    }
}
