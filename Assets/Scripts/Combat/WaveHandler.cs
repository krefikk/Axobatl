using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public List<GameObject> enemyList = new List<GameObject>();
    public bool waveFinished = false;

    private void Update()
    {
        UpdateEnemyList();
    }

    void UpdateEnemyList() 
    {
        if (PlayerController.player.GetWaveNumber() > 0 && enemyList.Count == 0) 
        {
            enemyList.Clear();
            waveFinished = true;
            if (PlayerController.player.GetWaveNumber() == 5)
            {
                PlayerController.player.finishedGame = true;
            }
        }
        foreach (GameObject enemy in enemyList) 
        {
            if (enemy.IsDestroyed()) 
            {
                enemyList.Remove(enemy);
            }
        }
    }
    
    void CreateEnemy(bool isMelee, bool canNecromance) 
    {
        GameObject e1 = Instantiate(enemyPrefab, spawnPoints[Random.Range(0, 9)].position, Quaternion.identity);
        Enemy enemy1 = e1.GetComponent<Enemy>();
        enemy1.canNecromance = canNecromance;
        enemy1.canShoot = !isMelee;
        enemy1.meleeEnemy = isMelee;
        enemyList.Add(e1);
    }
    
    void CreateWave1() 
    {
        for (int i = 0; i < 5; i++) 
        {
            CreateEnemy(true, false);
        }
        for (int z = 0; z < 5; z++) 
        {
            CreateEnemy(false, false);
        }
    }
    public void StartWave1() { StartCoroutine(StartWave1Co()); }
    IEnumerator StartWave1Co() 
    {
        yield return new WaitForSeconds(5);
        waveFinished = false;
        CreateWave1();
        PlayerController.player.IncreaseWaveNumber(1);
    }

    void CreateWave2() 
    {
        for (int z = 0; z < 7; z++)
        {
            CreateEnemy(false, false);
        }
        for (int i = 0; i < 1; i++) 
        {
            CreateEnemy(true, true);
        }
    }
    public void StartWave2() { StartCoroutine(StartWave2Co()); }
    IEnumerator StartWave2Co()
    {
        yield return new WaitForSeconds(5);
        waveFinished = false;
        CreateWave2();
        PlayerController.player.IncreaseWaveNumber(1);
    }

    void CreateWave3() 
    {
        for (int z = 0; z < 9; z++)
        {
            CreateEnemy(false, false);
        }
        for (int i = 0; i < 2; i++)
        {
            CreateEnemy(true, true);
        }
    }
    public void StartWave3() { StartCoroutine(StartWave3Co()); }
    IEnumerator StartWave3Co()
    {
        yield return new WaitForSeconds(5);
        waveFinished = false;
        CreateWave3();
        PlayerController.player.IncreaseWaveNumber(1);
    }

    void CreateWave4() 
    {
        for (int z = 0; z < 6; z++)
        {
            CreateEnemy(false, false);
        }
        for (int i = 0; i < 5; i++)
        {
            CreateEnemy(true, true);
        }
    }
    public void StartWave4() { StartCoroutine(StartWave4Co()); }
    IEnumerator StartWave4Co()
    {
        yield return new WaitForSeconds(5);
        waveFinished = false;
        CreateWave4();
        PlayerController.player.IncreaseWaveNumber(1);
    }

    void CreateWave5() 
    {
        for (int z = 0; z < 20; z++)
        {
            CreateEnemy(false, false);
        }
    }
    public void StartWave5() { StartCoroutine(StartWave5Co()); }
    IEnumerator StartWave5Co()
    {
        yield return new WaitForSeconds(5);
        waveFinished = false;
        CreateWave5();
        PlayerController.player.IncreaseWaveNumber(1);
    }

}
