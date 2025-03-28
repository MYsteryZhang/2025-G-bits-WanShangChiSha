using UnityEngine;

public class Interactable : MonoBehaviour
{
    //���пɻ�����Ʒ�Ļ���
    public string interactionPrompt = "�� F ����";
    public bool isInteractable = true;

    public bool IsInteractable()
    {
        return isInteractable;
    }
    public virtual void OnInteract(GameObject player)
    {
    }
}
