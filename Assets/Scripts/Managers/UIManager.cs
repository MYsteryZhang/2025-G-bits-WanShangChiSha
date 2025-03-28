using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private MouseLook mouseLookScript;
    public static UIManager Instance;
    void Awake()
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

    public void TogglePauseMenu(bool _isPaused)
    {
        mouseLookScript.enabled = !_isPaused;
        pauseMenu.SetActive(_isPaused);
    }
}
