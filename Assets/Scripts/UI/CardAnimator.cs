using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimator : MonoBehaviour
{
    Image image;
    public Sprite[] spritesToAnimate;
    bool animFinished = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        StartCoroutine(CardAnimation());
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

    public bool AnimFinished() 
    {
        return animFinished;
    }
}
