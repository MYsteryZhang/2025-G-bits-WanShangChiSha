using UnityEngine;

public class PickupGravityGunUpgrade : Interactable
{
    //当捡起重力枪升级组件时触发
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        Destroy(gameObject);
        GunUpgradeManager.Instance.UpgradeToAdvancedGravityGun();
    }
}
