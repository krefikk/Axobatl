using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WholeGameManager : MonoBehaviour
{
    private static WholeGameManager instance;

    // State flags
    bool inGameflow = false;
    bool inMainMenu = true;
    bool inSettingsMenu = false;
    bool inCreditsMenu = false;
    bool inCardsMenu = false;
    bool inIntro = false;

    // Scene managers
    SettingsMenuHandler settingsMenuHandler;
    CreditsMenuManager creditsMenuManager;
    GameManager mainGameManager;
    CardSceneManager cardSceneManager;
    MainMenuManager mainMenuManager;
    IntroManager introManager;

    // Music and sfx
    float sfxVolume = 100; // Max is 100
    float musicVolume = 100; // Max is 100
    bool isTutorialsOn = true; // Default is true


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 100);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 100);
        if (PlayerPrefs.GetInt("IsTutorialsOn", 1) == 1)
        {
            isTutorialsOn = true;
        }
        else 
        {
            isTutorialsOn = false;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("GotWave1Award", 0);
        PlayerPrefs.SetInt("GotWave2Award", 0);
        PlayerPrefs.SetInt("GotWave3Award", 0);
        PlayerPrefs.SetInt("GotWave4Award", 0);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset all flags and managers
        ResetFlagsAndManagers();

        // Check which scene is loaded and set the corresponding flags and managers
        switch (scene.name)
        {
            case "MainMenu":
                Debug.Log("Current scene: Main Menu");
                inMainMenu = true;
                mainMenuManager = FindAnyObjectByType<MainMenuManager>();
                break;
            case "Intro":
                Debug.Log("Current scene: Intro");
                inIntro = true;
                introManager = FindAnyObjectByType<IntroManager>();
                break;
            case "SettingScene":
                Debug.Log("Current scene: Settings");
                inSettingsMenu = true;
                settingsMenuHandler = FindAnyObjectByType<SettingsMenuHandler>();
                break;
            case "CreditsScene":
                Debug.Log("Current scene: Credits");
                inCreditsMenu = true;
                creditsMenuManager = FindAnyObjectByType<CreditsMenuManager>();
                break;
            case "MainGame":
                Debug.Log("Current scene: Main Game");
                inGameflow = true;
                mainGameManager = FindAnyObjectByType<GameManager>();
                break;
            case "CardChoose":
                Debug.Log("Current scene: Cards");
                inCardsMenu = true;
                cardSceneManager = FindAnyObjectByType<CardSceneManager>();
                break;
        }
    }

    private void ResetFlagsAndManagers()
    {
        inSettingsMenu = false;
        settingsMenuHandler = null;

        inCreditsMenu = false;
        creditsMenuManager = null;

        inGameflow = false;
        mainGameManager = null;

        inCardsMenu = false;
        cardSceneManager = null;

        inMainMenu = false;
        mainMenuManager = null;

        PlayerPrefs.Save();
    }

    //---------------------------------------- Getters & Setters --------------------------------------------------
    public float GetSFXVolume() 
    {
        return sfxVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetSFXVolume(float value) 
    {
        sfxVolume = value;
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
    }

    public bool IsTutorialsOn() 
    {
        return isTutorialsOn;
    }

    public void TurnOnOffTutorials(bool value) 
    {
        isTutorialsOn = value;
    }
}