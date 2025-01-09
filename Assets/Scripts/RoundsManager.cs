using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundsManager : MonoBehaviour
{
    [Header("-----Round System-----")]
    [SerializeField] private int currentRound; 
    //[SerializeField] private int enemiesPerRound; 
    [SerializeField] private GameObject[] enemyPrefab; 
    [SerializeField] private Transform[] spawnPoints; 

    private int enemiesRemaining;

    [SerializeField] private TMP_Text txtRounds;

    [Header("-----Player-----")]
    [SerializeField] private FP player;
    [SerializeField] private TMP_Text txtPoints;

    [Header("-----GameOver-----")]
    [SerializeField] private GameObject gameOver; 
    [SerializeField] private TMP_Text txtGameOverRounds;
    private bool isGameover = false;
    void Start()
    {
        gameOver.SetActive(false);
        StartNewGame();
    }
    void Update()
    {
        if (!isGameover && player.Lives <= 0)
        {
            GameOver();
        }
    }
    private int CalculateEnemies(int round)
    {
        //enemigos que empiezan en la ronda 1.
        float baseEnemies = 10;
        //como crece la aparicion de zombies.
        float enemiesGrowth = 1.15f;
        //MATHF.POW ---> eleva un numero (baseEnemies) a una potencia (dentro de Pow) se usa round - 1 para crecer exponencialmente
        //CeilToInt ---> redondea el resultado al numero mas cercano hacia arriba
        return Mathf.CeilToInt(baseEnemies * Mathf.Pow(enemiesGrowth, round - 1));
    }
    private float CalculateSpawnDelay(int round)
    {
        //tiempo para que se hagan todos los spawns
        float baseTime = 20f;
        int enemies = CalculateEnemies(round);
        return baseTime / enemies;
    }
    private IEnumerator StartNewRound()
    {
        int totalEnemies = CalculateEnemies(currentRound);
        enemiesRemaining = totalEnemies;
        float spawnDelay = CalculateSpawnDelay(currentRound);

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
    private void StartNewGame()
    {
        StopAllCoroutines();
        currentRound = 1;
        StartCoroutine(ShowRound());
        StartCoroutine(RoundLoop());
    }
    public void EnemyDefeated()
    {
        enemiesRemaining--;
    }
    private IEnumerator ShowRound()
    {
        txtRounds.alpha = 1; //El texto es visible
        txtRounds.text = ("Round " + currentRound);

        yield return new WaitForSecondsRealtime(2f); //Mostrar el texto por 2 segundos en tiempo real

        while (txtRounds.alpha > 0)
        {
            txtRounds.alpha -= Time.unscaledDeltaTime; //Desaparecer gradualmente
            yield return null;
        }
    }
    private void GameOver()
    {
        isGameover = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameOver.SetActive(true);
        txtGameOverRounds.text = ("You Survive " + currentRound + " Rounds");
    }
    public void Retry()
    {
        StartNewGame();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

        //coger que la escena actual es la del juego.
        //volver a tirar la escena actual.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
