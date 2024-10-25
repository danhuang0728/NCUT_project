using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapControll : MonoBehaviour
{
    private Rigidbody2D trapRig;
    private Animator trapAni;
    public bool close;
    void Start()
    {
        trapRig = GetComponent<Rigidbody2D>();
        trapAni = GetComponent<Animator>();
        trapRig.simulated = false;
        trapAni.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (close == true){
            trapRig.simulated = true;
            trapAni.enabled = true;
        }
        if (close == false){
            
            trapRig.simulated = false;
            trapAni.enabled = false;       
        }
    }
}
