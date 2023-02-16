using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class controller : MonoBehaviour
{
    int skipClickCounter = 0;
    int continueClickCounter = 0;
    // Start is called before the first frame update
    public void NextScene()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void Skip()
    {
        skipClickCounter += 1;
        if(skipClickCounter == 2)
        {
            NextScene();
            skipClickCounter = 0;
        }
    }

    public void Continue()
    {
        continueClickCounter += 1;
        if (continueClickCounter == 3)
        {
            NextScene();
            continueClickCounter = 0;
        }
    }
}
