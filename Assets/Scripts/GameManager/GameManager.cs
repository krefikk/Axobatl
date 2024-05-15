using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // GameManager Singleton
    public static GameManager gameManager;

    public bool gamePaussed = false;

    private void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gameManager = this;
        }
    }

    public void StopGame() 
    {
        if (!gamePaussed) 
        {
            gamePaussed = true;
        }
    }

    public void ContinueGame() 
    {
        if (gamePaussed)
        {
            gamePaussed = false;
        }
    }
}
