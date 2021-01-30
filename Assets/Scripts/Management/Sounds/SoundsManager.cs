using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource endGame;
    public AudioSource takeSoul;
    public AudioSource soulCreation;
    public AudioSource dontKnow;
    public AudioSource godmodeMusic;
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

    //used reference from here https://forum.unity.com/threads/fade-out-audio-source.335031/
    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
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

    public void transition(AudioSource audioIn, AudioSource audioOut, float fadeTime)
    {
        StartCoroutine(FadeIn(audioIn, fadeTime));
        StartCoroutine(FadeOut(audioOut, fadeTime));
    }
    
    public void enterGodModeMusic()
    {
        transition(godmodeMusic, bgm, transitionTime);
    }
    public void exitGodModeMusic()
    {
        transition(bgm, godmodeMusic, transitionTime);
    }
}
