using UnityEngine;

public class PickupTimeGun : Interactable
{
    //������ʱ��ǹʱ����
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        Destroy(gameObject);
        GunUpgradeManager.Instance.FirstGetTimeGun();
    }

}
