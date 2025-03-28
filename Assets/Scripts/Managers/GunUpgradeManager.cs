using UnityEngine;

public class GunUpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject gravityGun;
    [SerializeField] private GameObject timeGun;
    [SerializeField] private GameObject forceArrowPrefab;
    [SerializeField] protected GameObject player;
    [SerializeField] Transform fpsCam;
    [SerializeField] Transform grabPoint;
    public static GunUpgradeManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FirstGetGravityGun()
    {
        gravityGun.SetActive(true);
        DialogManager.Instance.StartDialogue("GetGravityGun_Prompt", 5f);
    }

    public void FirstGetTimeGun()
    {
        timeGun.SetActive(true);
        GunModeManager.Instance.ActiveSwitchFunction();
    }

    //将基础重力枪升级为进阶重力枪
    public void UpgradeToAdvancedGravityGun()
    {
        BaseGravityGunMode baseGravityGunMode = gravityGun.GetComponent<BaseGravityGunMode>();
        if (baseGravityGunMode != null)
        {
            Destroy(baseGravityGunMode);
        }
        AdvancedGravityGunMode level2 = gravityGun.AddComponent<AdvancedGravityGunMode>();
        level2.SetDefaultValue(forceArrowPrefab,player,fpsCam,grabPoint);
    }


}
