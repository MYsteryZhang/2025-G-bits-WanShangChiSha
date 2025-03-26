using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactionPrompt = "°´ F »¥¶¯";
    public bool isInteractable = true;

    public virtual void OnInteract(GameObject player)
    {
        Destroy(gameObject);
    }
}
