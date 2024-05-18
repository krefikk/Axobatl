using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public EventReference handgun;
    //[SerializeField] GameObject player;
    public PlayerController controller;
    //[SerializeField] float rate;

    public void PlayGunSound()
    {
        RuntimeManager.PlayOneShotAttached(handgun, controller.getPlayer());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isShooting())
        {
            PlayGunSound();
        }
    }
}
