using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenuManager : MonoBehaviour
{
    public void BackToMainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
