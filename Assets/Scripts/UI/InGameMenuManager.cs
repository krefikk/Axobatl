using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour
{
    WholeGameManager wholeGameManager;
    public Slider sfxSlider;
    public Slider musicSlider;

    private void Awake()
    {
        wholeGameManager = FindAnyObjectByType<WholeGameManager>();
    }

    private void Start()
    {
        sfxSlider.value = wholeGameManager.GetSFXVolume() / 100;
        musicSlider.value = wholeGameManager.GetMusicVolume() / 100;
    }

    private void Update()
    {
        AudioManager.audioManager.ChangeMusicVolume(wholeGameManager.GetMusicVolume() / 100);
        AudioManager.audioManager.ChangeSFXVolume(wholeGameManager.GetSFXVolume() / 100);
    }

    public void ChangedSFXSliderValue()
    {
        wholeGameManager.SetSFXVolume(sfxSlider.value * 100);
    }

    public void ChangedMusicSliderValue()
    {
        wholeGameManager.SetMusicVolume(musicSlider.value * 100);
    }
}
