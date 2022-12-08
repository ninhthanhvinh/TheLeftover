using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    public static bool isPaused = false;
    public static bool isEnded = false;
    public GameObject pauseMenuUI;
    public GameObject endgameMenuUI;
    public static float time;
    public Transform player;
    public Transform point;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public float waveSpawnCooldown = 10f;
    public GameObject[] spawnPoints;
    public GameObject[] enemiesPrefabs;
    public GameObject healthbarUI;

    private AudioManager audioManager;
    int enemiesSpawnThisWave;

    private void Awake()
    {
        if(GM == null)
        {
            GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        }
    }

    private void Start()
    {
        time = 600f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        point = GameObject.FindGameObjectWithTag("WinPoint").transform;
        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError("FREAK OUT!!! No audioManager found in this scene.");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }

        waveSpawnCooldown -= Time.deltaTime;
        if(waveSpawnCooldown <= 0)
        {
            enemiesSpawnThisWave += 2;
            SpawnEnemies(enemiesSpawnThisWave);
            waveSpawnCooldown = 10f;
        }
    }

    private void SpawnEnemies(int _enemySpawnThisWay)
    {
        for (int i = 0; i < Random.Range(4, _enemySpawnThisWay + 4); i++)
        {
            Instantiate(enemiesPrefabs[0], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        var distance = Vector3.Distance(player.position, point.position); 
        if (time <= 0)
        {
            if (distance < 20f)
                CompleteGame();
            else
                EndGame();
        };
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        audioManager.PlaySound("BGM");
        healthbarUI.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        healthbarUI.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }


    public void EndGame()
    {
        if(!isEnded)
        {
            isEnded = true;
            Time.timeScale = 0f;
            endgameMenuUI.SetActive(true);
            healthbarUI.SetActive(false);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void CompleteGame()
    {
        Time.timeScale = 0f;
        Debug.Log("congratulations");
        healthbarUI.SetActive(false);
    }

    public void LoadInv()
    {
        inventory.Load();
    }
}
