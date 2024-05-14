using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    protected bool gamePaussed = false;

    public void StopGame() 
    {
        if (!gamePaussed) 
        {
            gamePaussed = true;
            Time.timeScale = 0;
        }
    }

    public void ContinueGame() 
    {
        if (gamePaussed)
        {
            gamePaussed = false;
            Time.timeScale = 1;
        }
    }
}
