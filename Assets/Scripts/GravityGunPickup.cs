using UnityEngine;

public class GravityGunPickup : Interactable
{
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        GunUpgradeManager.Instance.FirstGetGravityGun();
    }
}
