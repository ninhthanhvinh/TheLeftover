using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneExit : MonoBehaviour
{
    public string sceneName;
    GameManager GM;
    private void Start()
    {
        GM = GameManager.GM;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GM.NextScene();
        //SceneManager.LoadScene(sceneName);
        
    }
}
