using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/Audio Configuration")]
public class AudioConfig : ScriptableObject
{
    [System.Serializable]
    public class Sound
    {
        public string name;                 // 音效唯一标识
        public AudioClip[] clips;           // 支持多个音频变体
        [Range(0, 1)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop = false;
        public bool isMusic = false;        // 是否为音乐
        [Header("3D Sound Settings")]
        public float spatialBlend = 0f;     // 0=2D, 1=3D
        public float minDistance = 1f;
        public float maxDistance = 500f;
    }

    public List<Sound> sounds = new List<Sound>();
}