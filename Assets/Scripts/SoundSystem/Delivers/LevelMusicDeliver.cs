using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SoundSystem
{
    public class LevelMusicDeliver : MonoBehaviour, IMusicDeliver
    {
        [SerializeField] private AudioContainer track;
        private AudioSource source;
        [Inject] private SoundPlayer player;

        private void Start()
        {
            //DeliveMusic();
            //DeliveMusic();
            source = GetComponent<AudioSource>();
            //track.CopyPastePropertiesTo(source);
            source.Play();
            //track.CopyPastePropertiesTo(source);
            //source.Play();
        }

        public void DeliveMusic()
        {
            player.SedMusic(track);
        }
    }
}
