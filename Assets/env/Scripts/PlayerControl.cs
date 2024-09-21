using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float speed = 5f;
    private float InputX;
    private float InputY;
    private bool isFlip = false;

    private Rigidbody2D rig;
    private Animator ani;

    private void Start() 
    {
        rig = GetComponent<Rigidbody2D>();    
        ani = GetComponent<Animator>();
    }
    private void Update() 
    {
        rig.velocity = new Vector2(speed * InputX , speed * InputY);    
       
        if (math.abs(rig.velocity.x) > 0 || rig.velocity.y != 0)
        {
            ani.SetBool("move",true);
        }
        else
        {
            ani.SetBool("move",false);
        }

        if (!isFlip)
        {
            if (rig.velocity.x > 0)
            {
                isFlip = true;
                transform.Rotate(0.0f,180.0f,0.0f);
            }
        }
        else
        {
            if (rig.velocity.x < 0)
            {
                isFlip = false;
                transform.Rotate(0.0f,180.0f,0.0f);
            }
        }
      

    }
    public void Move(InputAction.CallbackContext context)
    {
        InputX = context.ReadValue<Vector2>().x;
        InputY = context.ReadValue<Vector2>().y;
    }
}
