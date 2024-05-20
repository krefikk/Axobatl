using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTextHandler : MonoBehaviour
{
    public Animator startAnim;
    public Animator settingsAnim;
    public Animator creditsAnim;
    public Animator exitAnim;
    public Animator htpAnim;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonEnterAnimation() 
    {
        startAnim.Play("startEnter");
    }

    public void StartButtonExitAnimation()
    {
        startAnim.Play("startExit");
    }

    public void SettingsButtonEnterAnimation()
    {
        settingsAnim.Play("settingsEnter");
    }

    public void SettingsButtonExitAnimation()
    {
        settingsAnim.Play("settingsExit");
    }

    public void CreditsButtonEnterAnimation()
    {
        creditsAnim.Play("creditsEnter");
    }

    public void CreditsButtonExitAnimation()
    {
        creditsAnim.Play("creditsExit");
    }

    public void ExitButtonEnterAnimation()
    {
        exitAnim.Play("exitEnter");
    }

    public void ExitButtonExitAnimation()
    {
        exitAnim.Play("exitExit");
    }

    public void HTPButtonEnterAnimation()
    {
        htpAnim.Play("htpEnter");
    }

    public void HTPButtonExitAnimation()
    {
        htpAnim.Play("htpExit");
    }
}
