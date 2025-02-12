using System;
using UnityEngine;
using Unity.Audio;

namespace SoundSystem
{
    public class AudioContainer : MonoBehaviour
    {
        public AudioSource soundFile;
        public AudioProperties properties;

        public AudioContainer()
        {
            if (soundFile == null) return;
            properties.clip = soundFile.clip;
            properties.volume = soundFile.volume;  
            properties.pitch = soundFile.pitch;
            properties.priority = soundFile.priority;
            properties.stereoPan = soundFile.panStereo;
            properties.spatialBlend = soundFile.spatialBlend;
            properties.reverbZoneMix = soundFile.reverbZoneMix;
            properties.dopplerLevel = soundFile.dopplerLevel;
            properties.spread = soundFile.spread;
            properties.volumeRolloff = soundFile.rolloffMode;
            properties.minDistance = soundFile.minDistance;
            properties.maxDistance = soundFile.maxDistance;
        }
        
    }

    public class AudioProperties
    {
        public AudioClip clip;
        public float volume;
        public float pitch;
        public float priority;
        [Range(-1, 1)] public float stereoPan;
        [Range(0, 1)] public float spatialBlend;
        [Range(0, 1.1f)] public float reverbZoneMix;
        [Range(0, 5)] public float dopplerLevel;
        [Range(0, 360)] public float spread;
        public AudioRolloffMode volumeRolloff;
        public float minDistance;
        public float maxDistance;
    }
}
