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
        //�ӿ������Ǳ�׼���ġ����������������ꡣ��������½�Ϊ (0,0)�����Ͻ�Ϊ (1,1)��
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
