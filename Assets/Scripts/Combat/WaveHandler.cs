using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveHandler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public List<GameObject> enemyList = new List<GameObject>();
    public bool waveFinished = false;
    private bool isWaveCoroutineRunning = false;
    bool gotWave1Award = false;
    bool gotWave2Award = false;
    bool gotWave3Award = false;
    bool gotWave4Award = false;

    private void Start()
    {
        if (PlayerPrefs.GetInt("GotWave1Award", 0) == 0)
        {
            gotWave1Award = false;
        }
        else 
        {
            gotWave1Award = true;
        }
        if (PlayerPrefs.GetInt("GotWave2Award", 0) == 0)
        {
            gotWave2Award = false;
        }
        else
        {
            gotWave2Award = true;
        }
        if (PlayerPrefs.GetInt("GotWave3Award", 0) == 0)
        {
            gotWave3Award = false;
        }
        else
        {
            gotWave3Award = true;
        }
        if (PlayerPrefs.GetInt("GotWave4Award", 0) == 0)
        {
            gotWave4Award = false;
        }
        else
        {
            gotWave4Award = true;
        }
    }
    private void Update()
    {
        if (!GameManager.gameManager.gamePaussed) 
        {
            Debug.Log("Enemy Count: " + enemyList.Count);
            UpdateEnemyList();
            if (waveFinished && !isWaveCoroutineRunning && enemyList.Count == 0)
            {
                if (PlayerController.player.GetWaveNumber() == 1 && !gotWave1Award)
                {
                    PlayerPrefs.SetInt("GotWave1Award", 1);
                    SceneManager.LoadScene("CardChoose");
                    PlayerController.player.gameObject.SetActive(false);
                    PlayerController.player.inCardsScene = true;
                }
                else if (PlayerController.player.GetWaveNumber() == 2 && !gotWave2Award)
                {
                    PlayerPrefs.SetInt("GotWave2Award", 1);
                    SceneManager.LoadScene("CardChoose");
                    PlayerController.player.gameObject.SetActive(false);
                    PlayerController.player.inCardsScene = true;
                }
                else if (PlayerController.player.GetWaveNumber() == 3 && !gotWave3Award)
                {
                    PlayerPrefs.SetInt("GotWave3Award", 1);
                    SceneManager.LoadScene("CardChoose");
                    PlayerController.player.gameObject.SetActive(false);
                    PlayerController.player.inCardsScene = true;
                }
                else if (PlayerController.player.GetWaveNumber() == 4 && !gotWave4Award)
                {
                    PlayerPrefs.SetInt("GotWave4Award", 1);
                    SceneManager.LoadScene("CardChoose");
                    PlayerController.player.gameObject.SetActive(false);
                    PlayerController.player.inCardsScene = true;
                }
                switch (PlayerController.player.GetWaveNumber())
                {
                    case 0:
                        StartWave1();
                        break;
                    case 1:
                        StartWave2();
                        break;
                    case 2:
                        StartWave3();
                        break;
                    case 3:
                        StartWave4();
                        break;
                    case 4:
                        StartWave5();
                        break;
                }
            }
        }
    }

    void UpdateEnemyList()
    {
        if (enemyList.Count > 0) 
        {
            for (int i = enemyList.Count - 1; i >= 0; i--)
            {
                if (enemyList[i] == null)
                {
                    enemyList.RemoveAt(i);
                }
            }
        }
        else if (enemyList.Count == 0 && PlayerController.player.GetWaveNumber() >= 0 && !waveFinished)
        {
            waveFinished = true;
            Debug.Log("Wave finished.");
        }
    }

    IEnumerator CreateEnemy(bool isMelee, bool canNecromance)
    {
        Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        GameObject e1 = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemy1 = e1.GetComponent<Enemy>();
        enemy1.canNecromance = canNecromance;
        enemy1.canShoot = !isMelee;
        enemy1.meleeEnemy = isMelee;
        enemyList.Add(e1);
        yield return new WaitForSeconds(0.75f);
    }

    IEnumerator CreateWave(List<(bool isMelee, bool canNecromance)> enemyConfigs)
    {
        waveFinished = false;
        isWaveCoroutineRunning = true;
        Debug.Log("Starting wave...");
        PlayerController.player.IncreaseWaveNumber(1);
        yield return new WaitForSeconds(5);
        foreach (var config in enemyConfigs)
        {
            yield return StartCoroutine(CreateEnemy(config.isMelee, config.canNecromance));
        }
        isWaveCoroutineRunning = false;
    }

    void StartWave1()
    {
        Debug.Log("Starting Wave 1");
        StartCoroutine(CreateWave(new List<(bool, bool)>
        {
            (true, false), (true, false), (true, false), (true, false), (true, false),
            (false, false), (false, false), (false, false), (false, false), (false, false)
        }));
    }

    void StartWave2()
    {
        Debug.Log("Starting Wave 2");
        StartCoroutine(CreateWave(new List<(bool, bool)>
        {
            (false, false), (false, false), (false, false), (false, false),
            (false, false), (false, false), (false, false), (true, true)
        }));
    }

    void StartWave3()
    {
        Debug.Log("Starting Wave 3");
        StartCoroutine(CreateWave(new List<(bool, bool)>
        {
            (false, false), (false, false), (false, false), (false, false),
            (false, false), (false, false), (false, false), (false, false),
            (false, false), (true, true), (true, true)
        }));
    }

    void StartWave4()
    {
        Debug.Log("Starting Wave 4");
        StartCoroutine(CreateWave(new List<(bool, bool)>
        {
            (false, false), (false, false), (false, false), (false, false),
            (false, false), (false, false), (true, true), (true, true),
            (true, true), (true, true), (true, true)
        }));
    }

    void StartWave5()
    {
        Debug.Log("Starting Wave 5");
        StartCoroutine(CreateWave(new List<(bool, bool)>
        {
            (false, false), (false, false), (false, false), (false, false),
            (false, false), (false, false), (false, false), (false, false),
            (false, false), (false, false), (false, false), (false, false),
            (false, false), (false, false), (false, false), (false, false),
            (false, false), (false, false), (false, false), (false, false)
        }));
    }
}
