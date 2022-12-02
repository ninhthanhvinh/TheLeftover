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

    private AudioManager audioManager;

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
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
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
    }

    public void LoadInv()
    {
        inventory.Load();
    }
}
