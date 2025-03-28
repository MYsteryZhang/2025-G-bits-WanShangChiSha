using UnityEngine;

public class Pickable : Interactable
{
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        Destroy(gameObject);
    }
}
