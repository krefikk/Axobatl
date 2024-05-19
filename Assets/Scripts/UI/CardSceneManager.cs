using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSceneManager : MonoBehaviour
{

    public EventReference cardLoad;

    // Start is called before the first frame update
    void Start()
    {
        RuntimeManager.PlayOneShot(cardLoad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
