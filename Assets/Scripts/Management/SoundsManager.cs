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
}
