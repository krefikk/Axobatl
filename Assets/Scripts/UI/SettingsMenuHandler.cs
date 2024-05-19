using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuHandler : MonoBehaviour
{
    WholeGameManager wholeGameManager;
    
    public Slider sfxSlider;
    public Slider musicSlider;
    public Toggle checkBox;
    public TextMeshProUGUI musicValue;
    public TextMeshProUGUI sfxValue;
    public Animator backAnim;
    public Animator titleAnim;
    public Animator musicTextAnim;
    public Animator sfxTextAnim;
    public Animator tutorialTextAnim;
    public Animator musicSliderAnim;
    public Animator sfxSliderAnim;
    public Animator checkBoxAnim;
    public Animator musicValueAnim;
    public Animator sfxValueAnim;

    private void Awake()
    {
        wholeGameManager = FindAnyObjectByType<WholeGameManager>();
    }

    private void Start()
    {
        sfxSlider.value = wholeGameManager.GetSFXVolume() / 100;
        musicSlider.value = wholeGameManager.GetMusicVolume() / 100;
        checkBox.isOn = wholeGameManager.IsTutorialsOn();
    }

    private void Update()
    {
        musicValue.text = ((int) wholeGameManager.GetMusicVolume()).ToString();
        sfxValue.text = ((int) wholeGameManager.GetSFXVolume()).ToString();

        AudioManager.audioManager.ChangeMusicVolume(wholeGameManager.GetMusicVolume() / 100);
        AudioManager.audioManager.ChangeSFXVolume(wholeGameManager.GetSFXVolume() / 100);

    }

    public void ClickedCheckBox() 
    {
        wholeGameManager.TurnOnOffTutorials(!wholeGameManager.IsTutorialsOn());
    }

    public void ChangedSFXSliderValue() 
    {
        wholeGameManager.SetSFXVolume(sfxSlider.value * 100);
    }

    public void ChangedMusicSliderValue()
    {
        wholeGameManager.SetMusicVolume(musicSlider.value * 100);
    }

    public void MusicTextEnterAnimation()
    {
        musicTextAnim.Play("musicVolumeTextEnter");
    }

    public void MusicTextExitAnimation()
    {
        musicTextAnim.Play("musicVolumeTextExit");
    }

    public void SFXTextEnterAnimation()
    {
        sfxTextAnim.Play("sfxVolumeTextEnter");
    }

    public void SFXTextExitAnimation()
    {
        sfxTextAnim.Play("sfxVolumeTextExit");
    }

    public void TutorialTextEnterAnimation()
    {
        tutorialTextAnim.Play("tutorialsEnter");
    }

    public void TutorialTextExitAnimation()
    {
        tutorialTextAnim.Play("tutorialsExit");
    }

    public void TitleEnterAnimation()
    {
        titleAnim.Play("titleEnter");
    }

    public void TitleExitAnimation()
    {
        titleAnim.Play("titleExit");
    }

    public void MusicValueEnterAnimation()
    {
        musicValueAnim.Play("musicVolumeValueEnter");
    }

    public void MusicValueExitAnimation()
    {
        musicValueAnim.Play("musicVolumeValueExit");
    }

    public void SFXValueEnterAnimation()
    {
        sfxValueAnim.Play("sfxVolumeValueEnter");
    }

    public void SFXValueExitAnimation()
    {
        sfxValueAnim.Play("sfxVolumeValueExit");
    }

    public void MusicSliderEnterAnimation()
    {
        musicSliderAnim.Play("musicSliderEnter");
    }

    public void MusicSliderExitAnimation()
    {
        musicSliderAnim.Play("musicSliderExit");
    }

    public void SFXSliderEnterAnimation()
    {
        sfxSliderAnim.Play("sfxSliderEnter");
    }

    public void SFXSliderExitAnimation()
    {
        sfxSliderAnim.Play("sfxSliderExit");
    }

    public void CheckBoxEnterAnimation()
    {
        checkBoxAnim.Play("checkBoxEnter");
    }

    public void CheckBoxExitAnimation()
    {
        checkBoxAnim.Play("checkBoxExit");
    }

    public void BackEnterAnimation() 
    {
        backAnim.Play("backEnter");
    }
    public void BackExitAnimation()
    {
        backAnim.Play("backExit");
    }

    public void PressedBackButton() 
    {
        StartCoroutine(PressedBackButtonCO());
    }

    IEnumerator PressedBackButtonCO()
    {
        sfxSlider.interactable = false;
        musicSlider.interactable = false;
        checkBox.interactable = false;
        BackExitAnimation();
        yield return new WaitForSeconds(0.25f);
        CheckBoxExitAnimation();
        yield return new WaitForSeconds(0.25f);
        SFXSliderExitAnimation();
        yield return new WaitForSeconds(0.25f);
        MusicSliderExitAnimation();
        yield return new WaitForSeconds(0.25f);
        SFXValueExitAnimation();
        yield return new WaitForSeconds(0.25f);
        MusicValueExitAnimation();
        yield return new WaitForSeconds(0.25f);
        TutorialTextExitAnimation();
        yield return new WaitForSeconds(0.25f);
        SFXTextExitAnimation();
        yield return new WaitForSeconds(0.25f);
        MusicTextExitAnimation();
        yield return new WaitForSeconds(0.25f);
        TitleExitAnimation();
        yield return new WaitForSeconds(0.25f);
        sfxSlider.interactable = true;
        musicSlider.interactable = true;
        checkBox.interactable = true;
        if (PlayerController.player is not null) 
        {
            SceneManager.LoadScene("MainGame");
        }
        SceneManager.LoadScene("MainMenu");
    }
}
