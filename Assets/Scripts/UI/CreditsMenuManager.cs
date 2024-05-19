using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenuManager : MonoBehaviour
{
    public GameObject train;
    bool stopAnim = false;
    public Animator backAnim;
    Animator trainAnim;

    private void Start()
    {
        trainAnim = train.GetComponent<Animator>(); 
        StartCoroutine(CreditsAnimation());    
    }

    public void BackToMainMenu() 
    {
        stopAnim = true;
        StartCoroutine(BackToMM());
    }

    IEnumerator BackToMM() 
    {
        trainAnim.Play("trainExit");
        yield return new WaitForSeconds(0.5f);
        backAnim.Play("backExit");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator CreditsAnimation() 
    {   
        while (train.transform.position.y <= 3200 && !stopAnim) 
        {
            train.transform.position += new Vector3(0, 2, 0);
            yield return new WaitForSeconds(0.025f);
        }
    }
}
