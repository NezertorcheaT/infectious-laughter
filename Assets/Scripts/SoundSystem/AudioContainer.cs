using System;
using UnityEngine;
using Unity.Audio;

namespace SoundSystem
{
    public class AudioContainer : MonoBehaviour
    {
        public AudioSource soundFile;

        private AudioClip _clip;
        private float _volume;
        private float _pitch;
        [Range(0, 256)] private int _priority;
        [Range(-1, 1)] private float _panStereo;
        [Range(0, 1)] private float _spatialBlend;
        [Range(0, 1.1f)] private float _reverbZoneMix;
        [Range(0, 5)] private float _dopplerLevel;
        [Range(0, 360)] private float _spread;
        private AudioRolloffMode _rolloffMode;
        private float _minDistance;
        private float _maxDistance;
        private void Start()
        {
            if(soundFile == null)
            {
                Debug.Log("BAD");
                return;
            }
            _clip = soundFile.clip;
            _volume = soundFile.volume;
            _pitch = soundFile.pitch;
            _priority = soundFile.priority;
            _panStereo = soundFile.panStereo;
            _spatialBlend = soundFile.spatialBlend;
            _reverbZoneMix = soundFile.reverbZoneMix;
            _dopplerLevel = soundFile.dopplerLevel;
            _spread = soundFile.spread;
            _rolloffMode = soundFile.rolloffMode;
            _minDistance = soundFile.minDistance;
            _maxDistance = soundFile.maxDistance;
        }

        public void CopyPastePropertiesTo(AudioSource audioSource)
        {
            audioSource.clip = _clip;
            audioSource.volume = _volume;
            audioSource.pitch = _pitch;
            audioSource.priority = _priority;
            audioSource.panStereo = _panStereo;
            audioSource.spatialBlend = _spatialBlend;
            audioSource.reverbZoneMix = _reverbZoneMix;
            audioSource.dopplerLevel = _dopplerLevel;
            audioSource.spread = _spread;
            audioSource.rolloffMode = _rolloffMode;
            audioSource.minDistance = _minDistance;
            audioSource.maxDistance = _maxDistance;
        }

        public void CopyPastePropertiesFrom(AudioSource audioSource)
        {
            _clip = audioSource.clip;
            _volume = audioSource.volume;
            _pitch = audioSource.pitch;
            _priority = audioSource.priority;
            _panStereo = audioSource.panStereo;
            _spatialBlend = audioSource.spatialBlend;
            _reverbZoneMix = audioSource.reverbZoneMix;
            _dopplerLevel = audioSource.dopplerLevel;
            _spread = audioSource.spread;
            _rolloffMode = audioSource.rolloffMode;
            _minDistance = audioSource.minDistance;
            _maxDistance = audioSource.maxDistance;
        }
        
    }
}
