using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimator : MonoBehaviour
{
    Image image;
    public Sprite[] spritesToAnimate;
    bool animFinished = false;
    bool canCloseScene = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        animFinished = false;
        canCloseScene = false;
    }

    void Start()
    {
        StartCoroutine(CardAnimation());
    }

    public void StartCardExitAnimation() 
    {
        StartCoroutine(CardExitAnimation());
    }

    IEnumerator CardAnimation() 
    {
        for (int i = 0; i < spritesToAnimate.Length; i++) 
        {
            image.sprite = spritesToAnimate[i];
            yield return new WaitForSeconds(0.05f);
        }
        animFinished = true;
    }

    IEnumerator CardExitAnimation() 
    {
        for (int i = spritesToAnimate.Length - 1; i >= 0; i--)
        {
            image.sprite = spritesToAnimate[i];
            yield return new WaitForSeconds(0.05f);
        }
        animFinished = false;
        canCloseScene = true;
    }

    public bool AnimFinished() 
    {
        return animFinished;
    }

    public bool CanCloseScene()
    {
        return canCloseScene;
    }
}
