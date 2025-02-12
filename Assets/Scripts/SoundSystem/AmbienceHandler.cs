using System.Collections;
using UnityEngine;

namespace SoundSystem
{
    public class AmbienceHandler : MonoBehaviour
    {
        [SerializeField] private float minIntervalInSeconds;
        [SerializeField] private float maxIntervalInSeconds;

        public AudioSource[] ambienceSounds;

        public bool isWorking;

        private SoundPlayer player;
        void Start()
        {
            player = GetComponent<SoundPlayer>();
            isWorking = true;
            StartCoroutine(PlayAmbience());
        }
        public void TurnOffAmbience() => isWorking = false;
        public void TurnOnAmbience() => isWorking = true;
        
        private IEnumerator PlayAmbience()
        {
            while (isWorking)
            {
                var timeInSeconds = Random.Range(minIntervalInSeconds, maxIntervalInSeconds);
                yield return new WaitForSeconds(timeInSeconds);

                player.PlayUIAudio(ambienceSounds[Random.Range(0, ambienceSounds.Length)]);
            }
        }
    }
}