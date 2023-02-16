using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float cd;
    float count;
    [SerializeField]
    GameObject[] enemiesPrefabs;
    void Start()
    {
        count = cd;
    }

    // Update is called once per frame
    void Update()
    {
        count -= Time.deltaTime;
        if (count <= 0)
        {
            SpawnEnemies();
            count = cd;
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < Random.Range(0, 2); i++)
        {
            Instantiate(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)], this.transform.position, Quaternion.identity);
        }
    }
}
