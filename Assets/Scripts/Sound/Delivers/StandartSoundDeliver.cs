using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SoundSystem
{   
    public class StandartSoundDeliver : MonoBehaviour, IStandartDeliver
    {
        [Inject] SoundPlayer soundPlayer;
        [SerializeField] private AudioSource defaultClip;

        public void DeliveDefaultClip()
        {
            soundPlayer.PlayAudio(defaultClip, transform.position);
        }

        public void DeliveClip(AudioSource clip)
        {
            soundPlayer.PlayAudio(clip, transform.position);
        }
    }
}
