using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public void Start()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex +1 );
    }
}