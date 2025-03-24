using System;
using UnityEngine;

public class GunModeManager : MonoBehaviour
{
    //Manager均采用单例模式
    public static GunModeManager Instance { get; private set; }

    [SerializeField] private GameObject Gun;
    [SerializeField] private GameObject GravityGunPrefab;
    [SerializeField] private GameObject TimeGunPrefab;

    private BaseGravityGunMode _currentGravityGunModeScript;
    private BaseTimeGunMode _currentTimeGunModeScript;

    private bool isGravityModeActive = true;

    public delegate void OnGunModeChanged();
    public static event OnGunModeChanged onGunModeChange;

    //确保Manager类只有一个实例，全局可访问
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SetGunMode(isGravityModeActive);
        GravityGunPrefab.SetActive(true);
        TimeGunPrefab.SetActive(false);

        onGunModeChange += SwitchGunPrefab;
    }

    private void SetGunMode(bool isGravityMode)
    {
        if (HasGravityAndTimeMode())
        {
            _currentGravityGunModeScript = Gun.transform.GetComponent<BaseGravityGunMode>();
            _currentTimeGunModeScript = Gun.transform.GetComponent<BaseTimeGunMode>();

            _currentGravityGunModeScript.enabled = isGravityMode;
            _currentTimeGunModeScript.enabled = !isGravityMode;
        }
    }


    private void Update()
    {
        //当玩家按下右键时，禁用当前模式脚本，启用另一模式脚本
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            //只有当且仅当枪械同时拥有两个模式时才能进行模式切换
            if(HasGravityAndTimeMode())
            {
                ChangeGunMode();
            }
        }
        //当松开右键时切换回原来的模式
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (HasGravityAndTimeMode())
            {
                ChangeGunMode();
            }
        }
    }

    private void ChangeGunMode()
    {
        isGravityModeActive = !isGravityModeActive;
        SetGunMode(isGravityModeActive);
        onGunModeChange?.Invoke();
    }

    private bool HasGravityAndTimeMode()
    {
        return Gun.transform.GetComponent<BaseGravityGunMode>() != null && Gun.transform.GetComponent<BaseTimeGunMode>() != null;
    }
    
    private void SwitchGunPrefab()
    {
        TimeGunPrefab.SetActive(!isGravityModeActive);
    }
}
