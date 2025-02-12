using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SoundSystem
{
    public class LevelMusicDeliver : MonoBehaviour, IMusicDeliver
    {
        [SerializeField] private AudioContainer track;

        [Inject] private SoundPlayer player;

        private void Start()
        {
            //DeliveMusic();
            player.PlayUIAudio(track.soundFile);
        }

        public void DeliveMusic()
        {
            
        }
    }
}
