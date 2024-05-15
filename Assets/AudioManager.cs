using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] EventReference gunshot;
    //[SerializeField] GameObject player;
    [SerializeField] PlayerController controller;
    //[SerializeField] float rate;

    public void PlayGunSound()
    {
        //RuntimeManager.PlayOneShotAttached(gunshot, controller.getPlayer());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isAttacking())
        {
            PlayGunSound();
        }
    }
}
