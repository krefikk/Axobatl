using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;
    public SettingsMenuHandler settings;

    //private FMOD.Studio.Bus FMODBus;

    //private FMODUnity.StudioEventEmitter FMODEmitter;

    string musicBusString = "bus:/Music";
    string sfxBusString = "bus:/SFX";
    FMOD.Studio.Bus musicBus;
    FMOD.Studio.Bus sfxBus;

    public bool menuMusicTracker = false;

    private void Awake()
    {
        if (audioManager == null)
        {
            audioManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicBus = RuntimeManager.GetBus(musicBusString);
        sfxBus = RuntimeManager.GetBus(sfxBusString);

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Change the volume of the music.
    public void ChangeMusicVolume(float newVol)
    {
        musicBus.setVolume(newVol);
    }

    // Change the volume of the SFX.
    public void ChangeSFXVolume(float newVol)
    {
        sfxBus.setVolume(newVol);
    }

    // Stop all music
    public void EndMusic()
    {
        musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public bool getMenuMusicTracker()
    {
        return menuMusicTracker;
    }

    public void setMenuMusicTracker(bool value)
    {
        menuMusicTracker = value;
    }


}
