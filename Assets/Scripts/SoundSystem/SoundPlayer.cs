using SoundSystem;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private float timeToFade = 2.25f;

    private AudioSource _source;
    private AudioSource _musicSource;
    private AudioClip _mainMusic;
    private AudioClip _tempMusic;

    private float _musicVolume;
    private float _tempMusicVolume;
    private void Start ()
    {
        _musicSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioSource soundSource, Vector3 position)
    {
        _source = soundSource;
        AudioSource source = Instantiate(_source, position, Quaternion.identity);
        source.Play();

        var time = source.clip.length;
        Destroy(source.gameObject, time);
    }

    public void PlayUIAudio(AudioSource soundSource)
    {
        _source = soundSource;
        AudioSource source = Instantiate(_source);
        source.Play();

        var time = source.clip.length;
        Destroy(source.gameObject, time);
    }
    
    public void SetMainMusic(AudioClip music, float volume)
    {
        _musicSource.clip = music;
        _musicVolume = volume;
        FadeOutTrack();
    }
    public void SetTempMusic(AudioClip music, float volume)
    {
        FadeInTrack();
        _musicSource.clip = music;
        _tempMusicVolume = _musicVolume;
        _musicVolume = volume;
        FadeOutTrack();
    }
    public void PlayMusic() => _musicSource.Play();
    public void StopMusic() => _musicSource.Stop();

    public void ChangeMusic(AudioContainer audioContainer)
    {
        FadeInTrack();
        
    }
    public void ReturnDefaultMusic() => SetMainMusic(_tempMusic, _tempMusicVolume);

    private void FadeInTrack() => StartCoroutine(FadeIn());
    private void FadeOutTrack() => StartCoroutine(FadeOut());
    private IEnumerator FadeIn()
    {
        float timeElapsed = 0;
        while (timeElapsed < timeToFade)
        {
            _musicSource.volume = Mathf.Lerp(_musicVolume, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        StopMusic();
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