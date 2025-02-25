using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SoundSystem
{
    public class LevelMusicDeliver : MonoBehaviour, IMusicDeliver
    {
        [SerializeField] private AudioClip levelMusic;
        [SerializeField] private float musicVolume;
        [Inject] private SoundPlayer player;

        private void Start()
        {
            DeliveMusic();
        }

        public void DeliveMusic()
        {
            player.SetMainMusic(levelMusic, musicVolume);
        }
    }
}
