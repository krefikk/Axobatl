using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenuManager : MonoBehaviour
{  
    public GameObject start;
    public GameObject settings;
    public GameObject credits;
    public GameObject exit;
    Button startText;
    Button settingsText;
    Button creditsText;
    Button exitText;
    MainMenuTextHandler textHandler;

    void Start()
    {
        startText = start.GetComponent<Button>();
        settingsText = settings.GetComponent<Button>();
        creditsText = credits.GetComponent<Button>();
        exitText = exit.GetComponent<Button>();
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

    IEnumerator PressedStartButtonCO()
    {
        startText.interactable = false;
        settingsText.interactable = false;
        creditsText.interactable = false;
        exitText.interactable = false;
        textHandler.StartButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.SettingsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.CreditsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.ExitButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        startText.interactable = true;
        settingsText.interactable = true;
        creditsText.interactable = true;
        exitText.interactable = true;
        if (PlayerPrefs.GetInt("SawIntro", 0) == 0)
        {
            SceneManager.LoadScene("Intro");
            PlayerPrefs.SetInt("SawIntro", 1);
        }
        else 
        {
            SceneManager.LoadScene("MainGame");
        }       
    }

    IEnumerator PressedSettingsButtonCO() 
    {
        startText.interactable = false;
        settingsText.interactable = false;
        creditsText.interactable = false;
        exitText.interactable = false;
        textHandler.StartButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.SettingsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.CreditsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.ExitButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        startText.interactable = true;
        settingsText.interactable = true;
        creditsText.interactable = true;
        exitText.interactable = true;
        SceneManager.LoadScene("SettingScene");
    }

    IEnumerator PressedCreditsButtonCO()
    {
        startText.interactable = false;
        settingsText.interactable = false;
        creditsText.interactable = false;
        exitText.interactable = false;
        textHandler.StartButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.SettingsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.CreditsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.ExitButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        startText.interactable = true;
        settingsText.interactable = true;
        creditsText.interactable = true;
        exitText.interactable = true;
        SceneManager.LoadScene("CreditsScene");
    }

    IEnumerator PressedExitButtonCO()
    {
        startText.interactable = false;
        settingsText.interactable = false;
        creditsText.interactable = false;
        exitText.interactable = false;
        textHandler.StartButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.SettingsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.CreditsButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        textHandler.ExitButtonExitAnimation();
        yield return new WaitForSeconds(0.5f);
        startText.interactable = true;
        settingsText.interactable = true;
        creditsText.interactable = true;
        exitText.interactable = true;
        Application.Quit();
    }
}
