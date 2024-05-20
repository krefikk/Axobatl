using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GunChooseManager : MonoBehaviour
{
    public GameObject texts;
    public EventReference cardLoad;

    private void Start()
    {
        RuntimeManager.PlayOneShot(cardLoad);
        StartCoroutine(DisplayTexts());
    }

    IEnumerator DisplayTexts() 
    {
        yield return new WaitForSeconds(1.5f);
        texts.SetActive(true);
    }

    public void AssignLeftCardToPlayer()
    {
        WholeGameManager.instance.SetGun(0);
        texts.SetActive(false);
        SceneManager.LoadScene("MainGame");
    }

    public void AssignMiddleCardToPlayer()
    {
        WholeGameManager.instance.SetGun(1);
        texts.SetActive(false);
        SceneManager.LoadScene("MainGame");
    }

    public void AssignRightCardToPlayer()
    {
        WholeGameManager.instance.SetGun(2);
        texts.SetActive(false);
        SceneManager.LoadScene("MainGame");
    }
}
