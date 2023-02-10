using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMapObject : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] InventoryObject inventory;
    [SerializeField] InventoryObject equipment;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Transform>().CompareTag("Player"))
        {
            //Lưu trữ dữ liệu nhân vật
            inventory.Save();
            equipment.Save();

            //Chuyển sang scene tiếp theo
            SceneManager.LoadScene(sceneName);
        }
    }
}
