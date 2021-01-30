using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsManager : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource endGame;
    public AudioSource takeSoul;
    public AudioSource soulCreation;
    public AudioSource dontKnow;
    public AudioSource godmodeMusic;
    public Slider masterVolumeSlider;
    public float transitionTime = 5;

    public void playEndGame()
    {
        endGame.Play();
    }
    public void playTakeSoul()
    {
        takeSoul.Play();
    }
    public void playSoulCreation()
    {
        soulCreation.Play();
    }
    public void playDontKnow()
    {
        dontKnow.Play();
    }

    public void changeMasterVolume()
    {
        AudioListener.volume = masterVolumeSlider.value;
        PlayerPrefs.SetFloat("volume", masterVolumeSlider.value);
        PlayerPrefs.Save();
        Debug.Log("Master volume = " + masterVolumeSlider.value);
    }

    //used reference from here https://forum.unity.com/threads/fade-out-audio-source.335031/
    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime, bool stop)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        if (stop)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Pause();
        }
        audioSource.volume = startVolume;
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float endVolume = audioSource.volume;
        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < endVolume)
        {
            audioSource.volume += endVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
    }

    public void transition(AudioSource audioIn, AudioSource audioOut, float fadeTime, bool stop)
    {
        StartCoroutine(FadeIn(audioIn, fadeTime));
        StartCoroutine(FadeOut(audioOut, fadeTime, stop));
    }
    
    public void enterGodModeMusic()
    {
        transition(godmodeMusic, bgm, transitionTime / 2, false);
    }
    public void exitGodModeMusic()
    {
        transition(bgm, godmodeMusic, transitionTime, true);
    }
}
