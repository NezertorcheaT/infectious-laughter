using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
    public interface IMusicDeliver
    {
        /// <summary>
        /// Play default clip means that you put prefab with sound
        /// in script's field from inspector and don't mention it
        /// in code where you call sound playing
        /// </summary>
        public void DeliveMusic();
    }
}
