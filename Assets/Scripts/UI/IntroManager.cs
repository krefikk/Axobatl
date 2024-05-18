using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Camera mainCamera;
    public TextMeshProUGUI text;
    public GameObject background;
    public bool finished = false;

    private void Start()
    {
        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the loopPointReached event
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (!finished)
        {
            background.SetActive(false);
            //mainCamera.backgroundColor = Color.black;
            StartCoroutine(DisplayTextIntroCO());
            finished = true;
        }
    }

    IEnumerator DisplayTextIntroCO()
    {
        yield return new WaitForSeconds(1.5f);
        string message = "Somewhere in Oklahoma...";
        for (int i = 0; i <= message.Length; i++)
        {
            text.text = message.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainGame");
    }
}