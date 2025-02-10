using UnityEngine;

public class SoundPlayer : MonoBehaviour
    {
        private AudioSource _source;

        public void PlayAudio(AudioSource clip, Vector3 position)
        {
            _source = clip;
            AudioSource source = Instantiate(_source, position, Quaternion.identity);
            source.Play();

            var time = source.clip.length;
            Destroy(source.gameObject, time);
        }

        public void PlayUIAudio(AudioSource clip)
        {
            _source = clip;
            AudioSource source = Instantiate(_source);
            source.Play();

            var time = source.clip.length;
            Destroy(source.gameObject, time);
        }
    }
