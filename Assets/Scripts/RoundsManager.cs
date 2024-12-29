using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundsManager : MonoBehaviour
{
    [Header("-----Round System-----")]
    [SerializeField] private int currentRound; 
    [SerializeField] private int enemiesPerRound; 
    [SerializeField] private GameObject[] enemyPrefab; 
    [SerializeField] private Transform[] spawnPoints; 
    [SerializeField] private float spawnDelay; 

    private int enemiesRemaining;

    [SerializeField] private TMP_Text txtRounds;

    [Header("-----Player-----")]
    [SerializeField] private FP player;
    [SerializeField] private TMP_Text txtPoints;

    [Header("-----GameOver-----")]
    [SerializeField] private GameObject gameOver; 
    [SerializeField] private TMP_Text txtGameOverRounds;
    void Start()
    {
        gameOver.SetActive(false);
        StartCoroutine(ShowRound());
        StartCoroutine(RoundLoop());
    }
    void Update()
    {
        if (player.Lives <= 0)
        {
            GameOver();
        }
    }

    private IEnumerator StartNewRound()
    {
        int totalEnemies = enemiesPerRound * currentRound;
        enemiesRemaining = totalEnemies;

        for (int i = 0; i < totalEnemies; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPoint.position, spawnPoint.rotation);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
    private IEnumerator RoundLoop()
    {
        while (true)
        {
            yield return StartCoroutine(StartNewRound());

            while (enemiesRemaining > 0)
            {
                yield return null;
            }
            currentRound++;
            player.Points += 1000;
            txtPoints.text = ("" + player.Points);
            StartCoroutine(ShowRound());
        }
    }
    public void EnemyDefeated()
    {
        enemiesRemaining--;
    }
    private IEnumerator ShowRound()
    {
        txtRounds.alpha = 1; //El texto es visible
        txtRounds.text = ("Round " + currentRound);

        yield return new WaitForSeconds(2f); //Mostrar el texto por 2 segundos

        while (txtRounds.alpha > 0)
        {
            txtRounds.alpha -= Time.deltaTime; //Desaparecer gradualmente
            yield return null;
        }
    }
    private void GameOver()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameOver.SetActive(true);
        txtGameOverRounds.text = ("Sobreviviste " + currentRound + " rondas");
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        //volver a tirar la escena actual.
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
