using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
    public interface IRandomPitchDeliver
    {
        /// <summary>
        /// Play default clip means that you put prefab with sound
        /// in script's field from inspector and don't mention it
        /// in code where you call sound playing
        /// </summary>
        public void DeliveDefaultClip();

        /// <summary>
        /// If you have a various number of clips that you wanna
        /// play from single script than you can use one of them in
        /// params
        /// </summary>
        /// <param name="clip"></param>
        public void DeliveClip(AudioSource clip, float min, float max);
    }
}
