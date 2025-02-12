using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SoundSystem
{
    public class RandomPitchDeliver : MonoBehaviour, IRandomPitchDeliver
    {
        [Inject] SoundPlayer player;
        [SerializeField] private AudioSource defaultClip;
        [SerializeField] [Range(-3, 3)] private float defaultMinValue;
        [SerializeField] [Range(-3, 3)] private float defaultMaxValue;
        public void DeliveDefaultClip()
        {
            var pitch = Random.Range(defaultMinValue, defaultMaxValue);
            defaultClip.pitch = pitch;
            player.PlayAudio(defaultClip, transform.position);
        }

        public void DeliveClip(AudioSource clip, float min, float max)
        {
            var pitch = Random.Range(min, max);
            clip.pitch = pitch;
            player.PlayAudio(clip, transform.position);
        }
    }
}