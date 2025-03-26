using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private int initialPoolSize = 5;

    private Dictionary<string, AudioConfig.Sound> soundDictionary;
    private AudioSource musicSource;
    private List<AudioSource> sfxPool;
    private AudioSource current3DSource;

    // 音量控制
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float musicVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;

    // 淡入淡出参数
    private Coroutine musicFadeCoroutine;
    private const float FADE_DURATION = 1.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        // 1. 创建字典：键为声音名称（string），值为Sound配置对象
        soundDictionary = new Dictionary<string, AudioConfig.Sound>();
        // 2. 遍历音频配置中的所有声音
        foreach (var sound in audioConfig.sounds)
        {
            // 3. 以声音名称为键，Sound对象为值存入字典
            soundDictionary[sound.name] = sound;
        }

        // 初始化音乐播放器
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;

        // 初始化对象池
        sfxPool = new List<AudioSource>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            sfxPool.Add(CreateNewAudioSource());
        }
    }

    #region 基础功能
    public void PlaySFX(string soundName)
    {
        if (!soundDictionary.TryGetValue(soundName, out AudioConfig.Sound sound))
        {
            Debug.LogWarning($"Sound {soundName} not found!");
            return;
        }

        AudioSource source = GetAvailableAudioSource();
        ConfigureAudioSource(source, sound);
        source.Play();

        if (!sound.loop)
        {
            StartCoroutine(ReturnToPool(source, sound.clips[0].length));
        }
    }

    public void PlayMusic(string musicName)
    {
        if (!soundDictionary.TryGetValue(musicName, out AudioConfig.Sound music) || !music.isMusic)
        {
            Debug.LogWarning($"Music {musicName} not found!");
            return;
        }

        if (musicFadeCoroutine != null)
            StopCoroutine(musicFadeCoroutine);

        musicFadeCoroutine = StartCoroutine(FadeSwitchMusic(music));
    }

    public void Play3DSound(string soundName, Vector3 position)
    {
        if (!soundDictionary.TryGetValue(soundName, out AudioConfig.Sound sound))
            return;

        GameObject obj = new GameObject("3D Sound");
        obj.transform.position = position;
        AudioSource source = obj.AddComponent<AudioSource>();
        ConfigureAudioSource(source, sound);
        source.spatialBlend = sound.spatialBlend;
        source.minDistance = sound.minDistance;
        source.maxDistance = sound.maxDistance;
        source.Play();

        Destroy(obj, sound.clips[0].length);
    }
    #endregion

    #region 高级功能
    // 随机播放音效变体
    public void PlayRandomizedSFX(string soundName)
    {
        if (!soundDictionary.TryGetValue(soundName, out AudioConfig.Sound sound))
            return;

        if (sound.clips.Length == 0) return;

        AudioSource source = GetAvailableAudioSource();
        source.clip = sound.clips[Random.Range(0, sound.clips.Length)];
        ConfigureAudioSource(source, sound);
        source.Play();

        if (!sound.loop)
        {
            StartCoroutine(ReturnToPool(source, source.clip.length));
        }
    }

    // 动态混音：根据游戏状态调整参数
    public void SetLowPassFilter(float cutoffFrequency)
    {
        AudioLowPassFilter lowPass = GetComponent<AudioLowPassFilter>();
        if (lowPass == null)
            lowPass = gameObject.AddComponent<AudioLowPassFilter>();

        lowPass.cutoffFrequency = cutoffFrequency;
    }

    // 暂停/恢复系统
    public void PauseAll()
    {
        musicSource.Pause();
        foreach (var source in sfxPool)
        {
            if (source.isPlaying) source.Pause();
        }
    }

    public void ResumeAll()
    {
        musicSource.UnPause();
        foreach (var source in sfxPool)
        {
            if (source.isPlaying) source.UnPause();
        }
    }
    #endregion

    #region 辅助方法
    private AudioSource CreateNewAudioSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        return source;
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in sfxPool)
        {
            if (!source.isPlaying) return source;
        }

        // 池不足时动态扩展
        AudioSource newSource = CreateNewAudioSource();
        sfxPool.Add(newSource);
        return newSource;
    }

    private void ConfigureAudioSource(AudioSource source, AudioConfig.Sound sound)
    {
        source.clip = sound.clips[0];
        source.volume = sound.volume * masterVolume * (sound.isMusic ? musicVolume : sfxVolume);
        source.pitch = sound.pitch;
        source.loop = sound.loop;
        source.spatialBlend = sound.spatialBlend;
    }

    private IEnumerator FadeSwitchMusic(AudioConfig.Sound newMusic)
    {
        // 淡出当前音乐
        if (musicSource.isPlaying)
        {
            float startVolume = musicSource.volume;
            for (float t = 0; t < FADE_DURATION; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0, t / FADE_DURATION);
                yield return null;
            }
            musicSource.Stop();
        }

        // 配置并淡入新音乐
        musicSource.clip = newMusic.clips[0];
        musicSource.volume = 0;
        musicSource.Play();

        float targetVolume = newMusic.volume * masterVolume * musicVolume;
        for (float t = 0; t < FADE_DURATION; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, targetVolume, t / FADE_DURATION);
            yield return null;
        }
    }

    private IEnumerator ReturnToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.Stop();
        source.clip = null;
    }
    #endregion

    #region 音量控制
    public void UpdateVolumes()
    {
        musicSource.volume = soundDictionary[musicSource.clip.name].volume * masterVolume * musicVolume;

        foreach (AudioSource source in sfxPool)
        {
            if (source.isPlaying && source.clip != null)
            {
                source.volume = soundDictionary[source.clip.name].volume * masterVolume * sfxVolume;
            }
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }
    #endregion
}