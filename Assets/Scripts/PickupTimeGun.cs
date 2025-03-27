using UnityEngine;

public class PickupTimeGun : Interactable
{
    //当捡起时间枪时触发
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        Destroy(gameObject);
        GunUpgradeManager.Instance.FirstGetTimeGun();
    }

}
