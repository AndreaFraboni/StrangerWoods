using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGController : MonoBehaviour
{
    private Rigidbody rb;

    private Animator anim;
    private bool isPlayerMoving;

    private float playerSpeed = 0.5f;
    private float rotateSpeed = 4f;

    private float moveHorizontal, moveVertical;

    private float rotY = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        rotY = transform.localRotation.eulerAngles.y;
    }

    private void Update()
    {
        PlayerMoveKeyboard();
    }

    void FixedUpdate()
    {
       MoveAndRotate();
       AnimatePlayer();
    }

    void PlayerMoveKeyboard()
    {
        //*************************************************************************//
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveHorizontal = -1;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            moveHorizontal = 0;
        }
        //*************************************************************************//
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveHorizontal = 1;
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            moveHorizontal = 0;
        }
        //*************************************************************************//
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveVertical = 1;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            moveVertical = 0;
        }
        //*************************************************************************//
    }

    void MoveAndRotate()
    {
        if (moveVertical != 0)
        {
            rb.MovePosition(transform.position + transform.forward * (moveVertical * playerSpeed));
        }
        rotY += moveHorizontal * rotateSpeed;
        rb.rotation = Quaternion.Euler(0f, rotY, 0f);
    }

    void AnimatePlayer()
    {
        if (moveVertical != 0)
        {
            if (!isPlayerMoving)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
                {
                    isPlayerMoving = true;
                    anim.SetTrigger("Walking");
                }
            }
        }
        else
        {
            if (isPlayerMoving)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
                {
                    isPlayerMoving = false;
                    anim.SetTrigger("Stop");
                }
            }
        }

    }

    void Attack()
    {

    }





}
