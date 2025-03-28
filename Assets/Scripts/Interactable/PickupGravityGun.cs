using UnityEngine;

public class PickupGravityGun : Pickable
{
    //捡起重力枪时触发
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        GunUpgradeManager.Instance.FirstGetGravityGun();
    }
}
