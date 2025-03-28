using UnityEngine;

public class PickupTimeGun : Pickable
{
    //当捡起时间枪时触发
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        GunUpgradeManager.Instance.FirstGetTimeGun();
    }

}
