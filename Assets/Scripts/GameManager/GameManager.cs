using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // GameManager Singleton
    public static GameManager gameManager;

    public bool gamePaussed = false;
    private float elapsedTime = 0f;

    public EventReference musicMain;

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
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        RuntimeManager.PlayOneShot(musicMain);
    }

    private void Update()
    {
        if (!gamePaussed)
        {
            elapsedTime += Time.deltaTime;
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

    public void OpenCardsScene() 
    {
        SceneManager.LoadScene("CardChoose");
    }

    public float GetElapsedTime() 
    {
        return elapsedTime;
    }

    public string GetFormattedElapsedTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
