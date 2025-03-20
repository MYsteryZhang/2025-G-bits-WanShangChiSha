using UnityEngine;

public class GunMode : MonoBehaviour
{
    [SerializeField] protected Transform fpsCam;
    [SerializeField] protected float interactionRange;

    protected virtual void HandlerInteraction() { }
    protected virtual void OnUpdate() { }

    protected virtual void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            HandlerInteraction();
        }
    }
}
