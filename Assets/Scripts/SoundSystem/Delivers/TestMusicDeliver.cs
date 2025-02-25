using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SoundSystem
{
    public class TestMusicDeliver : MonoBehaviour, IMusicDeliver
    {
        [Inject] private SoundPlayer player;
        [SerializeField] private AudioClip clip;
        [SerializeField] private float volume;

        private void Start()
        {
            
        }

        public void DeliveMusic()
        {
            player.SetTempMusic(clip, volume);
        }

        public void ReturnDefaultMusic()
        {
            player.ReturnDefaultMusic();
        }
    }
}
