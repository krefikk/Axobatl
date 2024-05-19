using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public List<GameObject> enemyList = new List<GameObject>();
    public bool waveFinished = false;
    private bool isWaveCoroutineRunning = false;

    private void Update()
    {
        UpdateEnemyList();
        Debug.Log("Wave Finished: " + waveFinished + ". Wave Coroutine Running: " + isWaveCoroutineRunning);
        if (waveFinished && !isWaveCoroutineRunning)
        {
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
                default:
                    Debug.Log("All waves completed.");
                    break;
            }
        }
    }

    void UpdateEnemyList()
    {
        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
            }
        }

        if (enemyList.Count == 0 && PlayerController.player.GetWaveNumber() >= 0)
        {
            waveFinished = true;
            Debug.Log("Wave finished.");
        }
    }

    IEnumerator CreateEnemy(bool isMelee, bool canNecromance)
    {
        Debug.Log("Creating enemy...");
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
        yield return new WaitForSeconds(5);

        foreach (var config in enemyConfigs)
        {
            yield return StartCoroutine(CreateEnemy(config.isMelee, config.canNecromance));
        }

        PlayerController.player.IncreaseWaveNumber(1);
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
