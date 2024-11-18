using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int levels;
    [SerializeField] private int roundsxLevel;
    [SerializeField] private int spawnxRound;
    [SerializeField] private float waitLevels;
    [SerializeField] private float waitRounds;
    [SerializeField] private float waitSpawns;
    void Start()
    {
        StartCoroutine(SpawnSystem());
    }
    void Update()
    {
        
    }
    private IEnumerator SpawnSystem()
    {
        for (int i = 0; i < levels; i++)
        {
            for(int j = 0; j < roundsxLevel; j++)
            {
                for (int k = 0; k < spawnxRound; k++)
                {
                    Instantiate(enemyPrefabs, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

                    yield return new WaitForSeconds(waitSpawns);
                }
                //actualizar texto ronda
                yield return new WaitForSeconds(waitRounds);
            }
            //actualizar texto nivel
            yield return new WaitForSeconds(waitLevels);
        }
    }
}
