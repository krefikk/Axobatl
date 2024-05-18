using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject reButton;
    Animator reButtonAnim;
    TextMeshProUGUI reButtonText;
    public GameObject mainMenuButton;
    Animator mainMenuAnim;
    public GameObject highScore;
    Animator highScoreAnim;
    public GameObject title;
    TextMeshProUGUI titleText;
    Animator titleAnim;
    public GameObject score;
    TextMeshProUGUI scoreText;
    Animator scoreAnim;
    public Animator thanksAnim;
    bool restartingGame = false;
    bool returningMainMenu = false;

    private void Awake()
    {
        titleText = title.GetComponent<TextMeshProUGUI>();
        titleAnim = title.GetComponent<Animator>();
        scoreText = score.GetComponent<TextMeshProUGUI>();
        scoreAnim = score.GetComponent<Animator>();
        highScoreAnim = highScore.GetComponent<Animator>();
        reButtonText = reButton.GetComponentInChildren<TextMeshProUGUI>();
        reButtonAnim = reButton.GetComponent<Animator>();
        mainMenuAnim = mainMenuButton.GetComponent<Animator>();
        OpenScene();
    }

    void SetHighScore() 
    {
        if (PlayerController.player.highScoreUpdated) 
        {
            highScore.SetActive(true);
        }
    }

    public void OpenScene() 
    {
        SetHighScore();
        if (PlayerController.player.finishedGame)
        {
            titleText.text = "YOU WON!";
            reButtonText.text = "Restart";
        }
        else 
        {
            titleText.text = "YOU LOSE!";
            reButtonText.text = "Retry";
        }
        scoreText.text = "Score: " + ((int) PlayerController.player.GetScore()).ToString();
    }

    IEnumerator CloseSceneCO() 
    {
        reButtonAnim.Play("reExit");
        yield return new WaitForSeconds(0.5f);
        mainMenuAnim.Play("mmExit");
        yield return new WaitForSeconds(0.5f);
        if (highScore.activeSelf) 
        {
            highScoreAnim.Play("highScoreExit");
            yield return new WaitForSeconds(0.5f);
        }
        thanksAnim.Play("thanksExit");
        yield return new WaitForSeconds(0.5f);
        scoreAnim.Play("scoreExit");
        yield return new WaitForSeconds(0.5f);
        titleAnim.Play("titleExit");
        yield return new WaitForSeconds(0.5f);
        if (restartingGame) 
        {
            PlayerController.player.Restart();
            SceneManager.LoadScene("MainGame");
            restartingGame = false;
        }
        if (returningMainMenu) 
        {
            PlayerController.player.Restart();
            SceneManager.LoadScene("MainMenu");
            returningMainMenu = false;
        }
    }

    public void PressedReButton() 
    {
        restartingGame = true;
        StartCoroutine(CloseSceneCO());
    }

    public void PressedMMButton()
    {
        returningMainMenu = true;
        StartCoroutine(CloseSceneCO());
    }

}
