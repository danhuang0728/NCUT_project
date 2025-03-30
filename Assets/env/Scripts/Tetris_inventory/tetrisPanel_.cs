using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tetrisPanel_ : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject pageObj;
    public bool isInventoryOpen = false;
    void Start()
    {
        inventoryPanel.SetActive(false);
        pageObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        pageObj.SetActive(true);
        isInventoryOpen = true;
        Time.timeScale = 0;
    }
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        pageObj.SetActive(false);
        Time.timeScale = 1;
        isInventoryOpen = false;
    }
}
