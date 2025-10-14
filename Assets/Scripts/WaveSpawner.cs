using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;

    public Transform spawnPoint;

    public int maxWaves = 3;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private int waveNumber = 1;
    private bool currentWaveComplete = true;

    void Update()
    {
        if(countdown <= 0f && maxWaves > 0)
        {
            currentWaveComplete = false;
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        if (currentWaveComplete)
        {
            countdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave()
    {
        for(int i = 0; i < waveNumber; i++) 
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

        waveNumber++;
        currentWaveComplete = true;
        maxWaves--;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
