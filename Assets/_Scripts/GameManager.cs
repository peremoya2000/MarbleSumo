using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    [SerializeField]
    private PlayfabManager playfab;
    [SerializeField]
    private GameObject[] powerUpPrefabs;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    private float spawnRange = 9.5f;
    private int enemyCount;
    private short wave = 0;
    private bool playing = true;
    
    // Start is called before the first frame update
    void Start(){
        StartCoroutine(CheckWave());
    }

    public IEnumerator CheckWave() {
        while (playing){
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (enemyCount == 0){
                SpawnEnemyWave(++wave);
                waveText.SetText("Wave: {0}",wave);
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
    /// <summary>
    /// Generate a random point inside the 2d area defined by spawnRange
    /// </summary>
    /// <returns>Random point within spawnRange</returns>
    private Vector3 GenerateSpawnPoint()
    {
        Vector3 spawnPoint= new Vector3(
            Random.Range(-spawnRange,spawnRange),
            0 ,
            Random.Range(-spawnRange,spawnRange));
        return spawnPoint;
    }

    /// <summary>
    /// Spawn the specified ammount of enemies
    /// </summary>
    /// <param name="ammount">Enemies to spawn</param>
    private void SpawnEnemyWave(short ammount) {
        Transform c = GameObject.Find("Center").transform;
        int powerUps= GameObject.FindGameObjectsWithTag("PowerUp").Length;
        for (short i = 0; i < ammount; ++i) {
            int enemy = (int)(Random.Range(0, enemyPrefabs.Length)*Random.Range(0.75f, 1.1f)* Random.Range(0.8f, 1.1f));
            Instantiate(enemyPrefabs[enemy], GenerateSpawnPoint(), c.rotation);
            if (wave % 2 == 0 && i<7 && powerUps<5 && Random.value<(0.3+(0.03*i))){
                int powerUpIndex = Random.Range(0, powerUpPrefabs.Length);
                GameObject o = Instantiate(powerUpPrefabs[powerUpIndex], GenerateSpawnPoint(), c.rotation);
                o.transform.rotation = c.rotation;
            }
        }
        for (short j = 0; j < (short)(wave / 10); ++j) {
            Instantiate(enemyPrefabs[enemyPrefabs.Length-1], GenerateSpawnPoint(), c.rotation);
        }
    }

    public void BackToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void SendScore() {
        playing = false;
        playfab.SendLeaderboard(wave);
    }
    
    public void OnDeath() {
        playfab.GetLeaderboardPosition();
    }

}
