using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using FMODUnity;
using FMOD.Studio;


public class MainMenuManager : MonoBehaviour
{  
    public GameObject start;
    public GameObject settings;
    public GameObject credits;
    public GameObject exit;
    public GameObject htp;
    Button startText;
    Button settingsText;
    Button creditsText;
    Button exitText;
    Button htpText;
    MainMenuTextHandler textHandler;
    public Animator logoAnim;

    void Start()
    {
        startText = start.GetComponent<Button>();
        settingsText = settings.GetComponent<Button>();
        creditsText = credits.GetComponent<Button>();
        exitText = exit.GetComponent<Button>();
        htpText = htp.GetComponent<Button>();
        textHandler = start.GetComponentInParent<MainMenuTextHandler>();
    }

    public void PressedStartButton() 
    {
        StartCoroutine(PressedStartButtonCO());
    }

    public void PressedSettingsButton()
    {
        StartCoroutine(PressedSettingsButtonCO());
    }

    public void PressedCreditsButton()
    {
        StartCoroutine(PressedCreditsButtonCO());
    }

    public void PressedExitButton()
    {
        StartCoroutine(PressedExitButtonCO());
    }

    public void PressedHTPButton() 
    {
        StartCoroutine(PressedHTPButtonCO());
    }

    IEnumerator PressedStartButtonCO()
    {
        startText.interactable = false;
        settingsText.interactable = false;
        creditsText.interactable = false;
        exitText.interactable = false;
        htpText.interactable = false;
        logoAnim.Play("logoExit");
        yield return new WaitForSeconds(0.5f);
        textHandler.StartButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.SettingsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.CreditsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.ExitButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.HTPButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        startText.interactable = true;
        settingsText.interactable = true;
        creditsText.interactable = true;
        exitText.interactable = true;
        htpText.interactable = true;
        if (PlayerPrefs.GetInt("SawIntro", 0) == 0)
        {
            SceneManager.LoadScene("Intro");
            PlayerPrefs.SetInt("SawIntro", 1);
        }
        else 
        {
            SceneManager.LoadScene("GunChoose");
        }       
    }

    IEnumerator PressedSettingsButtonCO() 
    {
        startText.interactable = false;
        settingsText.interactable = false;
        creditsText.interactable = false;
        exitText.interactable = false;
        htpText.interactable = false;
        logoAnim.Play("logoExit");
        yield return new WaitForSeconds(0.5f);
        textHandler.StartButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.SettingsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.CreditsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.ExitButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.HTPButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        startText.interactable = true;
        settingsText.interactable = true;
        creditsText.interactable = true;
        exitText.interactable = true;
        htpText.interactable = true;
        SceneManager.LoadScene("SettingScene");
    }

    IEnumerator PressedCreditsButtonCO()
    {
        startText.interactable = false;
        settingsText.interactable = false;
        creditsText.interactable = false;
        exitText.interactable = false;
        htpText.interactable = false;
        logoAnim.Play("logoExit");
        yield return new WaitForSeconds(0.5f);
        textHandler.StartButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.SettingsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.CreditsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.ExitButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.HTPButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        startText.interactable = true;
        settingsText.interactable = true;
        creditsText.interactable = true;
        exitText.interactable = true;
        htpText.interactable = true;
        SceneManager.LoadScene("CreditsScene");
    }

    IEnumerator PressedExitButtonCO()
    {
        startText.interactable = false;
        settingsText.interactable = false;
        creditsText.interactable = false;
        exitText.interactable = false;
        htpText.interactable = false;
        logoAnim.Play("logoExit");
        yield return new WaitForSeconds(0.5f);
        textHandler.StartButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.SettingsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.CreditsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.ExitButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.HTPButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        startText.interactable = true;
        settingsText.interactable = true;
        creditsText.interactable = true;
        exitText.interactable = true;
        htpText.interactable = true;
        Application.Quit();
    }

    IEnumerator PressedHTPButtonCO()
    {
        startText.interactable = false;
        settingsText.interactable = false;
        creditsText.interactable = false;
        exitText.interactable = false;
        htpText.interactable = false;
        logoAnim.Play("logoExit");
        yield return new WaitForSeconds(0.5f);
        textHandler.StartButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.SettingsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.CreditsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.ExitButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.HTPButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        startText.interactable = true;
        settingsText.interactable = true;
        creditsText.interactable = true;
        exitText.interactable = true;
        htpText.interactable = true;
        SceneManager.LoadScene("HowToPlay");
    }
}
