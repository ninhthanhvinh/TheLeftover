using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    public static bool isPaused = false;
    public static bool isEnded = false;
    public GameObject pauseMenuUI;
    public GameObject timeUI;
    public GameObject endgameMenuUI;
    public GameObject completeSceneUI;
    public static float time;
    public Transform player;
    public Transform point;
    public GameObject healthbarUI;
    public DialogueTrigger dialogue;
    public Animator anim;

    private AudioManager audioManager;
    public DialogueManager dialogueManager;
    int currentSceneIndex;

    private void Awake()
    {
        if(GM == null)
        {
            GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        }
    }

    private void Start()
    {
        time = 180f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        point = GameObject.FindGameObjectWithTag("WinPoint").transform;
        //dialogueManager = DialogueManager.instance;
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

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void CallDialogue(DialogueTrigger trigger)
    {
        if (!trigger.isCalled)
        {
            dialogueManager.StartDialogue(trigger.dialogue);
            trigger.isCalled = true;
        }

    }


    private void FixedUpdate()
    {
        timeUI.GetComponent<TextMeshProUGUI>().SetText(time.ToString());
        time -= Time.fixedDeltaTime;
        if(point == null) point = GameObject.FindGameObjectWithTag("WinPoint").transform;
        var distance = Vector3.Distance(player.position, point.position);
        if (time <= 60)
        {
            anim.SetBool("isOpen", true);
            CallDialogue(dialogue);
            
        }
        
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
        Debug.Log("Tính năng đang phát triển");
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
        Debug.Log(SceneManager.GetActiveScene().name);
        DontDestroyOnLoad(player.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        isEnded = false;
        Time.timeScale = 1f;
        endgameMenuUI.SetActive(false);
        player.GetComponent<PlayerController>().currentHealth = player.GetComponent<PlayerController>().originMaxHealth;
        player.gameObject.SetActive(false);
    }

    public void CompleteGame()
    {
        if (!isEnded)
        {
            isEnded = true;
            Time.timeScale = 0f;
            completeSceneUI.SetActive(true);
            //healthbarUI.SetActive(false);
        }
    }

    public void NextScene()
    {
        isEnded = false;
        player.GetComponent<PlayerController>().SavePlayer();
        DontDestroyOnLoad(player.gameObject);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(audioManager.gameObject);
        DontDestroyOnLoad(timeUI);
        DontDestroyOnLoad(dialogueManager.gameObject);
        DontDestroyOnLoad(GameObject.Find("UI"));
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);
        Time.timeScale = 1f;
        completeSceneUI.SetActive(false);
        player.GetComponent<PlayerController>().LoadPlayer();
        player.position = Vector3.zero;
        time = 180f;
    }
}
