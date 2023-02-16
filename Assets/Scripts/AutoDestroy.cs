using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float selfDestroyTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        selfDestroyTime -= Time.deltaTime;
        if (selfDestroyTime <= 0)
            Destroy(gameObject);
    }
}
