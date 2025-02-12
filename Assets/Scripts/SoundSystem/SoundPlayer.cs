using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private float timeToFade = 0.25f;

    private AudioSource _source;
    private AudioSource _musicSource;
    private AudioSource _tempSource;

    private float _musicVolume;
    private void Start ()
    {
        _musicSource = GetComponent<AudioSource>();
    }

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

    public void SetMusic(AudioSource source)
    {
        _musicSource = source;
        _musicVolume = _musicSource.volume;
        FadeOutTrack();
    }

    public void PlayMusic() => _musicSource.Play();
    public void StopMusic() => _musicSource.Stop();

    public void ChangeMusic(AudioSource newSource)
    {
        FadeInTrack();
        _tempSource = _musicSource;
        SetMusic(newSource);
    }

    public void ReturnDefaultMusic() => SetMusic(_tempSource);

    private void FadeInTrack() => StartCoroutine(FadeIn());
    private void FadeOutTrack() => StartCoroutine(FadeOut());
    private IEnumerator FadeIn()
    {
        float timeElapsed = 0;
        if(_musicSource != null)
        {
            while (timeElapsed < timeToFade)
            {
                _musicSource.volume = Mathf.Lerp(_musicVolume, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            StopMusic();
        }
    }

    private IEnumerator FadeOut()
    {
        _musicSource.volume = 0;
        PlayMusic();

        float timeElapsed = 0;
        while(timeElapsed < timeToFade)
        {
            _musicSource.volume = Mathf.Lerp(0, _musicVolume, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}