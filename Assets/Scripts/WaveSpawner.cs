using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public static int enemiesAlive = 0;

    public Wave[] waves;
    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private int waveNumber = 0;

    private bool gameEnded = false;

    public GameObject gameOverUi;
    public GameObject gameWinUi;

    private void Awake()
    {
        enemiesAlive = 0;
    }

    void Update()
    {
        if (gameEnded)
            return;

        if (PlayerLives.Lives <= 0)
        {
            GameOver(false);
            return;
        }

        if (waveNumber >= waves.Length && enemiesAlive == 0)
        {
            GameOver(true);
            return;
        }

        if (enemiesAlive > 0)
        {
            return;
        }

        if(countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveNumber];

        int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        Transform chosenSpawnPoint = spawnPoints[randomSpawnPoint];

        for (int i = 0; i < wave.count; i++) 
        {
            SpawnEnemy(wave.enemy, chosenSpawnPoint);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveNumber++;

    }

    void SpawnEnemy(GameObject enemy, Transform spawnPoint)
    {

        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        enemiesAlive++;
    }

    void GameOver(bool won)
    {
        gameEnded = true;

        if (won)
        {
            gameWinUi.SetActive(true);
        }
        else
        {
            gameOverUi.SetActive(true);
        }
    }
}
