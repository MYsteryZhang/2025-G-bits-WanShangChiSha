using UnityEngine;

public class PickupGravityGun : Interactable
{
    //捡起重力枪时触发
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        Destroy(gameObject);
        GunUpgradeManager.Instance.FirstGetGravityGun();
    }
}
