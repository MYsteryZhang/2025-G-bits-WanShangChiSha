using UnityEngine;

public class Interactable : MonoBehaviour
{
    //所有可互动物品的基类
    public string interactionPrompt = "按 F 互动";
    public bool isInteractable = true;

    public bool IsInteractable()
    {
        return isInteractable;
    }
    public virtual void OnInteract(GameObject player)
    {
    }
}
